using System.Xml.Serialization;

namespace DataStoring.PanasonicControlsResponse
{
	[XmlRoot(ElementName = "device", Namespace = "urn:schemas-upnp-org:device-1-0")]
	public class Device
	{
		[XmlElement(ElementName = "deviceType", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string DeviceType { get; set; }
		[XmlElement(ElementName = "friendlyName", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string FriendlyName { get; set; }
		[XmlElement(ElementName = "manufacturer", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string Manufacturer { get; set; }
		[XmlElement(ElementName = "modelName", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string ModelName { get; set; }
		[XmlElement(ElementName = "modelNumber", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string ModelNumber { get; set; }
		[XmlElement(ElementName = "modelDescription", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string ModelDescription { get; set; }
		[XmlElement(ElementName = "serialNumber", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string SerialNumber { get; set; }
		[XmlElement(ElementName = "modelURL", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string ModelURL { get; set; }
		[XmlElement(ElementName = "manufacturerURL", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string ManufacturerURL { get; set; }
		[XmlElement(ElementName = "UDN", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public string UDN { get; set; }
		[XmlElement(ElementName = "X_DLNADOC", Namespace = "urn:schemas-dlna-org:device-1-0")]
		public string X_DLNADOC { get; set; }
		[XmlElement(ElementName = "iconList", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public IconList IconList { get; set; }
		[XmlElement(ElementName = "X_MAC_ADDRESS_LIST", Namespace = "urn:schemas-panasonic-com:vli")]
		public X_MAC_ADDRESS_LIST X_MAC_ADDRESS_LIST { get; set; }
		[XmlElement(ElementName = "X_ACE_LIST", Namespace = "urn:schemas-panasonic-com:vli")]
		public X_ACE_LIST X_ACE_LIST { get; set; }
		[XmlElement(ElementName = "X_DEV_CATEGORY", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_DEV_CATEGORY { get; set; }
		[XmlElement(ElementName = "X_IPPLTV_LIST", Namespace = "urn:schemas-panasonic-com:vli")]
		public X_IPPLTV_LIST X_IPPLTV_LIST { get; set; }
		[XmlElement(ElementName = "X_DLNACAP", Namespace = "urn:schemas-dlna-org:device-1-0")]
		public string X_DLNACAP { get; set; }
		[XmlElement(ElementName = "X_MOJ_VERSION", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_VERSION { get; set; }
		[XmlElement(ElementName = "X_MOJ_DEVICE_TYPE", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_DEVICE_TYPE { get; set; }
		[XmlElement(ElementName = "X_MHC_DEVICE_ID", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MHC_DEVICE_ID { get; set; }
		[XmlElement(ElementName = "X_MOJ_ENC_QUALITY_HOME", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_ENC_QUALITY_HOME { get; set; }
		[XmlElement(ElementName = "X_MOJ_ENC_QUALITY_INET", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_ENC_QUALITY_INET { get; set; }
		[XmlElement(ElementName = "X_MOJ_DMS_PORT", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_DMS_PORT { get; set; }
		[XmlElement(ElementName = "X_MOJ_DTCP_PORT_HOME", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_DTCP_PORT_HOME { get; set; }
		[XmlElement(ElementName = "X_MOJ_DTCP_PORT_INET", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_DTCP_PORT_INET { get; set; }
		[XmlElement(ElementName = "X_MOJ_JP_THUMBNAIL_PORT", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_JP_THUMBNAIL_PORT { get; set; }
		[XmlElement(ElementName = "X_MOJ_CHAPLIST_PORT", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_CHAPLIST_PORT { get; set; }
		[XmlElement(ElementName = "X_MOJ_RSREG_PORT", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_RSREG_PORT { get; set; }
		[XmlElement(ElementName = "X_MOJ_DEVICE_CAP", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_DEVICE_CAP { get; set; }
		[XmlElement(ElementName = "X_MOJ_CONTROL_PATH", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_CONTROL_PATH { get; set; }
		[XmlElement(ElementName = "X_MOJ_CONTROL_PORT", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_MOJ_CONTROL_PORT { get; set; }
		[XmlElement(ElementName = "X_DRC_PORT", Namespace = "urn:schemas-panasonic-com:vli")]
		public string X_DRC_PORT { get; set; }
		[XmlElement(ElementName = "X_DMSUDN", Namespace = "urn:schemas-panasonic-com:viera")]
		public string X_DMSUDN { get; set; }
		[XmlElement(ElementName = "X_DMRUDN", Namespace = "urn:schemas-panasonic-com:viera")]
		public string X_DMRUDN { get; set; }
		[XmlElement(ElementName = "serviceList", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public ServiceList ServiceList { get; set; }
	}

}
