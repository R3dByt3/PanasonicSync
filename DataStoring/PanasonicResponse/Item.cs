using System.Xml.Serialization;

namespace DataStoring.PanasonicResponse
{
    [XmlRoot(ElementName = "item", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
    public class Item
    {
        [XmlElement(ElementName = "bbr_content_id", Namespace = "urn:schemas-panasonic-com:pxn")]
        public string Bbr_content_id { get; set; }
        [XmlElement(ElementName = "channelID", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public ChannelID ChannelID { get; set; }
        [XmlElement(ElementName = "channelName", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string ChannelName { get; set; }
        [XmlElement(ElementName = "channelNr", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string ChannelNr { get; set; }
        [XmlElement(ElementName = "class", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public Class Class { get; set; }
        [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Date { get; set; }
        [XmlElement(ElementName = "genre", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string Genre { get; set; }
        [XmlElement(ElementName = "groupID", Namespace = "urn:schemas-panasonic-com:pxn")]
        public string GroupID { get; set; }
        [XmlElement(ElementName = "groupMemberNum", Namespace = "urn:schemas-panasonic-com:pxn")]
        public string GroupMemberNum { get; set; }
        [XmlElement(ElementName = "groupPlaybackCount", Namespace = "urn:schemas-panasonic-com:pxn")]
        public string GroupPlaybackCount { get; set; }
        [XmlElement(ElementName = "groupTopFlag", Namespace = "urn:schemas-panasonic-com:pxn")]
        public string GroupTopFlag { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "objectType", Namespace = "urn:schemas-dvb-org:metadata-1-0/")]
        public string ObjectType { get; set; }
        [XmlAttribute(AttributeName = "parentID")]
        public string ParentID { get; set; }
        [XmlElement(ElementName = "playbackCount", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string PlaybackCount { get; set; }
        [XmlElement(ElementName = "recordable", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string Recordable { get; set; }
        [XmlElement(ElementName = "res", Namespace = "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/")]
        public RemoteMovie RemoteMovie { get; set; }
        [XmlAttribute(AttributeName = "restricted")]
        public string Restricted { get; set; }
        [XmlElement(ElementName = "storageMedium", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string StorageMedium { get; set; }
        [XmlElement(ElementName = "title", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Title { get; set; }
        [XmlElement(ElementName = "writeStatus", Namespace = "urn:schemas-upnp-org:metadata-1-0/upnp/")]
        public string WriteStatus { get; set; }
    }

}