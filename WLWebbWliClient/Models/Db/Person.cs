using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models.Db
{
    public class Person
    {
        public string SocialSecurityNumber { get; set; }
        public int CustomerId { get; set; }
        public DateTime LastUploaded { get; set; }
    }
}
