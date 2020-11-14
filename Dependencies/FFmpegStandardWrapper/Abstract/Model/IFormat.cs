namespace FFmpegStandardWrapper.Abstract.Model
{
    public interface IFormat
    {
        bool CanDemux { get; set; }
        bool CanMux { get; set; }
        string Identifier { get; set; }
        string Description { get; }
        bool Fill(string helpLine);
    }
}
