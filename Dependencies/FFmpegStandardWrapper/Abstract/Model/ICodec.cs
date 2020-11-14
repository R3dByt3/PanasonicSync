namespace FFmpegStandardWrapper.Abstract.Model
{
    public interface ICodec
    {
        bool CanDecode { get; }
        bool CanEncode { get; }
        bool IsVideoCodec { get; }
        bool IsAudioCodec { get; }
        bool IsSubtitleCodec { get; }
        bool IsIntraFrameOnlyCodec { get; }
        bool HasLossyCompression { get; }
        bool HasLosslessCompression { get; }
        string Identifier { get; }
        string Description { get; }
        bool Fill(string helpLine);
    }
}