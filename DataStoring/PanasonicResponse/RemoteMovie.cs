using DataStoring.Contracts.PanasonicResponse;
using System;
using System.Xml.Serialization;

namespace DataStoring.PanasonicResponse
{

	[XmlRoot(ElementName = "res", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
	public class RemoteMovie : IRemoteMovie
	{
		[XmlAttribute(AttributeName = "ChapterList", Namespace = "urn:schemas-panasonic-com:pxn")]
		public string ChapterList { get; set; }
		[XmlAttribute(AttributeName = "duration")]
		public string Duration { get; set; }
		[XmlAttribute(AttributeName = "NaviList", Namespace = "urn:schemas-panasonic-com:pxn")]
		public string NaviList { get; set; }
		[XmlAttribute(AttributeName = "playitemNum", Namespace = "urn:schemas-panasonic-com:vli")]
		public string PlayitemNum { get; set; }
		[XmlAttribute(AttributeName = "protocolInfo")]
		public string ProtocolInfo { get; set; }
		[XmlAttribute(AttributeName = "ResumePoint", Namespace = "urn:schemas-panasonic-com:pxn")]
		public string ResumePoint { get; set; }
		[XmlAttribute(AttributeName = "size")]
		public string Size { get; set; }
		[XmlAttribute(AttributeName = "StreamPort", Namespace = "urn:schemas-panasonic-com:pxn")]
		public string StreamPort { get; set; }
		[XmlText]
		public string Text { get; set; }
		[XmlAttribute(AttributeName = "VgaContentProtocolInfo", Namespace = "urn:schemas-panasonic-com:pxn")]
		public string VgaContentProtocolInfo { get; set; }
		[XmlAttribute(AttributeName = "VgaContentUri", Namespace = "urn:schemas-panasonic-com:pxn")]
		public string VgaContentUri { get; set; }
		[XmlAttribute(AttributeName = "VgaContentVideoBitrate", Namespace = "urn:schemas-panasonic-com:pxn")]
		public string VgaContentVideoBitrate { get; set; }
	}

}