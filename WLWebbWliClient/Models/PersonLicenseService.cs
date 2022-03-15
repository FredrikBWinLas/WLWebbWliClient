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
    public class PersonLicenseService
    {
        private readonly string _connectionString;
        private IDictionary<int, List<PersonLicense>> _allPersonLicenses = null;

        public PersonLicenseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDictionary<int, List<PersonLicense>> GetAllPersonLicenses()
        {
            if (_allPersonLicenses == null)
            {
                try
                {
                    var result = new Dictionary<int, List<PersonLicense>>();
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        var query = @"SELECT * FROM WLI_PersonLicense";
                        foreach (var row in conn.Query(query))
                        {
                            if (!result.ContainsKey(row.PersonId))
                            {
                                result.Add(row.PersonId, new List<PersonLicense>());
                            }

                            var createdTimeString = row.CreatedTime as string;
                            var createdTimeDate = string.IsNullOrEmpty(createdTimeString)
                                ? DateTime.MinValue
                                : DateTime.Parse(createdTimeString);
                            var lastCheckedTimeString = row.LastCheckedTime as string;
                            var lastCheckedTimeStringDate = string.IsNullOrEmpty(lastCheckedTimeString)
                                ? DateTime.MinValue
                                : DateTime.Parse(lastCheckedTimeString);
                            var dateLicenseIssuedString = row.LastCheckedTime as string;
                            DateTime? dateLicenseIssuedStringDate = string.IsNullOrEmpty(dateLicenseIssuedString)
                                ? null
                                : DateTime.Parse(dateLicenseIssuedString);
                            result[row.PersonId].Add(new PersonLicense
                            {
                                CreatedTime = createdTimeDate,
                                LastCheckedTime = lastCheckedTimeStringDate,
                                DateLicenseIssued = dateLicenseIssuedStringDate,
                                PersonId = row.PersonId,
                                LicenseId = row.LicenseId
                            });
                        }
                    }

                    _allPersonLicenses = result;
                }
                catch (Exception e)
                {
                    _allPersonLicenses = new Dictionary<int, List<PersonLicense>>();
                }
            }

            return _allPersonLicenses;
        }
        public void RemovePersonLicenses(IEnumerable<PersonLicense> personLicenses)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var personLicense in personLicenses)
                {
                    var sql = $"DELETE WLI_PersonLicense WHERE LicenseId=@LicenseId AND PersonId=@PersonId";
                    connection.Execute(sql, personLicense);
                }
            }
        }
        public void UpdatePersonLicenses(IEnumerable<PersonLicense> personLicenses)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var personLicense in personLicenses)
                {
                    var sql = $"UPDATE WLI_PersonLicense SET CreatedTime=@CreatedTimeAsString, LastCheckedTime=@LastCheckedTimeAsString, DateLicenseIssued=@DateLicenseIssuedAsString, DeletedTime=@DeletedTimeAsString, REGTID='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE LicenseId=@LicenseId AND PersonId=@PersonId";
                    connection.Execute(sql, personLicense);
                }
            }
        }
        public void InsertPersonLicenses(IEnumerable<PersonLicense> personLicenses)
        {
            var sqls = GetSqlsInBatches(personLicenses);
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var sql in sqls)
                {
                    connection.Execute(sql);
                }
            }
        }

        private IList<string> GetSqlsInBatches(IEnumerable<PersonLicense> personLicenses)
        {
            var insertSql = "INSERT INTO [WLI_PersonLicense] (LicenseId, PersonId, CreatedTime, LastCheckedTime, DateLicenseIssued, DeletedTime) VALUES ";
            var valuesSql = "({0},{1},'{2}','{3}','{4}','{5}')";
            var batchSize = 1000;

            var sqlsToExecute = new List<string>();
            var numberOfBatches = (int)Math.Ceiling((double)personLicenses.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var itemsToInsert = personLicenses.Skip(i * batchSize).Take(batchSize);
                var valuesToInsert = itemsToInsert.Select(u => string.Format(valuesSql, u.LicenseId, u.PersonId, u.CreatedTimeAsString, u.LastCheckedTimeAsString, u.DateLicenseIssuedAsString, u.DeletedTimeAsString));
                sqlsToExecute.Add(insertSql + string.Join(',', valuesToInsert));
            }

            return sqlsToExecute;
        }
        public IEnumerable<PersonLicense> GetPersonLicenses(int personId)
        {
            if (GetAllPersonLicenses().ContainsKey(personId))
            {
                return GetAllPersonLicenses()[personId];
            }

            return new List<PersonLicense>();
        }
    }
}
