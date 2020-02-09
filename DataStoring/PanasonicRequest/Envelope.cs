using DataStoring.Contracts.PanasonicRequest;
using System.Xml.Serialization;

namespace DataStoring.PanasonicRequest
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope : IEnvelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public RequestBody PBody
        {
            get => (RequestBody)Body;
            set => Body = value;
        }
        [XmlIgnore]
        public IRequestBody Body { get; set; }
        [XmlAttribute(AttributeName = "SOAP-ENV", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string SOAPENV { get; set; }
        [XmlAttribute(AttributeName = "encodingStyle", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public string EncodingStyle { get; set; }

        public Envelope(IRequestBody body)
        {
            Body = body;
        }

        private Envelope()
        {

        }

        public void SetStartingIndex(int startingIndex)
        {
            Body.Browse.StartingIndex.Text = startingIndex.ToString();
        }

        public void SetRequestedCount(int requestedCount)
        {
            Body.Browse.RequestedCount.Text = requestedCount.ToString();
        }
    }
}
