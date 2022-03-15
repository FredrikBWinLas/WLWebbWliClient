using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models.Db
{
    public class PersonQualification
    {
        public int QualificationId { get; set; }
        public int PersonId { get; set; }
        public int FromYear { get; set; } = 0;
        public int ToYear { get; set; } = 0;
        public DateTime CreatedTime { get; set; }
        public DateTime? LastChangedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public string CreatedTimeAsString => CreatedTime.ToString("yyyy-MM-dd HH:mm:ss");
        public string LastChangedTimeAsString => LastChangedTime?.ToString("yyyy-MM-dd HH:mm:ss");
        public string DeletedTimeAsString => DeletedTime?.ToString("yyyy-MM-dd HH:mm:ss");

        public Qualification Qualification { get; set; }
    }
}
