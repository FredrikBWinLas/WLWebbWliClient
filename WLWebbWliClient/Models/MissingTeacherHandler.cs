using System;
using System.Collections.Generic;
using System.Linq;
using WLWebbWliClient.Models.Db;
using WLWebbWliClient.Models.Xml;

namespace WLWebbWliClient.Models
{
    public class MissingTeacherHandler
    {
        private readonly LicenseService _licenseService;
        private readonly PersonLicenseService _personLicenseService;
        private readonly PersonService _personService;

        public MissingTeacherHandler(LicenseService licenseService, PersonLicenseService personLicenseService, PersonService personService)
        {
            _licenseService = licenseService;
            _personLicenseService = personLicenseService;
            _personService = personService;
        }
        public void DoMissingTeachers(IEnumerable<MissingTeacher> missingTeachers, DateTime issuedDate)
        {
            var personLicensesToAdd = new List<PersonLicense>();
            var personLicensesToUpdate = new List<PersonLicense>();

            var licenseMissingTeacherId = _licenseService.MissingTeacherLicense.Id;
            foreach (var missingTeacher in missingTeachers)
            {
                if (!_personService.PersonExists(missingTeacher.SocialSecurityNumber)) continue;

                var personId = _personService.GetPersonId(missingTeacher.SocialSecurityNumber);
                var dbPersonLicense = _personLicenseService.GetPersonLicenses(personId).FirstOrDefault(x => x.PersonId == personId &&
                    x.LicenseId == licenseMissingTeacherId);

                if (dbPersonLicense == null)
                {
                    personLicensesToAdd.Add(new PersonLicense
                    {
                        PersonId = personId,
                        LicenseId = licenseMissingTeacherId,
                        CreatedTime = issuedDate,
                        LastCheckedTime = issuedDate
                    });
                }
                else if (issuedDate > dbPersonLicense.LastCheckedTime)
                {
                    dbPersonLicense.LastCheckedTime = issuedDate;
                    personLicensesToUpdate.Add(dbPersonLicense);
                }
            }
            _personLicenseService.InsertPersonLicenses(personLicensesToAdd);
            _personLicenseService.UpdatePersonLicenses(personLicensesToUpdate);
        }
    }
}