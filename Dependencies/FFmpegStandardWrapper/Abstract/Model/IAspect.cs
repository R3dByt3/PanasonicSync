namespace FFmpegStandardWrapper.Abstract.Model
{
    public interface IAspect
    {
        ulong Height { get; set; }
        ulong Width { get; set; }
        bool HasValue { get; }
    }
}