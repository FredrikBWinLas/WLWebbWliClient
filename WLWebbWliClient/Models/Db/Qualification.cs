using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models.Db
{
    public class Qualification
    {
        public int Id { get; set; }
        public string TypeOfQualification { get; set; }
        public string TypeOfSchooling { get; set; }
        public string Code { get; set; }
        public string StudyPathCode { get; set; }
        public string SpecializationCode { get; set; }
        public string SubCode { get; set; }
        public string Name { get; set; }
        public string SpecializationName { get; set; }
        public string StudyPathName { get; set; }
    }
}
