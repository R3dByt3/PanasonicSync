using System.Xml.Serialization;

namespace DataStoring.PanasonicControlsResponse
{
	[XmlRoot(ElementName = "X_IPPLTV_LIST", Namespace = "urn:schemas-panasonic-com:vli")]
	public class X_IPPLTV_LIST
	{
		[XmlElement(ElementName = "X_IPPLTV_VERSION", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_IPPLTV_VERSION { get; set; }
		[XmlElement(ElementName = "X_IPPLTV_RECMODE", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_IPPLTV_RECMODE { get; set; }
		[XmlElement(ElementName = "X_IPPLTV_SCPDURL", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_IPPLTV_SCPDURL { get; set; }
		[XmlElement(ElementName = "X_IPPLTV_controlURL", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_IPPLTV_controlURL { get; set; }
		[XmlElement(ElementName = "X_IPPLTV_eventSubURL", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_IPPLTV_eventSubURL { get; set; }
		[XmlElement(ElementName = "X_IPPLTV_CAP", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_IPPLTV_CAP { get; set; }
	}

}
