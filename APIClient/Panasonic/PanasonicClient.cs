using APIClient.Contracts.Panasonic;
using DataStoring.Contracts.PanasonicRequest;
using DataStoring.Contracts.PanasonicResponse;
using DataStoring.Contracts.UpnpResponse;
using DataStoring.PanasonicControlsResponse;
using NetStandard.Logger;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace APIClient.Panasonic
{
    public class PanasonicClient : IPanasonicClient
    {
        private readonly IKernel _kernel;
        private readonly ILogger _logger;

        private Uri _controlsUri;

        public PanasonicClient(IKernel kernel, ILoggerFactory loggerFactory)
        {
            _kernel = kernel;
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

        public IEnumerable<IItem> RequestMovies()
        {
            if (_controlsUri == null)
                throw new FieldAccessException("Controls-Uri has not been loaded yet");

            var request = WebRequest.CreateHttp(_controlsUri);
            request.Method = WebRequestMethods.Http.Post;

            int itemsToRequest = 1000;
            int itemsRequested = 0;
            IEnvelope envelope = _kernel.Get<IEnvelope>();
            IBrowseResponse browseResponse = null;

            while (itemsToRequest > 0)
            {
                var response = SendMoviesRequest(envelope, _controlsUri, request, itemsToRequest, itemsRequested);

                foreach (var item in ReadMoviesResponse(browseResponse, response))
                {
                    yield return item;
                }

                itemsRequested += int.Parse(browseResponse.NumberReturned);
                itemsToRequest = int.Parse(browseResponse.TotalMatches) - itemsRequested;

                envelope.SetRequestedCount(itemsToRequest);
                envelope.SetStartingIndex(itemsRequested);
            }
        }

        private IEnumerable<IItem> ReadMoviesResponse(IBrowseResponse browseResponse, HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (var xmlReader = XmlReader.Create(responseStream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(IResponseBody));
                browseResponse = serializer.Deserialize(xmlReader) as IBrowseResponse;
            }

            browseResponse.Parse();

            foreach (var item in browseResponse.ParsedResult.Items)
            {
                yield return item;
            }
        }

        private HttpWebResponse SendMoviesRequest(IEnvelope envelope, Uri uri, HttpWebRequest request, int itemsToRequest, int itemsRequested)
        {
            envelope.SetRequestedCount(itemsToRequest);
            envelope.SetStartingIndex(itemsRequested);

            request.Headers.Clear();
            request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            request.KeepAlive = false;
            request.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            request.ContentType = @"text/xml; charset=""utf - 8""";
            request.UserAgent = "PanasonicSync";
            request.Headers.Add("FriendlyName.DLNA.ORG", Environment.MachineName);
            request.Headers.Add("SOAPAction", @"""urn:schemas-upnp-org:service:ContentDirectory:2#Browse""");
            request.Host = $"{uri.Host}:{uri.Port}";

            using (Stream requestStream = request.GetRequestStream())
            using (var xmlWriter = XmlWriter.Create(requestStream, new XmlWriterSettings() { Indent = true, NewLineHandling = NewLineHandling.Entitize, }))
            {
                XmlSerializer serializer = new XmlSerializer(envelope.GetType());
                serializer.Serialize(xmlWriter, envelope);
                xmlWriter.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }
    }
}
