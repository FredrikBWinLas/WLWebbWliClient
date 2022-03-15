using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WLWebbWliClient.Models.Db;

namespace WLWebbWliClient.Models
{
    public class LicenseService
    {
        private readonly string _connectionString;
        private List<License> _allLicenses = null;
        public License MissingTeacherLicense => _allLicenses?.FirstOrDefault(x => x.Code == "MISSING_TEACHER");

        public LicenseService(string connectionString)
        {
            _connectionString = connectionString;
            _allLicenses = GetAllLicenses();
            if (MissingTeacherLicense == null)
            {
                CreateLicense("MISSING_TEACHER", "Saknar lärarlegitimation");
            }
        }


        public List<License> GetAllLicenses()
        {
            if (_allLicenses == null)
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        var query = @"SELECT * FROM WLI_License";
                        _allLicenses = (conn.Query<License>(query)).ToList();
                    }
                }
                catch
                {
                    _allLicenses = new List<License>();
                }
            }

            return _allLicenses;
        }

        public License CreateLicense(string code, string name)
        {
            var license = GetAllLicenses().FirstOrDefault(x => x.Code == code);
            if (license != null) return license;

            license = new License
            {
                Code = code,
                Name = name
            };
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO WLI_License (Code, Name) ";
                query += "VALUES (@Code, @Name); ";
                query += "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                license.Id = conn.QueryFirst<int>(query, license);
                _allLicenses.Add(license);
            }

            return license;
        }
    }
}
