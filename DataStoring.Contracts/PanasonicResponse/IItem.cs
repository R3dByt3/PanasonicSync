namespace DataStoring.Contracts.PanasonicResponse
{
    public interface IItem
    {
        string Bbr_content_id { get; set; }
        IChannelID ChannelID { get; set; }
        string ChannelName { get; set; }
        string ChannelNr { get; set; }
        IClass Class { get; set; }
        string Date { get; set; }
        string Genre { get; set; }
        string GroupID { get; set; }
        string GroupMemberNum { get; set; }
        string GroupPlaybackCount { get; set; }
        string GroupTopFlag { get; set; }
        string Id { get; set; }
        string ObjectType { get; set; }
        string ParentID { get; set; }
        string PlaybackCount { get; set; }
        string Recordable { get; set; }
        IRemoteMovie RemoteMovie { get; set; }
        string Restricted { get; set; }
        string StorageMedium { get; set; }
        string Title { get; set; }
        string WriteStatus { get; set; }
    }
}