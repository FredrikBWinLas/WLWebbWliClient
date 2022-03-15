using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace WLWebbWliClient.Models
{
    public class PersonService
    {
        private readonly string _connectionString;
        private IDictionary<string, int> _personIdLookup;

        public PersonService(string connectionString) => _connectionString = connectionString;

        public bool PersonExists(string socialSecurityNumber) => GetPersonIdDictionary().ContainsKey(socialSecurityNumber);
        public int GetPersonId(string socialSecurityNumber) => GetPersonIdDictionary()[socialSecurityNumber];
        private IDictionary<string, int> GetPersonIdDictionary()
        {
            if (_personIdLookup == null)
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        var query = @"SELECT PERSONID, PERSONNR FROM WLPERSDATA";
                        _personIdLookup = conn.Query(query).ToDictionary(
                            row => (string) row.PERSONNR.Substring(0, 8) + "-" + (string) row.PERSONNR.Substring(8),
                            row => (int) row.PERSONID);
                    }
                }
                catch
                {
                    _personIdLookup = new Dictionary<string, int>();
                }
            }

            return _personIdLookup;
        }
    }
}
