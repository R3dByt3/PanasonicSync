using DataStoring.Contracts.PanasonicRequest;
using System.Xml.Serialization;

namespace DataStoring.PanasonicRequest
{
    [XmlRoot(ElementName = "Browse", Namespace = "urn:schemas-upnp-org:service:ContentDirectory:2")]
    public class Browse : IBrowse
    {
        [XmlElement(ElementName = "ObjectID")]
        public ObjectID PObjectID
        {
            get => (ObjectID)ObjectID;
            set => ObjectID = value;
        }
        [XmlIgnore]
        public IObjectID ObjectID { get; set; }
        [XmlElement(ElementName = "BrowseFlag")]
        public BrowseFlag PBrowseFlag
        {
            get => (BrowseFlag)BrowseFlag;
            set => BrowseFlag = value;
        }
        [XmlIgnore]
        public IBrowseFlag BrowseFlag { get; set; }
        [XmlElement(ElementName = "Filter")]
        public Filter PFilter
        {
            get => (Filter)Filter;
            set => Filter = value;
        }
        [XmlIgnore]
        public IFilter Filter { get; set; }
        [XmlElement(ElementName = "StartingIndex")]
        public StartingIndex PStartingIndex
        {
            get => (StartingIndex)StartingIndex;
            set => StartingIndex = value;
        }
        [XmlIgnore]
        public IStartingIndex StartingIndex { get; set; }
        [XmlElement(ElementName = "RequestedCount")]
        public RequestedCount PRequestedCount
        {
            get => (RequestedCount)RequestedCount;
            set => RequestedCount = value;
        }
        [XmlIgnore]
        public IRequestedCount RequestedCount { get; set; }
        [XmlElement(ElementName = "SortCriteria")]
        public SortCriteria PSortCriteria
        {
            get => (SortCriteria)SortCriteria;
            set => SortCriteria = value;
        }
        [XmlIgnore]
        public ISortCriteria SortCriteria { get; set; }
        [XmlAttribute(AttributeName = "m", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string M { get; set; }

        public Browse(IObjectID objectID, IBrowseFlag browseFlag, IFilter filter, IStartingIndex startingIndex, IRequestedCount requestedCount, ISortCriteria sortCriteria)
        {
            ObjectID = objectID;
            BrowseFlag = browseFlag;
            Filter = filter;
            StartingIndex = startingIndex;
            RequestedCount = requestedCount;
            SortCriteria = sortCriteria;
        }

        public Browse()
        {

        }
    }
}
