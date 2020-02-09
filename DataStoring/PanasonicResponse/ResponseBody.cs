using DataStoring.Contracts.PanasonicResponse;
using System.Xml.Serialization;

namespace DataStoring.PanasonicResponse
{
    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class ResponseBody : IResponseBody
    {
        [XmlElement(ElementName = "BrowseResponse", Namespace = "urn:schemas-upnp-org:service:ContentDirectory:2")]
        public IBrowseResponse BrowseResponse { get; set; }
    }
}
