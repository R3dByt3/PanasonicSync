using DataStoring.Contracts.PanasonicRequest;
using System.Xml.Serialization;

namespace DataStoring.PanasonicRequest
{
    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class RequestBody : IRequestBody
    {
        [XmlElement(ElementName = "Browse", Namespace = "urn:schemas-upnp-org:service:ContentDirectory:2")]
        public Browse PBrowse
        {
            get => (Browse) Browse;
            set => Browse = value;
        }
        [XmlIgnore]
        public IBrowse Browse { get; set; }

        public RequestBody(IBrowse browse)
        {
            Browse = browse;
        }

        private RequestBody()
        {

        }
    }
}
