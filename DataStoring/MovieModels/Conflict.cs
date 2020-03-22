using DataStoring.Contracts.MovieModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoring.MovieModels
{
    public class Conflict : IConflict
    {
        public IMovieFile MovieFile { get; set; }
        public IList<IMovieFile> Conflicts { get; set; }
    }
}
