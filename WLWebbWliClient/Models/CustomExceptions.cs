using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models
{
    [Serializable()]
    public class NoXmlException : System.Exception
    {
        public NoXmlException() : base() { }

        protected NoXmlException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
