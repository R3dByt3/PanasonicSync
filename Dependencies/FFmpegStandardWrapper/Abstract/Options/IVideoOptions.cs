using FFmpegStandardWrapper.Abstract.Model;

namespace FFmpegStandardWrapper.Abstract.Options
{
    public interface IVideoOptions
    {
        /// <summary>
        /// Set the number of video frames to output [-vframes]
        /// </summary>
        ulong OutputFrames { get; set; }
        /// <summary>
        /// Set frame rate (Hz value or fraction) [-r]
        /// </summary>
        double FrameRate { get; set; }
        /// <summary>
        /// Set frame size (WxH) [-s]
        /// </summary>
        ISize Size { get; set; }
        /// <summary>
        /// Set aspect ratio (4:3, 16:9 or 1.3333, 1.7777); ("W:H") [-aspect]
        /// </summary>
        IAspect Aspect { get; set; }
        /// <summary>
        /// Disable the video [-vn]
        /// </summary>
        bool DisableVideo { get; set; }
        /// <summary>
        /// Force video codec ('copy' to copy stream) [-vcodec]
        /// </summary>
        ICodec Codec { get; set; }
        /// <summary>
        /// Video bitrate [-b:v]
        /// </summary>
        ulong VideoBitrate { get; set; }
        /// <summary>
        /// Audio bitrate [-b:a]
        /// </summary>
        ulong AudioBitrate { get; set; }
    }
}