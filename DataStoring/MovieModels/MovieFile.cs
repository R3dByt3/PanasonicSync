using DataStoring.Contracts.MovieModels;
using System;

namespace DataStoring.MovieModels
{
    public class MovieFile : IMovieFile
    {
        public string Title { get; set; }
        public ulong Size { get; set; }
        public TimeSpan Duration { get; set; }
        public string FilePath { get; set; }
        public Uri FileLink { get; set; }
        public bool IsSelected { get; set; }

        public string SizeHr => $"{Size/1024/1024/1024} GB";

        public MovieFile()
        {
            IsSelected = true;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
