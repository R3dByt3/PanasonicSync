using DataStoring.Contracts.PanasonicRequest;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataStoring.PanasonicRequest
{
    [XmlRoot(ElementName = "SortCriteria")]
    public class SortCriteria : ISortCriteria
    {
        [XmlElement(ElementName = "dt", Namespace = "http://www.w3.org/2000/xmlns/")]
        public List<string> Dt { get; set; }

        public SortCriteria()
        {
            Dt = new List<string> { "string" };
        }
    }
}
