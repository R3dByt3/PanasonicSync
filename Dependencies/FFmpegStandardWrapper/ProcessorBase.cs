using FFmpegStandardWrapper.Exceptions;
using System.Diagnostics;

namespace FFmpegStandardWrapper
{
    public abstract class ProcessorBase
    {
        internal string RunFFmpeg(string args)
        {
            if (string.IsNullOrWhiteSpace(Controller.FFmpegPath))
                throw new FFmpegException("Engine is not initialized yet");

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = Controller.FFmpegPath,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
                throw new FFmpegException(error);

            process.Dispose();

            return output;
        }

        internal string RunFFprobe(string args)
        {
            if (string.IsNullOrWhiteSpace(Controller.FFProbePath))
                throw new FFprobeException("Engine is not initialized yet");

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = Controller.FFProbePath,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
                throw new FFprobeException(error);

            process.Dispose();

            return output;
        }
    }
}
