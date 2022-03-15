using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models.Db
{
    public class PersonLicense
    {
        public int LicenseId { get; set; }
        public int PersonId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastCheckedTime { get; set; }
        public DateTime? DateLicenseIssued { get; set; }
        public DateTime? DeletedTime { get; set; }
        public string CreatedTimeAsString => CreatedTime.ToString("yyyy-MM-dd HH:mm:ss");
        public string LastCheckedTimeAsString => LastCheckedTime.ToString("yyyy-MM-dd HH:mm:ss");
        public string DateLicenseIssuedAsString => DateLicenseIssued?.ToString("yyyy-MM-dd");
        public string DeletedTimeAsString => DeletedTime?.ToString("yyyy-MM-dd HH:mm:ss");

        public License License { get; set; }
    }
}
