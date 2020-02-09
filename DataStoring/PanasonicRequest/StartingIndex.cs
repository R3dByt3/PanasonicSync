using DataStoring.Contracts.PanasonicRequest;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataStoring.PanasonicRequest
{
	[XmlRoot(ElementName = "StartingIndex")]
	public class StartingIndex : IStartingIndex
	{
		[XmlElement(ElementName = "dt", Namespace = "http://www.w3.org/2000/xmlns/")]
		public List<string> Dt { get; set; }
		[XmlText]
		public string Text { get; set; }

		public StartingIndex()
		{
			Dt = new List<string> { "ui4" };
			Text = "0";
		}
	}
}
