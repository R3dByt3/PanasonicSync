using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoring.Contracts.MovieModels
{
    public interface IConflict
    {
        IMovieFile MovieFile { get; set; }
        IList<IMovieFile> Conflicts { get; set; }
    }
}
