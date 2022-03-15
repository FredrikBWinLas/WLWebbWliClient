using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using WLWebbWliClient.Models.Db;

namespace WLWebbWliClient.Models
{
    public class PersonQualificationService
    {
        private readonly string _connectionString;
        private IDictionary<int, List<PersonQualification>> _allPersonQualifications = null;

        public PersonQualificationService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDictionary<int, List<PersonQualification>> GetAllPersonQualifications()
        {
            if (_allPersonQualifications == null)
            {
                try
                {
                    var result = new Dictionary<int, List<PersonQualification>>();
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        var query = @"SELECT * FROM WLI_PersonQualification";
                        foreach (var row in conn.Query(query))
                        {
                            if (!result.ContainsKey(row.PersonId))
                            {
                                result.Add(row.PersonId, new List<PersonQualification>());
                            }
                            var createdTimeString = row.CreatedTime as string;
                            var createdTimeDate = string.IsNullOrEmpty(createdTimeString)
                                ? DateTime.MinValue
                                : DateTime.Parse(createdTimeString);
                            var lastChangedTimeString = row.LastChangedTime as string;
                            DateTime? lastChangedTimeStringDate = string.IsNullOrEmpty(lastChangedTimeString)
                                ? null
                                : DateTime.Parse(lastChangedTimeString); 
                            var deletedTimeString = row.DeletedTime as string;
                            DateTime? deletedTimeStringDate = string.IsNullOrEmpty(deletedTimeString)
                                ? null
                                : DateTime.Parse(deletedTimeString);
                            result[row.PersonId].Add(new PersonQualification
                            {
                                QualificationId = row.QualificationId,
                                PersonId = row.PersonId,
                                FromYear = row.FromYear,
                                ToYear = row.ToYear,
                                CreatedTime = createdTimeDate,
                                LastChangedTime = lastChangedTimeStringDate,
                                DeletedTime = deletedTimeStringDate
                            });
                        }
                    }

                    _allPersonQualifications = result;
                }
                catch (Exception e)
                {
                    _allPersonQualifications = new Dictionary<int, List<PersonQualification>>();
                }
            }

            return _allPersonQualifications;
        }
        public void RemovePersonQualifications(IEnumerable<PersonQualification> personQualifications)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var personQualification in personQualifications)
                {
                    var sql = $"DELETE WLI_PersonQualification WHERE QualificationId=@QualificationId AND PersonId=@PersonId";
                    connection.Execute(sql, personQualification);
                }
            }
        }
        public void UpdatePersonQualifications(IEnumerable<PersonQualification> personQualifications)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var personQualification in personQualifications)
                {
                    var sql = $"UPDATE WLI_PersonQualification SET FromYear=@FromYear, ToYear=@ToYear, CreatedTime=@CreatedTimeAsString, LastChangedTime=@LastChangedTimeAsString, DeletedTime=@DeletedTimeAsString, REGTID='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE QualificationId=@QualificationId AND PersonId=@PersonId";
                    connection.Execute(sql, personQualification);
                }
            }
        }
        public void InsertPersonQualifications(IEnumerable<PersonQualification> personQualifications)
        {
            var sqls = GetSqlsInBatches(personQualifications);
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var sql in sqls)
                {
                    try
                    {
                        connection.Execute(sql);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        private IList<string> GetSqlsInBatches(IEnumerable<PersonQualification> personQualifications)
        {
            var insertSql = "INSERT INTO [WLI_PersonQualification] (QualificationId, PersonId, FromYear, ToYear, CreatedTime, LastCheckedTime, DeletedTime) VALUES ";
            var valuesSql = "({0},{1},'{2}','{3}','{4}','{5}','{6}')";
            var batchSize = 1000;

            var sqlsToExecute = new List<string>();
            var numberOfBatches = (int)Math.Ceiling((double)personQualifications.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var itemsToInsert = personQualifications.Skip(i * batchSize).Take(batchSize);
                var valuesToInsert = itemsToInsert.Select(u => string.Format(valuesSql, u.QualificationId, u.PersonId, u.FromYear, u.ToYear, u.CreatedTimeAsString, u.LastChangedTimeAsString, u.DeletedTimeAsString));
                sqlsToExecute.Add(insertSql + string.Join(',', valuesToInsert));
            }

            return sqlsToExecute;
        }
        public IEnumerable<PersonQualification> GetPersonQualification(int personId)
        {
            if (GetAllPersonQualifications().ContainsKey(personId))
            {
                return GetAllPersonQualifications()[personId];
            }

            return new List<PersonQualification>();
        }
    }
}