using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PanasonicSync.GUI.ViewModels
{
    public class ProgressbarViewModel : PropertyChangedBase
    {
        public Stack<Tuple<string, bool>> Steps;

        private string _currentStep;
        private bool _isIndeterminate;
        private double _maximum;
        private double _value;

        public string CurrentStep
        {
            get => _currentStep;
            set
            {
                _currentStep = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set
            {
                _isIndeterminate = value;
                NotifyOfPropertyChange();
            }
        }

        public double Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                NotifyOfPropertyChange();
            }
        }

        public double Value 
        { 
            get => _value;
            set
            {
                _value = value;
                NotifyOfPropertyChange();
            }
        }

        public ProgressbarViewModel(IEnumerable<Tuple<string, bool>> steps)
        {
            Steps = new Stack<Tuple<string, bool>>(steps);
            Maximum = steps.Count();
        }

        public void Next()
        {
            var values = Steps.Pop();
            Value++;
            CurrentStep = values.Item1;
            IsIndeterminate = values.Item2;
        }

        public void End()
        {
            IsIndeterminate = false;
            CurrentStep = string.Empty;
        }

        public void Reset()
        {
            End();
            Maximum = 1;
            Value = 0;

            Steps.Clear();
        }
    }
}
