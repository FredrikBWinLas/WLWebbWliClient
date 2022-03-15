using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using WLWebbWliClient.Models.Db;
using WLWebbWliClient.Models.Xml;
using License = WLWebbWliClient.Models.Db.License;

namespace WLWebbWliClient.Models
{
    public class TeacherLicenseHandler
    {
        private readonly LicenseService _licenseService;
        private readonly PersonLicenseService _personLicenseService;
        private readonly PersonService _personService;

        public TeacherLicenseHandler(LicenseService licenseService, PersonLicenseService personLicenseService, PersonService personService)
        {
            _licenseService = licenseService;
            _personLicenseService = personLicenseService;
            _personService = personService;
        }

        public void DoTeacherLicenses(IEnumerable<Teacher> teachers, DateTime issuedDate)
        {
            var personLicensesToAdd = new List<PersonLicense>();
            var personLicensesToUpdate = new List<PersonLicense>();
            var personLicensesToRemove = new List<PersonLicense>();
            foreach (var teacher in teachers)
            {
                if (!_personService.PersonExists(teacher.SocialSecurityNumber))
                {
                    continue;
                }
                var personLicenses = new List<PersonLicense>();
                var personId = _personService.GetPersonId(teacher.SocialSecurityNumber);
                foreach (var personLicense in teacher.License)
                {
                    var license = _licenseService.GetAllLicenses().FirstOrDefault(x => x.Code == personLicense.TypeOflicense);
                    if (license == null)
                    {
                        license = _licenseService.CreateLicense(personLicense.TypeOflicense, personLicense.TypeOflicense);
                    }

                    personLicenses.Add(new PersonLicense
                    {
                        CreatedTime = issuedDate,
                        LastCheckedTime = issuedDate,
                        DateLicenseIssued = personLicense.DateLicenseIssued,
                        PersonId = personId,
                        LicenseId = license.Id
                    });
                }

                var dbPersonLicenses = _personLicenseService.GetPersonLicenses(personId);

                var removeLicenses = dbPersonLicenses
                    .Where(pk => !personLicenses.Exists(ik => ik.LicenseId == pk.LicenseId))
                    .ToArray();

                foreach (var removeLicense in removeLicenses)
                {
                    removeLicense.DeletedTime = issuedDate;
                    personLicensesToUpdate.Add(removeLicense);
                    //System.IO.File.AppendAllText(_logRemoveLicenseFileName, "DEL " + removeLicense.SocialSecurityNumber + " " + removeLicense.TypeOflicense + "\n");
                }

                if (personLicenses.Any())
                {
                    var dbMissingTeacher =
                        dbPersonLicenses.FirstOrDefault(x => x.LicenseId == _licenseService.MissingTeacherLicense.Id);
                    if (dbMissingTeacher != null)
                    {
                        personLicensesToRemove.Add(dbMissingTeacher);
                    }

                }

                foreach (var personLicense in personLicenses)
                {
                    var dbLicense = dbPersonLicenses.FirstOrDefault(x =>
                        x.LicenseId == personLicense.LicenseId);
                    if (dbLicense == null)
                    {
                        personLicensesToAdd.Add(personLicense);
                    }
                    else if (personLicense.LastCheckedTime > dbLicense.LastCheckedTime)
                    {
                        var dbChange = false;
                        if (personLicense.DateLicenseIssued.Value.Year >= 1900 &&
                            dbLicense.DateLicenseIssued != personLicense.DateLicenseIssued)
                        {
                            dbLicense.DateLicenseIssued = personLicense.DateLicenseIssued;
                            dbChange = true;
                        }

                        if (dbLicense.LastCheckedTime < personLicense.LastCheckedTime)
                        {
                            dbLicense.LastCheckedTime = personLicense.LastCheckedTime;
                            dbChange = true;
                        }

                        if (dbChange) personLicensesToUpdate.Add(dbLicense);
                    }
                }
            }

            _personLicenseService.InsertPersonLicenses(personLicensesToAdd);
            _personLicenseService.UpdatePersonLicenses(personLicensesToUpdate);
            _personLicenseService.RemovePersonLicenses(personLicensesToRemove);
        }
    }
}

