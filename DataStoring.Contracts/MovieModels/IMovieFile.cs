using System;

namespace DataStoring.Contracts.MovieModels
{
    public interface IMovieFile
    {
        TimeSpan Duration { get; set; }
        Uri FileLink { get; set; }
        string FilePath { get; set; }
        ulong Size { get; set; }
        string Title { get; set; }
    }
}