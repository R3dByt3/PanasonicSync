using DataStoring.Contracts.MovieModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanasonicSync.GUI.Comparer
{
    public class MovieEqualityComparer : IEqualityComparer<IMovieFile>
    {
        public bool Equals(IMovieFile x, IMovieFile y)
        {
            return x.Title == y.Title;
        }

        public int GetHashCode(IMovieFile obj)
        {
            return EqualityComparer<string>.Default.GetHashCode(obj.Title);
        }
    }
}
