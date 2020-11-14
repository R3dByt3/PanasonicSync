namespace FFmpegStandardWrapper.Abstract.Model
{
    public interface ISize
    {
        ulong Height { get; set; }
        ulong Width { get; set; }
        bool HasValue { get; }
    }
}