using FFmpegStandardWrapper.Abstract.Model;

namespace FFmpegStandardWrapper.Abstract.Options
{
    public interface ISubTitleOptions
    {
        /// <summary>
        /// Set frame size (WxH) [-s]
        /// </summary>
        ISize Size { get; set; }
        /// <summary>
        /// Disable subtitle [-sn]
        /// </summary>
        bool DisableSubTitles { get; set; }
        /// <summary>
        /// Force subtitle codec ('copy' to copy stream) [-scodec]
        /// </summary>
        ICodec Codec { get; set; }
        /// <summary>
        /// Set canvas size (WxH) [-canvas_size]
        /// </summary>
        ISize CanvasSize { get; set; }
    }
}