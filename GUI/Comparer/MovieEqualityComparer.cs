using DataStoring.Contracts.MovieModels;
using System.Collections.Generic;

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
