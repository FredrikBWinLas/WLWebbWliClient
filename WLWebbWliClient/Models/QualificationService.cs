using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using WLWebbWliClient.Models.Db;

namespace WLWebbWliClient.Models
{
    public class QualificationService
    {
        private readonly string _connectionString;
        private List<Qualification> _allQualifications = null;

        public QualificationService(string connectionString)
        {
            _connectionString = connectionString;
            _allQualifications = GetAllQualifications();
        }


        public List<Qualification> GetAllQualifications()
        {
            if (_allQualifications == null)
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        var query = @"SELECT * FROM WLI_Qualification";
                        _allQualifications = (conn.Query<Qualification>(query)).ToList();
                    }
                }
                catch
                {
                    _allQualifications = new List<Qualification>();
                }
            }

            return _allQualifications;
        }

        public Qualification CreateQualification(string typeOfQualification,
            string typeOfSchooling,
            string code,
            string studyPathCode,
            string specializationCode,
            string subCode,
            string name,
            string specializationName,
            string studyPathName)
        {
            var qualification = GetAllQualifications()
                .FirstOrDefault(x =>
                    x.TypeOfQualification == typeOfQualification &&
                    x.TypeOfSchooling == typeOfSchooling &&
                    x.Code == code &&
                    x.StudyPathCode == studyPathCode &&
                    x.SpecializationCode == specializationCode &&
                    x.SubCode == subCode);
            if (qualification != null) return qualification;

            qualification = new Qualification
            {
                TypeOfQualification = typeOfQualification,
                TypeOfSchooling = typeOfSchooling,
                Code = code,
                StudyPathCode = studyPathCode,
                SpecializationCode = specializationCode,
                SubCode = subCode,
                Name = name,
                SpecializationName = specializationName,
                StudyPathName = studyPathName
            };
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO WLI_Qualification (TypeOfQualification, TypeOfSchooling, Code, StudyPathCode, SpecializationCode, SubCode, Name, SpecializationName, StudyPathName) ";
                query += "VALUES (@TypeOfQualification, @TypeOfSchooling, @Code, @StudyPathCode, @SpecializationCode, @SubCode, @Name, @SpecializationName, @StudyPathName); ";
                query += "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                qualification.Id = conn.QueryFirst<int>(query, qualification);
                _allQualifications.Add(qualification);
            }

            return qualification;
        }
    }
}