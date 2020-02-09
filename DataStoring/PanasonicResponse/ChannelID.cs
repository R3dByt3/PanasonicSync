using System.Xml.Serialization;

namespace DataStoring.PanasonicResponse
{
    [XmlRoot(ElementName = "channelID", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
    public class ChannelID
    {
        [XmlText]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

}