using System;
using System.Linq;
using System.Xml.Serialization;

namespace DataStoring.PanasonicControlsResponse
{
	[XmlRoot(ElementName = "root", Namespace = "urn:schemas-upnp-org:device-1-0")]
	public class Root
	{
		[XmlElement(ElementName = "specVersion", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public SpecVersion SpecVersion { get; set; }
		[XmlElement(ElementName = "device", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public Device Device { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlAttribute(AttributeName = "dlna")]
		public string Dlna { get; set; }
		[XmlAttribute(AttributeName = "vli")]
		public string Vli { get; set; }
		[XmlAttribute(AttributeName = "viera")]
		public string Viera { get; set; }

		public string GetControlsURL()
		{
			return Device?
				.ServiceList?
				.Service?
				.FirstOrDefault(x => x.ServiceType == "urn:schemas-upnp-org:service:ContentDirectory:2" && 
					!string.IsNullOrWhiteSpace(x.ControlURL))?.ControlURL;
		}
	}

}
