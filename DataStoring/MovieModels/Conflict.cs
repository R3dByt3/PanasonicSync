using DataStoring.Contracts.MovieModels;
using System.Collections.Generic;

namespace DataStoring.MovieModels
{
    public class Conflict : IConflict
    {
        public IMovieFile MovieFile { get; set; }
        public IList<IMovieFile> Conflicts { get; set; }
    }
}
