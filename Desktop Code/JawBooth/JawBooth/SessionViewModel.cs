using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TxBDC_Common;

namespace JawBooth
{
    public class SessionViewModel : NotifyPropertyChangedObject
    {
        private SessionModel _model = null;
        private SimpleCommand _button_command;
        private SimpleCommand _manual_feed_command;
        private SimpleCommand _manual_stim_command;

        public SessionViewModel(string port_name)
        {
            _model = new SessionModel(port_name);
            _model.PropertyChanged += ExecuteReactionsToModelPropertyChanged;
        }
        
        public void CloseBackgroundThread()
        {
            _model.StopSession();
        }

        [ReactToModelPropertyChanged(new string[] { "RatName" })]
        public string RatName
        {
            get
            {
                return _model.RatName;
            }
            set
            {
                _model.RatName = TxBDC_Common.StringHelperMethods.CleanInput(value).Trim().ToUpper();
            }
        }

        [ReactToModelPropertyChanged(new string[] { "BoothLabel" })]
        public string BoothNumber
        {
            get
            {
                return _model.BoothLabel;
            }
        }

        [ReactToModelPropertyChanged(new string[] { "SecondsElapsed" })]
        public string SessionTimerText
        {
            get
            {
                return _model.SecondsElapsed;
            }
        }

        [ReactToModelPropertyChanged(new string[] { "SessionState" })]
        public string ButtonSessionContent
        {
            get
            {
                if (_model.SessionState == SessionModel.SessionStateEnum.NotRunning)
                {
                    return "Start";
                }
                else
                {
                    return "Stop";
                }
            }
        }

        [ReactToModelPropertyChanged(new string[] { "SessionState" })]
        public SolidColorBrush ButtonSessionColor
        {
            get
            {
                if (_model.SessionState == SessionModel.SessionStateEnum.NotRunning)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }
        }

        public SimpleCommand SessionButtonCommand
        {
            get
            {
                return _button_command ?? (_button_command = new SimpleCommand(() => ButtonPress(), true));
            }
        }

        public SimpleCommand ManualFeedCommand
        {
            get
            {
                return _manual_feed_command ?? (_manual_feed_command = new SimpleCommand(() => ManualFeed(), true));
            }
        }

        public SimpleCommand ManualStimCommand
        {
            get
            {
                return _manual_stim_command ?? (_manual_stim_command = new SimpleCommand(() => ManualStim(), true));
            }
        }

        public void ButtonPress()
        {
            if (_model.SessionState == SessionModel.SessionStateEnum.NotRunning)
            {
                _model.StartSession();
            }
            else
            {
                _model.StopSession();
            }
        }

        public void ManualFeed ()
        {
            _model.ManualFeed();
        }

        public void ManualStim ()
        {
            _model.ManualStim();
        }

        [ReactToModelPropertyChanged(new string[] { "FeederState" })]
        public SolidColorBrush FeederColor
        {
            get
            {
                if (_model.FeederState)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }
        }

        [ReactToModelPropertyChanged(new string[] { "NosepokeState" })]
        public SolidColorBrush NosepokeColor
        {
            get
            {
                if (_model.NosepokeState)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }
        }

        [ReactToModelPropertyChanged(new string[] { "VNSTriggerState" })]
        public SolidColorBrush StimulatorColor
        {
            get
            {
                if (_model.VNSTriggerState)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }
        }

        [ReactToModelPropertyChanged(new string[] { "FeederCount" })]
        public string FeederCount
        {
            get
            {
                return _model.FeederCount.ToString();
            }
        }

        [ReactToModelPropertyChanged(new string[] { "NosepokeCount" })]
        public string NosepokeCount
        {
            get
            {
                return _model.NosepokeCount.ToString();
            }
        }

        [ReactToModelPropertyChanged(new string[] { "VNSTriggerCount" })]
        public string StimulatorCount
        {
            get
            {
                return _model.VNSTriggerCount.ToString();
            }
        }

        public int StimulationSelectedIndex
        {
            get
            {
                if (_model.StimulationEnabled)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                int idx = value;
                if (idx == 0)
                {
                    _model.StimulationEnabled = false;
                }
                else
                {
                    _model.StimulationEnabled = true;
                }
            }
        }
    }
}
