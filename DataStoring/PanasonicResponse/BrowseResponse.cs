using DataStoring.Contracts.PanasonicResponse;
using System.IO;
using System.Xml.Serialization;

namespace DataStoring.PanasonicResponse
{
    [XmlRoot(ElementName = "BrowseResponse", Namespace = "urn:schemas-upnp-org:service:ContentDirectory:2")]
    public class BrowseResponse : IBrowseResponse
    {
        [XmlElement(ElementName = "NumberReturned")]
        public string NumberReturned { get; set; }
        [XmlElement(ElementName = "Result")]
        public string Result { get; set; }
        [XmlElement(ElementName = "TotalMatches")]
        public IMovieListResponse ParsedResult { get; private set; }
        public string TotalMatches { get; set; }
        [XmlAttribute(AttributeName = "u", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string U { get; set; }
        [XmlElement(ElementName = "UpdateID")]
        public string UpdateID { get; set; }

        public void Parse()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MovieListResponse));
            ParsedResult = (IMovieListResponse)serializer.Deserialize(GenerateStreamFromString(Result));
        }

        private Stream GenerateStreamFromString(string input)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
