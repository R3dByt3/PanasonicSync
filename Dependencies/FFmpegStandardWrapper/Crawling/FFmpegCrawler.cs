using FFmpegStandardWrapper.Abstract.Crawling;
using FFmpegStandardWrapper.Crawling.Model;
using NetStandard.IO.Compression;
using NetStandard.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace FFmpegStandardWrapper.Crawling
{
    public class FFmpegCrawler : IFFmpegCrawler
    {
        public RemoteFFmpegVersion CurrentFFmpegVersion { get; private set; }

        private const string ZipName = "ffmpeg.zip";
        private const string InfoBinName = "info.bin";

        private readonly ILogger _logger;
        private readonly ICompressor _compressor;
        private readonly string _ffmpegDir;
        private readonly string _infoBinPath;

        public FFmpegCrawler(ILoggerFactory factory, ICompressor compressor, string ffmpegDir)
        {
            _logger = factory.CreateFileLogger();
            _compressor = compressor;
            _ffmpegDir = ffmpegDir;
            _infoBinPath = Path.Combine(_ffmpegDir, InfoBinName);

            if (File.Exists(_infoBinPath))
            {
                CurrentFFmpegVersion = _compressor.DeCompress<RemoteFFmpegVersion>(File.ReadAllBytes(_infoBinPath));
                _logger.Info($"Local FFmpeg info found");
                _logger.Info($"Local FFmpeg version = [{CurrentFFmpegVersion.Version}]");
            }
        }

        public bool TryLoadLatestVersion()
        {
            try
            {
                List<RemoteFFmpegVersion> links = new List<RemoteFFmpegVersion>();

                string url = "https://ffmpeg.zeranoe.com/builds/win64/static/";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();
                    Regex regex = new Regex("^<a href=\".*\">(?<name>.*)</a>\\s+(?<date>\\S* \\S*)\\s+(?<size>.*)", RegexOptions.Multiline);
                    MatchCollection matches = regex.Matches(html);
                    if (matches.Count > 0)
                    {
                        ProcessMatches(links, matches);
                    }
                }

                var latestFromWeb = links.OrderByDescending(x => x.Date).FirstOrDefault();

                _logger.Info($"Remote FFmpeg info found");
                _logger.Info($"Remote FFmpeg version = [{latestFromWeb.Version}]");

                if (CurrentFFmpegVersion == null || latestFromWeb.Version > CurrentFFmpegVersion.Version)
                {
                    CurrentFFmpegVersion = latestFromWeb;
                    File.WriteAllBytes(_infoBinPath, _compressor.Compress(CurrentFFmpegVersion));

                    CleanUpOldFFmpegExecutables();

                    DownloadLatest();

                    _logger.Info($"Remote FFmpeg downloaded");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
                return false;
            }
        }

        private void CleanUpOldFFmpegExecutables()
        {
            string[] files = Directory.GetFiles(_ffmpegDir, "*.exe");

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        private void ProcessMatches(List<RemoteFFmpegVersion> links, MatchCollection matches)
        {
            Regex versionRegex = new Regex(@"ffmpeg-(?<major>\d)[\.|\:](?<minor>\d)[\.|\:](?<revision>\d)-win64-static\.zip");
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var name = match.Groups["name"].ToString();
                    var versionMatch = versionRegex.Match(name);

                    Version version;

                    if (versionMatch.Groups.Count == 3)
                        version = new Version(int.Parse(versionMatch.Groups["major"].ToString()), int.Parse(versionMatch.Groups["minor"].ToString()), 0, 0);
                    else if (versionMatch.Groups.Count == 4)
                        version = new Version(int.Parse(versionMatch.Groups["major"].ToString()), int.Parse(versionMatch.Groups["minor"].ToString()), int.Parse(versionMatch.Groups["revision"].ToString()), 0);
                    else
                        continue;

                    links.Add(new RemoteFFmpegVersion
                    {
                        WebLink = "https://ffmpeg.zeranoe.com/builds/win64/static/" + name,
                        Date = DateTime.Parse(match.Groups["date"].ToString()),
                        Version = version,
                        Name = Path.GetFileNameWithoutExtension(name)
                    });
                }
            }
        }

        private void DownloadLatest()
        {
            var zipPath = Path.Combine(_ffmpegDir, ZipName);
            using (var client = new WebClient())
            {
                client.DownloadFile(CurrentFFmpegVersion.WebLink, zipPath);
            }

            _compressor.ExtractZipFile(zipPath, _ffmpegDir);

            var innerPath = Path.Combine(_ffmpegDir, CurrentFFmpegVersion.Name);
            var innerBinPath = Path.Combine(innerPath, "bin");

            string[] files = Directory.GetFiles(innerBinPath, "*.exe");

            foreach (string file in files)
            {
                var targetPath = Path.Combine(_ffmpegDir, Path.GetFileName(file));
                File.Move(file, targetPath);
            }

            Directory.Delete(innerPath, true);
            File.Delete(zipPath);
        }
    }
}
