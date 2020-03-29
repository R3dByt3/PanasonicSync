using Caliburn.Micro;
using PanasonicSync.GUI.Enums;
using System.Collections.Generic;
using System.Linq;

namespace PanasonicSync.GUI.ViewModels
{
    public class ProgressbarViewModel : ViewModelBase, IScreen, IHandle<CommandEnum>, IHandle<IEnumerable<string>>, IHandle<string>, IHandle<int>
    {
        private readonly bool _doesHandle;

        private string _currentStep;
        private bool _isIndeterminate;
        private double _maximum;
        private double _value;
        private Stack<string> _steps;

        public Stack<string> Steps
        {
            get => _steps;
            set
            {
                _steps = value;
                NotifyOfPropertyChange();

                Maximum = _steps.Count();
            }
        }

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

        public ProgressbarViewModel()
        {
            _doesHandle = true;
        }

        public ProgressbarViewModel(bool doesHandle)
        {
            _doesHandle = doesHandle;
        }

        public void SetSteps(IEnumerable<string> steps)
        {
            Steps = new Stack<string>(steps.Reverse());
        }

        public void Next()
        {
            Value++;
            CurrentStep = Steps.Pop();
        }

        public void End()
        {
            IsIndeterminate = false;
            CurrentStep = string.Empty;
            Maximum = 1;
            Value = 0;
        }

        public void Reset()
        {
            End();

            Steps.Clear();
        }

        public void Handle(CommandEnum message)
        {
            if (!_doesHandle)
                return;

            switch (message)
            {
                case CommandEnum.ProgressbarEnd:
                    End();
                    break;
                case CommandEnum.ProgressbarNext:
                    Next();
                    break;
                case CommandEnum.IsIndetermined:
                    IsIndeterminate = true;
                    break;
                case CommandEnum.IsNotIndetermined:
                    IsIndeterminate = false;
                    break;
            }
        }

        public void Handle(IEnumerable<string> message)
        {
            if (!_doesHandle)
                return;

            SetSteps(message);
        }

        public void Handle(string message)
        {
            CurrentStep = message;
        }

        public void Handle(int message)
        {
            Value = message;
        }
    }
}
