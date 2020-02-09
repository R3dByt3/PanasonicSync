using DataStoring.Contracts.PanasonicRequest;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataStoring.PanasonicRequest
{
    [XmlRoot(ElementName = "BrowseFlag")]
    public class BrowseFlag : IBrowseFlag
    {
        [XmlElement(ElementName = "dt", Namespace = "http://www.w3.org/2000/xmlns/")]
        public List<string> Dt { get; set; }
        [XmlText]
        public string Text { get; set; }

        public BrowseFlag()
        {
            Dt = new List<string> { "string" };
            Text = "BrowseDirectChildren";
        }
    }
}
