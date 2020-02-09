using System.Xml.Serialization;

namespace DataStoring.PanasonicControlsResponse
{
    [XmlRoot(ElementName = "X_MAC_ADDRESS_LIST", Namespace = "urn:schemas-panasonic-com:vli")]
    public class X_MAC_ADDRESS_LIST
    {
        [XmlElement(ElementName = "X_MAC_ADDRESS", Namespace = "urn:schemas-panasonic-com:vli")]
        public string X_MAC_ADDRESS { get; set; }
    }

}
