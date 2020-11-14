using FFmpegStandardWrapper.Crawling.Model;

namespace FFmpegStandardWrapper.Abstract.Crawling
{
    public interface IFFmpegCrawler
    {
        RemoteFFmpegVersion CurrentFFmpegVersion { get; }
        bool TryLoadLatestVersion();
    }
}
