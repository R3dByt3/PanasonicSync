﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataStoring.PanasonicControlsResponse
{
    [XmlRoot(ElementName = "iconList", Namespace = "urn:schemas-upnp-org:device-1-0")]
	public class IconList
	{
		[XmlElement(ElementName = "icon", Namespace = "urn:schemas-upnp-org:device-1-0")]
		public List<Icon> Icon { get; set; }
	}

}
