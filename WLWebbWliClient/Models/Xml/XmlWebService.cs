using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WLWebbWliClient.Models.Xml
{
    [XmlRoot(ElementName = "amne")]
    public class XmlAmne
    {
        [XmlElement(ElementName = "kod")]
        public string Kod { get; set; }
        [XmlElement(ElementName = "namn")]
        public string Namn { get; set; }
        [XmlElement(ElementName = "winlaskod")]
        public string Winlaskod { get; set; }
        [XmlAttribute(AttributeName = "sprak")]
        public string Sprak { get; set; }
    }

    [XmlRoot(ElementName = "amnen")]
    public class XmlAmnen
    {
        [XmlElement(ElementName = "amne")]
        public List<XmlAmne> Amne { get; set; }
    }

    [XmlRoot(ElementName = "skolform")]
    public class XmlSkolform
    {
        [XmlElement(ElementName = "kod")]
        public string Kod { get; set; }
        [XmlElement(ElementName = "namn")]
        public string Namn { get; set; }
        [XmlElement(ElementName = "winlaskod")]
        public string Winlaskod { get; set; }
        [XmlElement(ElementName = "svkod")]
        public string Svkod { get; set; }
        [XmlElement(ElementName = "amnen")]
        public XmlAmnen Amnen { get; set; }
    }

    [XmlRoot(ElementName = "skolformer")]
    public class XmlSkolformer
    {
        [XmlElement(ElementName = "skolform")]
        public List<XmlSkolform> Skolform { get; set; }
    }

    [XmlRoot(ElementName = "typ")]
    public class Typ
    {
        [XmlElement(ElementName = "kod")]
        public string Kod { get; set; }
        [XmlElement(ElementName = "namn")]
        public string Namn { get; set; }
        [XmlElement(ElementName = "winlaskod")]
        public string Winlaskod { get; set; }
    }

    [XmlRoot(ElementName = "missingteacher")]
    public class Missingteacher
    {
        [XmlElement(ElementName = "typ")]
        public List<Typ> Typ { get; set; }
    }

    [XmlRoot(ElementName = "stod")]
    public class Stod
    {
        [XmlElement(ElementName = "skolformskod")]
        public string Skolformskod { get; set; }
        [XmlElement(ElementName = "typ")]
        public string Typ { get; set; }
        [XmlElement(ElementName = "namn")]
        public string Namn { get; set; }
        [XmlElement(ElementName = "winlaskod")]
        public string Winlaskod { get; set; }
    }

    [XmlRoot(ElementName = "stodtyper")]
    public class Stodtyper
    {
        [XmlElement(ElementName = "stod")]
        public List<Stod> Stod { get; set; }
    }
    [XmlRoot(ElementName = "skoltyp")]
    public class XmlSkoltyp
    {
        [XmlElement(ElementName = "kod")]
        public string Kod { get; set; }
        [XmlElement(ElementName = "namn")]
        public string Namn { get; set; }
        [XmlElement(ElementName = "winlaskod")]
        public string Winlaskod { get; set; }
    }

    [XmlRoot(ElementName = "skoltyper")]
    public class XmlSkoltyper
    {
        [XmlElement(ElementName = "skoltyp")]
        public List<XmlSkoltyp> Skoltyp { get; set; }
    }
}
