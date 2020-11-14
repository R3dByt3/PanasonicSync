namespace FFmpegStandardWrapper.Abstract.Options
{
    public interface IConversionOptions
    {
        IVideoOptions VideoOptions { get; set; }
        IAudioOptions AudioOptions { get; set; }
        ISubTitleOptions SubTitleOptions { get; set; }
    }
}
