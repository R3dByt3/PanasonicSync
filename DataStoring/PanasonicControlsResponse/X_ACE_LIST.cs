using System.Xml.Serialization;

namespace DataStoring.PanasonicControlsResponse
{
    [XmlRoot(ElementName = "X_ACE_LIST", Namespace = "urn:schemas-panasonic-com:vli")]
	public class X_ACE_LIST
	{
		[XmlElement(ElementName = "X_VERSION", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_VERSION { get; set; }
		[XmlElement(ElementName = "X_MAIN_MENU", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MAIN_MENU { get; set; }
		[XmlElement(ElementName = "X_PARAM_FILE", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_PARAM_FILE { get; set; }
	}

}
