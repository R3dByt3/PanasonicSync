using FFmpegStandardWrapper.Abstract.Core;
using FFmpegStandardWrapper.Abstract.Options;
using FFmpegStandardWrapper.EventArgs;
using FFmpegStandardWrapper.Exceptions;
using FFmpegStandardWrapper.Model.Probe;
using NetStandard.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace FFmpegStandardWrapper.Core
{
    public class FFmpeg : ProcessorBase, IFFmpeg
    {
        public event EventHandler<ConversionCompleteEventArgs> ConversionCompleteEvent;
        public event EventHandler<ConvertProgressEventArgs> ConvertProgressEvent;

        private readonly ILogger _logger;
        private readonly string _ffmpegPath;

        public FFmpeg(ILoggerFactory factory)
        {
            _logger = factory.CreateFileLogger();

            if (string.IsNullOrWhiteSpace(Controller.FFmpegPath))
                throw new FFmpegException("Engine has not been initialzied yet");

            _ffmpegPath = Controller.FFmpegPath;
        }

        public void Convert(string inputFile, string outputFile, IConversionOptions options)
        {
            if (string.IsNullOrWhiteSpace(Controller.FFmpegPath))
                throw new FFmpegException("Engine is not initialized yet");

            string additionalArgs = Path.DirectorySeparatorChar == '\\' ? "-nostdin -y -loglevel info " : "-y -loglevel info ";

            var startInfo = new ProcessStartInfo
            {
                FileName = Controller.FFmpegPath,
                Arguments = $"{additionalArgs}-i \"{inputFile}\" {options}\"{outputFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (var process = new Process())
            {
                List<string> receivedMessagesLog = new List<string>();
                TimeSpan totalMediaDuration = new TimeSpan();
                Exception caughtException = null;

                process.StartInfo = startInfo;
                process.ErrorDataReceived += (sender, received) =>
                {
                    if (received.Data == null) return;

                    try
                    {
                        receivedMessagesLog.Insert(0, received.Data);

                        Match matchDuration = RegexEngine.Index[RegexEngine.Find.Duration].Match(received.Data);
                        if (matchDuration.Success)
                        {
                            TimeSpan.TryParse(matchDuration.Groups[1].Value, out totalMediaDuration);
                        }

                        if (RegexEngine.IsProgressData(received.Data, out ConvertProgressEventArgs progressEvent))
                        {
                            progressEvent.TotalDuration = totalMediaDuration;
                            OnProgressChanged(progressEvent);
                        }
                        else if (RegexEngine.IsConvertCompleteData(received.Data, out ConversionCompleteEventArgs convertCompleteEvent))
                        {
                            convertCompleteEvent.TotalDuration = totalMediaDuration;
                            OnConversionComplete(convertCompleteEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        caughtException = ex;
                    }
                };
                process.Start();

                process.BeginErrorReadLine();
                process.WaitForExit();

                if (process.ExitCode != 0 || caughtException != null)
                {
                    throw new Exception(
                        process.ExitCode + ": " + receivedMessagesLog[0],
                        caughtException);
                }
            }
        }

        //public void ErrorDataReceived(object sender, DataReceivedEventArgs received)
        //{
        //}

        private void OnConversionComplete(ConversionCompleteEventArgs e)
        {
            ConversionCompleteEvent?.Invoke(this, e);
        }

        private void OnProgressChanged(ConvertProgressEventArgs e)
        {
            ConvertProgressEvent?.Invoke(this, e);
        }
    }
}
