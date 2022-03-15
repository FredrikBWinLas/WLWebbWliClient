using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models.Db
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public int MonthlyIntervals { get; set; }
        public bool Active { get; set; }
        public DateTime? LastUploadedPersonalList { get; set; }

        public IEnumerable<Person> Persons { get; set; }
    }
}
