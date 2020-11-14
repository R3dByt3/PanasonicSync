namespace FFmpegStandardWrapper.Abstract.Model
{
    public interface IMuxer
    {
        bool CanDemux { get; }
        bool CanMux { get; }
        string Identifier { get; }
        string Description { get; }
        //MuxerType MuxerType { get; }
        //HashSet<IFormat> Formats { get; }
        //ICodec DefaultVideoCodec { get; }
        //ICodec DefaultAudioCodec { get; }
        //ICodec DefaultSubTitleCodec { get; }

        bool Fill(string helpLine);
        //void FillDetails(string innerResult);
    }
}
