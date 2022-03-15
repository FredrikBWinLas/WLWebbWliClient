using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models.Db
{
    public class PersonRequest
    {
        public string SocialSecurityNumber { get; set; }
        public DateTime LastRequested { get; set; }
    }
}
