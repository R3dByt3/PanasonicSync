namespace DataStoring.Contracts.PanasonicResponse
{
    public interface IRemoteMovie
    {
        string ChapterList { get; set; }
        string Duration { get; set; }
        string NaviList { get; set; }
        string PlayitemNum { get; set; }
        string ProtocolInfo { get; set; }
        string ResumePoint { get; set; }
        string Size { get; set; }
        string StreamPort { get; set; }
        string Text { get; set; }
        string VgaContentProtocolInfo { get; set; }
        string VgaContentUri { get; set; }
        string VgaContentVideoBitrate { get; set; }
    }
}