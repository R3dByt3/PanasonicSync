using DataStoring.Contracts.PanasonicResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataStoring.PanasonicResponse
{
	[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public class PanasonicResponseRoot : IPanasonicResponseRoot
	{
		[XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		public IResponseBody Body { get; set; }
		[XmlAttribute(AttributeName = "encodingStyle", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		public string EncodingStyle { get; set; }
		[XmlAttribute(AttributeName = "s", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string S { get; set; }
	}
}
