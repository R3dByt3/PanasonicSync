using FFmpegStandardWrapper.Abstract.Model;

namespace FFmpegStandardWrapper.Abstract.Options
{
    public interface IAudioOptions
    {
        /// <summary>
        /// Set the number of audio frames to output [-aframes]
        /// </summary>
        ulong OutputFrames { get; set; }
        /// <summary>
        /// Set frame rate (Hz value) [-ar]
        /// </summary>
        double SampleRate { get; set; }
        /// <summary>
        /// Disable the audio [-an]
        /// </summary>
        bool DisableAudio { get; set; }
        /// <summary>
        /// Force audio codec ('copy' to copy stream) [-acodec]
        /// </summary>
        ICodec Codec { get; set; }
    }
}