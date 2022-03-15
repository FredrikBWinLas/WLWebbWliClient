using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models
{
    public class ExtractHandler
    {
        private readonly XmlFileHandler _xmlFileHandler;
        private string _connectionString = @"Server=FREDRIKB-HP\SQLEXPRESS;Database=WinLasSkelleftea4;Trusted_Connection=False;User Id=sa;Password=Tage2004;";
        //private readonly MissingTeacherHandler _missingTeacherHandler;
        //private readonly TeacherLicenseHandler _teacherLicenseHandler;
        //private readonly TeacherQualificationsHandler _teacherQualificationsHandler;
        public ExtractHandler(string connectionString = null)
        {
            if (!string.IsNullOrEmpty(connectionString)) _connectionString = connectionString;
            //_teacherLicenseHandler = new TeacherLicenseHandler();
            //_teacherQualificationsHandler = new TeacherQualificationsHandler();
            //_repository = new ExtractRepository();
            _xmlFileHandler = new XmlFileHandler();
            //_missingTeacherHandler = new MissingTeacherHandler();
        }

        public void DoXmlFile(string xmlFilePath)
        {
            var licenseService = new LicenseService(_connectionString);
            var qualificationService = new QualificationService(_connectionString);
            var personLicenseService = new PersonLicenseService(_connectionString);
            var personQualificationService = new PersonQualificationService(_connectionString);
            var personService = new PersonService(_connectionString);
            try
            {
                var teacherLicenseExtract = XmlFileExtractHandler.Get(_xmlFileHandler.GetXmlStringFromFile(xmlFilePath));
                if (teacherLicenseExtract == null) return;
                var missingTeacherHandler = new MissingTeacherHandler(licenseService, personLicenseService, personService);
                missingTeacherHandler.DoMissingTeachers(teacherLicenseExtract.ExtractResult.MissingTeachers.MissingTeacher, teacherLicenseExtract.Header.IssuedDate);
                var teacherLicenseHandler = new TeacherLicenseHandler(licenseService, personLicenseService, personService);
                teacherLicenseHandler.DoTeacherLicenses(teacherLicenseExtract.ExtractResult.Teachers.Teacher, teacherLicenseExtract.Header.IssuedDate);
                var teacherQualificationsHandler = new TeacherQualificationsHandler(qualificationService, personQualificationService, personService);
                teacherQualificationsHandler.DoTeacherQualifications(teacherLicenseExtract.ExtractResult.Teachers.Teacher, teacherLicenseExtract.Header.IssuedDate);
            }
            catch(Exception e)
            {
            }

            //_dbContext.Extracts.Add(new Extract
            //{
            //    Id = teacherLicenseExtract.Header.ExtractId,
            //    IssuedDate = teacherLicenseExtract.Header.IssuedDate
            //});
            //_dbContext.SaveChanges();
        }
    }
}
