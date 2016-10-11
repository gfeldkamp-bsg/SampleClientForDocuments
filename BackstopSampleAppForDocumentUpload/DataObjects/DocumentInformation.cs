using System;
using System.Xml.Serialization;

namespace BackstopSampleAppForDocumentUpload.DataObjects
{
    [Serializable]
    [XmlRoot("DocumentInformation", Namespace = "http://returnentity.webservice.fundbutter.backstopsolutions.com")]    
    public class DocumentInformation
    {
        [XmlElement]
        public string Author { get; set; }
        [XmlElement]
        public DateTime CreatedDate { get; set; }
        [XmlElement]
        public string Description { get; set; }
        [XmlElement]
        public string FileName { get; set; }
        [XmlElement]
        public int Id { get; set; }
        [XmlElement]
        public DateTime OccuredDate { get; set; }
        [XmlElement]
        public string Title { get; set; }
        [XmlElement]
        public string Uri { get; set; }

        public DocumentInformation()
        {

        }
    }
}
