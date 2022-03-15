using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WLWebbWliClient.Models.Xml;

namespace WLWebbWliClient.Models
{
    public class XmlFileExtractHandler
    {
        public static TeacherLicenseExtract Get(string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(TeacherLicenseExtract));
                TeacherLicenseExtract result;
                using (var stringReader = new StringReader(xml))
                {
                    result = serializer.Deserialize(stringReader) as TeacherLicenseExtract;
                }
                return result;
            }
            catch { }
            return null;
        }
    }
}
