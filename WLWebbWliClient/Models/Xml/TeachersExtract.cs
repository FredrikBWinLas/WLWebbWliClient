using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WLWebbWliClient.Models.Xml
{
    [XmlRoot(ElementName = "header", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class Header
    {
        [XmlElement(ElementName = "issuedDate", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public DateTime IssuedDate { get; set; }
        [XmlElement(ElementName = "extractId", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string ExtractId { get; set; }
    }

    [XmlRoot(ElementName = "license", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class License
    {
        [XmlElement(ElementName = "typeOflicense", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string TypeOflicense { get; set; }

        [XmlElement(ElementName = "dateLicenseIssued", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public DateTime DateLicenseIssued { get; set; }
    }

    [XmlRoot(ElementName = "qualification", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class Qualification
    {
        [XmlElement(ElementName = "typeOfQualification", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string TypeOfQualification { get; set; }
        [XmlElement(ElementName = "typeOfSchooling", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string TypeOfSchooling { get; set; }
        [XmlElement(ElementName = "code", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string Code { get; set; }
        [XmlElement(ElementName = "name", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string Name { get; set; }
        [XmlElement(ElementName = "fromYear", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string FromYear { get; set; }
        [XmlElement(ElementName = "toYear", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string ToYear { get; set; }
        [XmlElement(ElementName = "studyPathCode", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string StudyPathCode { get; set; }
        [XmlElement(ElementName = "studyPathName", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string StudyPathName { get; set; }
        [XmlElement(ElementName = "specializationCode", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string SpecializationCode { get; set; }
        [XmlElement(ElementName = "specializationName", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string SpecializationName { get; set; }
        [XmlElement(ElementName = "subCode", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string SubCode { get; set; }
    }

    [XmlRoot(ElementName = "qualifications", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class Qualifications
    {
        [XmlElement(ElementName = "qualification", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public List<Qualification> Qualification { get; set; }
    }

    [XmlRoot(ElementName = "teacher", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class Teacher
    {
        [XmlElement(ElementName = "socialSecurityNumber", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string SocialSecurityNumber { get; set; }
        [XmlElement(ElementName = "license", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public List<License> License { get; set; }
        [XmlElement(ElementName = "qualifications", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public Qualifications Qualifications { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlElement(ElementName = "foreignExamTitle", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string ForeignExamTitle { get; set; }
    }

    [XmlRoot(ElementName = "teachers", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class Teachers
    {
        [XmlElement(ElementName = "teacher", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public List<Teacher> Teacher { get; set; }
    }

    [XmlRoot(ElementName = "missingTeacher", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class MissingTeacher
    {
        [XmlElement(ElementName = "socialSecurityNumber", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public string SocialSecurityNumber { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

    [XmlRoot(ElementName = "missingTeachers", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class MissingTeachers
    {
        [XmlElement(ElementName = "missingTeacher", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public List<MissingTeacher> MissingTeacher { get; set; }
    }

    [XmlRoot(ElementName = "extractResult", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class ExtractResult
    {
        [XmlElement(ElementName = "teachers", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public Teachers Teachers { get; set; }
        [XmlElement(ElementName = "missingTeachers", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public MissingTeachers MissingTeachers { get; set; }
    }

    [XmlRoot(ElementName = "teacherLicenseExtract", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
    public class TeacherLicenseExtract
    {
        [XmlElement(ElementName = "header", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "extractResult", Namespace = "http://teacher_license_extract.skolverket.se/v1_0")]
        public ExtractResult ExtractResult { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
