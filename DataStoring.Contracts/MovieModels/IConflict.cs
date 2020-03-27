using System.Collections.Generic;

namespace DataStoring.Contracts.MovieModels
{
    public interface IConflict
    {
        IMovieFile MovieFile { get; set; }
        IList<IMovieFile> Conflicts { get; set; }
    }
}
