using APIClient.Contracts.Panasonic;
using DataStoring.Contracts.MovieModels;
using DiMappings;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UpnpClient.Contracts;

namespace PanasonicSync
{
    class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            IKernel kernel = new StandardKernel(new Aggregator().Mappings);

            var client = kernel.Get<IClient>();
            var panaclient = kernel.Get<IPanasonicClient>();

            var device = client.SearchUpnpDevices().First();

            panaclient.LoadControlsUri(device);
            var list = panaclient.RequestMovies().ToList();
            var remoteFileList = new List<IMovieFile>();
            var localFileList = new List<IMovieFile>();

            foreach (var file in list)
            {
                var remoteFile = kernel.Get<IMovieFile>();
                remoteFile.Title = RemoveInvalidChars(file.Title);
                remoteFile.Duration = TimeSpan.Parse(file.RemoteMovie.Duration);
                remoteFile.FileLink = new Uri(file.RemoteMovie.Text);
                remoteFile.Size = ulong.Parse(file.RemoteMovie.Size);
                remoteFileList.Add(remoteFile);
            }

            foreach(var file in Directory.GetFiles(@"N:\Movies\ALLE_RECS"))
            {
                var fileInfo = new FileInfo(file);

                var localFile = kernel.Get<IMovieFile>();
                localFile.Title = Path.GetFileNameWithoutExtension(file);
                //localFile.Duration = ulong.Parse(fileInfo.); //ToDo
                localFile.FilePath = file;
                localFile.Size = (ulong)fileInfo.Length;
                localFileList.Add(localFile);
            }

            foreach (var remoteFile in remoteFileList)
            {
                foreach (var localFile in localFileList)
                {
                    if (CalculateSimilarity(localFile.Title, remoteFile.Title) > 0.85)
                        Console.WriteLine(remoteFile.Title);
                }
            }

            Console.ReadLine();
        }

        private static double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null))
                return 0.0;
            if ((source.Length == 0) || (target.Length == 0))
                return 0.0;
            if (source == target)
                return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return 1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length));
        }

        private static int ComputeLevenshteinDistance(string source, string target)
        {
            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        private static string RemoveInvalidChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).ToArray()));
        }
    }
}
