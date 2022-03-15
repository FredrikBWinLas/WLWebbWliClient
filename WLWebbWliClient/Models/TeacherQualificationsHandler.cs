using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WLWebbWliClient.Models.Db;
using WLWebbWliClient.Models.Xml;
using License = WLWebbWliClient.Models.Db.License;
using Qualification = WLWebbWliClient.Models.Db.Qualification;

namespace WLWebbWliClient.Models
{
    public class TeacherQualificationsHandler
    {
        private readonly QualificationService _qualificationService;
        private readonly PersonQualificationService _personQualificationService;
        private readonly PersonService _personService;
        private List<Qualification> _qualifications;
        public TeacherQualificationsHandler(QualificationService qualificationService, PersonQualificationService personQualificationService, PersonService personService)
        {
            _qualificationService = qualificationService;
            _personQualificationService = personQualificationService;
            _personService = personService;
            _qualifications = _qualificationService.GetAllQualifications();
        }

        private Qualification _getQualification(Xml.Qualification xmlQualification)
        {
            var qualification = _qualifications.FirstOrDefault(x =>
                xmlQualification.Code == x.Code &&
                xmlQualification.SpecializationCode == x.SpecializationCode &&
                xmlQualification.StudyPathCode == x.StudyPathCode &&
                xmlQualification.SubCode == x.SubCode &&
                xmlQualification.TypeOfQualification == x.TypeOfQualification &&
                xmlQualification.TypeOfSchooling == x.TypeOfSchooling &&
                xmlQualification.StudyPathCode == x.StudyPathCode);
            if (qualification == null)
            {
                qualification = new Qualification
                {
                    Code = xmlQualification.Code,
                    SpecializationCode = xmlQualification.SpecializationCode,
                    SubCode = xmlQualification.SubCode,
                    TypeOfQualification = xmlQualification.TypeOfQualification,
                    TypeOfSchooling = xmlQualification.TypeOfSchooling,
                    StudyPathCode = xmlQualification.StudyPathCode,
                    StudyPathName = xmlQualification.StudyPathName,
                    SpecializationName = xmlQualification.SpecializationName,
                    Name = xmlQualification.Name
                };
                _qualifications.Add(_qualificationService.CreateQualification(xmlQualification.TypeOfQualification,
                    xmlQualification.TypeOfSchooling,
                    xmlQualification.Code,
                    xmlQualification.StudyPathCode,
                    xmlQualification.SpecializationCode,
                    xmlQualification.SubCode,
                    xmlQualification.Name,
                    xmlQualification.SpecializationName,
                    xmlQualification.StudyPathName));
            }

            return qualification;
        }

        private List<PersonQualification> _getTeacherQualifications(Teacher teacher, int personId, DateTime issuedDate)
        {
            var personQualifications = new List<PersonQualification>();
            foreach (var xmlQualification in teacher.Qualifications.Qualification)
            {
                var qualification = _getQualification(xmlQualification);

                int toYear = -1;
                int fromYear = -1;

                int.TryParse(xmlQualification.ToYear, out toYear);
                int.TryParse(xmlQualification.FromYear, out fromYear);

                if (personQualifications.Exists(x => x.QualificationId == qualification.Id))
                {
                    var pq = personQualifications.FirstOrDefault(x => x.QualificationId == qualification.Id);
                    pq.FromYear = Math.Min(pq.FromYear, fromYear);
                    pq.ToYear = Math.Min(pq.ToYear, toYear);
                }
                else
                {
                    personQualifications.Add(new PersonQualification
                    {
                        CreatedTime = issuedDate,
                        PersonId =personId,
                        QualificationId = qualification.Id,
                        FromYear = fromYear,
                        ToYear = toYear
                    });
                }
            }

            return personQualifications;
        }

        public void DoTeacherQualifications(IEnumerable<Teacher> teachers, DateTime issuedDate)
        {
            var personQualificationsToAdd = new Dictionary<string, PersonQualification>();
            var personQualificationsToUpdate = new List<PersonQualification>();

            foreach (var teacher in teachers)
            {
                if (!_personService.PersonExists(teacher.SocialSecurityNumber)) continue;

                var personId = _personService.GetPersonId(teacher.SocialSecurityNumber);
                var personQualifications = _getTeacherQualifications(teacher, personId, issuedDate);

                var dbTeacherQualifications = _personQualificationService.GetPersonQualification(personId);

                var removeQualifications = dbTeacherQualifications.Where(pk => !personQualifications.Exists(x =>
                    pk.QualificationId == x.QualificationId)).ToArray();

                foreach (var removeQualification in removeQualifications)
                {
                    if (!removeQualification.DeletedTime.HasValue && issuedDate > removeQualification.CreatedTime)
                    {
                        if (!removeQualification.LastChangedTime.HasValue ||
                            issuedDate > removeQualification.LastChangedTime)
                        {
                            removeQualification.DeletedTime = issuedDate;
                            personQualificationsToUpdate.Add(removeQualification);
                        }
                    }
                }

                foreach (var q in personQualifications)
                {
                    var dbQualification =
                        dbTeacherQualifications.FirstOrDefault(x => x.QualificationId == q.QualificationId);

                    if (dbQualification == null)
                    {
                        var key = q.PersonId + "_" + q.QualificationId;
                        if (personQualificationsToAdd.ContainsKey(key))
                        {
                            var pq = personQualificationsToAdd[key];
                            pq.FromYear = Math.Min(pq.FromYear, q.FromYear);
                            pq.ToYear = Math.Max(pq.ToYear, q.ToYear);
                        }
                        else
                        {
                            personQualificationsToAdd.Add(key, q);
                        }
                    }
                    else if (!dbQualification.LastChangedTime.HasValue || issuedDate > dbQualification.LastChangedTime)
                    {
                         var changed = false;
                        if (q.FromYear != dbQualification.FromYear)
                        {
                            dbQualification.FromYear = q.FromYear;
                            changed = true;
                        }

                        if (q.ToYear != dbQualification.ToYear)
                        {
                            dbQualification.ToYear = q.ToYear;
                            changed = true;
                        }

                        if (changed)
                        {
                            dbQualification.LastChangedTime = issuedDate;
                            personQualificationsToUpdate.Add(dbQualification);
                        }
                    }
                }
            }

            _personQualificationService.InsertPersonQualifications(personQualificationsToAdd.Values.ToList());
            _personQualificationService.UpdatePersonQualifications(personQualificationsToUpdate);
        }
    }
}
