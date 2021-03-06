﻿using Caliburn.Micro;
using DataStoring.Contracts.MovieModels;

namespace PanasonicSync.GUI.ViewModels
{
    public class MovieViewModel : ViewModelBase, IScreen
    {
        private IMovieFile _movie;
        private bool _readOnly;

        public IMovieFile Movie
        {
            get => _movie;
            set
            {
                _movie = value;
                NotifyOfPropertyChange();
            }
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                NotifyOfPropertyChange();
            }
        }

        public MovieViewModel()
        {

        }

        public MovieViewModel(IMovieFile movie)
        {
            Movie = movie;
        }

        public MovieViewModel(IMovieFile movie, bool readOnly) : this(movie)
        {
            ReadOnly = readOnly;
        }
    }
}
