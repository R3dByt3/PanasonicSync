using System.Xml.Serialization;
using System.Collections.Generic;
using DataStoring.Contracts.PanasonicResponse;

namespace DataStoring.PanasonicResponse
{
    [XmlRoot(ElementName = "DIDL-Lite", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
    public class MovieListResponse : IMovieListResponse
    {
        [XmlAttribute(AttributeName = "dc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Dc { get; set; }
        [XmlAttribute(AttributeName = "dvb", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Dvb { get; set; }
        [XmlElement(ElementName = "item", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
        public List<IItem> Items { get; set; }
        [XmlAttribute(AttributeName = "pxn", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Pxn { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
        [XmlAttribute(AttributeName = "upnp", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Upnp { get; set; }
        [XmlAttribute(AttributeName = "vli", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Vli { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}