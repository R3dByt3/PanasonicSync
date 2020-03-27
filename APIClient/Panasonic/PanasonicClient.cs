using APIClient.Contracts.Panasonic;
using DataStoring.Contracts.UpnpResponse;
using DataStoring.PanasonicControlsResponse;
using DataStoring.PanasonicResponse;
using NetStandard.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace APIClient.Panasonic
{
    public class PanasonicClient : IPanasonicClient
    {
        private const string Request = @"</SOAP-ENV:Envelope>\r\n
<?xml version=""1.0""?>\r\n<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
    <SOAP-ENV:Body>
        <m:Browse xmlns:m=""urn:schemas-upnp-org:service:ContentDirectory:2"">
            <ObjectID xmlns:dt=""urn:schemas-microsoft-com:datatypes"" dt:dt=""string"">AV_ALL</ObjectID>
            <BrowseFlag xmlns:dt=""urn:schemas-microsoft-com:datatypes"" dt:dt=""string"">BrowseDirectChildren</BrowseFlag>
            <Filter xmlns:dt=""urn:schemas-microsoft-com:datatypes"" dt:dt=""string"">*</Filter>
            <StartingIndex xmlns:dt=""urn:schemas-microsoft-com:datatypes"" dt:dt=""ui4"">{0}</StartingIndex>
            <RequestedCount xmlns:dt=""urn:schemas-microsoft-com:datatypes"" dt:dt=""ui4"">{1}</RequestedCount>
            <SortCriteria xmlns:dt=""urn:schemas-microsoft-com:datatypes"" dt:dt=""string""/>
        </m:Browse>
    </SOAP-ENV:Body>
</SOAP-ENV:Envelope>\r\n";

        private readonly ILogger _logger;

        private Uri _controlsUri;

        public PanasonicClient(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateFileLogger();
        }

        public void LoadControlsUri(IPanasonicDevice device)
        {
            var uri = new Uri(device.Location);
            var request = WebRequest.CreateHttp(uri);
            request.Method = WebRequestMethods.Http.Get;

            var response = request.GetResponse() as HttpWebResponse;

            using (Stream responseStream = response.GetResponseStream())
            using (var xmlReader = XmlReader.Create(responseStream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Root));
                var browseResponse = serializer.Deserialize(xmlReader) as Root;
                _controlsUri = new Uri(browseResponse.GetControlsURL());
            }
        }

        public IEnumerable<Item> RequestMovies()
        {
            if (_controlsUri == null)
                throw new FieldAccessException("Controls-Uri has not been loaded yet");

            int itemsToRequest = 15;
            int itemsRequested = 0;

            while (itemsToRequest > 0)
            {
                var response = SendMoviesRequest(itemsToRequest, itemsRequested, _controlsUri);

                foreach (var item in ReadMoviesResponse(response, ref itemsRequested, ref itemsToRequest))
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<Item> ReadMoviesResponse(HttpWebResponse response, ref int itemsRequested, ref int itemsToRequest)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                XDocument doc = XDocument.Load(responseStream);

                var result = (from results in doc.Descendants(XName.Get("BrowseResponse", "urn:schemas-upnp-org:service:ContentDirectory:2"))
                              select results.Element("Result")).FirstOrDefault().Value;

                itemsRequested += int.Parse(doc.Descendants(XName.Get("NumberReturned")).FirstOrDefault().Value);
                itemsToRequest = int.Parse(doc.Descendants(XName.Get("TotalMatches")).FirstOrDefault().Value) - itemsRequested;

                XmlSerializer serializer = new XmlSerializer(typeof(MovieListResponse));
                var casted = serializer.Deserialize(new StringReader(result)) as MovieListResponse;
                return casted.Items;
            }
        }

        private HttpWebResponse SendMoviesRequest(int itemsToRequest, int itemsRequested, Uri uri)
        {
            var request = WebRequest.CreateHttp(_controlsUri);
            request.Method = WebRequestMethods.Http.Post;
            request.Headers.Clear();
            request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            request.KeepAlive = false;
            request.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            request.ContentType = @"text/xml; charset=""utf - 8""";
            request.UserAgent = "PanasonicSync";
            request.Headers.Add("FriendlyName.DLNA.ORG", Environment.MachineName);
            request.Headers.Add("SOAPAction", @"""urn:schemas-upnp-org:service:ContentDirectory:2#Browse""");
            request.Host = $"{uri.Host}:{uri.Port}";

            string postData = string.Format(Request, itemsRequested, itemsToRequest);
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(postData);

            // Set the content length of the string being posted.
            request.ContentLength = bytes.Length;

            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
            }

            return request.GetResponse() as HttpWebResponse;
        }
    }
}
