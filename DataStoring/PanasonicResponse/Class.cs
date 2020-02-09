using DataStoring.Contracts.PanasonicResponse;
using System.Xml.Serialization;

namespace DataStoring.PanasonicResponse
{
    [XmlRoot(ElementName = "class", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
    public class Class : IClass
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

}