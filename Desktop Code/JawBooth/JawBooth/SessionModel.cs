using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using TxBDC_Common;

namespace JawBooth
{
    public class SessionModel : NotifyPropertyChangedObject
    {
        #region Private data members

        private string _com_port = string.Empty;
        private string _rat_name = string.Empty;
        private bool _stimulation_enabled = false;
        private Stopwatch _timer = new Stopwatch();
        private BackgroundWorker _background_thread = null;
        private ConcurrentBag<string> _background_properties_to_update = new ConcurrentBag<string>();

        private Stopwatch _feeder_timer = new Stopwatch();
        private Stopwatch _vns_trigger_timer = new Stopwatch();
        
        private Random rnd_gen = new Random();

        private DateTime last_actual_feed_time = DateTime.MinValue;
        private DateTime last_left_nosepoke_entry = DateTime.MinValue;
        private DateTime last_right_nosepoke_entry = DateTime.MinValue;
        private DateTime last_trial_completion_time = DateTime.MinValue;

        private bool vns_delay_active = false;
        private DateTime vns_delay_start = DateTime.Now;

        private string _booth_label = string.Empty;
        private int _ir_threshold = 500;

        private SessionWriter session_writer = new SessionWriter();

        public enum SessionStateEnum
        {
            NotRunning,
            SetUpNewSession,
            Running,
            FinalizeSession
        }

        public enum TrialStateEnum
        {
            WaitingForNosepokeEntry,
            Completed
        }

        private SessionStateEnum _session_state = SessionStateEnum.NotRunning;
        private TrialStateEnum _current_trial_state = TrialStateEnum.Completed;

        private bool _execute_manual_feed = false;
        private bool _execute_manual_stim = false;

        #endregion

        #region Constructor

        public SessionModel(string com_port)
        {
            _com_port = com_port;

            //Set up the background thread
            _background_thread = new BackgroundWorker();
            _background_thread.WorkerSupportsCancellation = true;
            _background_thread.WorkerReportsProgress = true;
            _background_thread.DoWork += _background_thread_DoWork;
            _background_thread.ProgressChanged += _background_thread_ProgressChanged;
            _background_thread.RunWorkerCompleted += _background_thread_RunWorkerCompleted;

            //Run the background thread
            _background_thread.RunWorkerAsync();
        }

        public void StopBackgroundThread()
        {
            _background_thread.CancelAsync();
        }

        private void _background_thread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //empty
        }

        private void _background_thread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string item = string.Empty;
            while (!_background_properties_to_update.IsEmpty)
            {
                _background_properties_to_update.TryTake(out item);
                NotifyPropertyChanged(item);
            }
        }

        private void _background_thread_DoWork(object sender, DoWorkEventArgs e)
        {
            _timer.Reset();

            //Connect to the Arduino board
            BehaviorBoard board = new BehaviorBoard();
            board.ConnectToArduino(_com_port);

            //Grab the booth number
            var booth_num = board.GetBoothNumber();
            BoothLabel = booth_num.ToString();

            //Set the default IR threshold for the nosepoke
            _ir_threshold = JawBoothConfiguration.GetInstance().IR_Threshold;

            //Check to see if their is a booth-specific IR threshold
            var booth_names = JawBoothConfiguration.GetInstance().Booth_Specific_IR_Thresholds.Keys.ToList();
            if (booth_names.Contains(BoothLabel))
            {
                _ir_threshold = JawBoothConfiguration.GetInstance().Booth_Specific_IR_Thresholds[BoothLabel];
            }

            //SLeep for 50 milliseconds to allow the previous operation to complete
            Thread.Sleep(50);
            
            //Enable streaming from the board
            board.StreamEnable(true);

            while (!_background_thread.CancellationPending)
            {
                HandleState(board);

                //Update the GUI based on what is happening in the background thread
                _background_thread.ReportProgress(0);

                //Sleep the thread for a bit
                Thread.Sleep(33);
            }

            //Disconnect from the Arduino board
            board.DisconnectFromArduino();
        }

        private void HandleState(BehaviorBoard board)
        {
            bool inc_feeder = false;
            bool inc_nosepoke = false;
            bool inc_stim = false;

            //Read in nosepoke data from the board
            var nosepoke_input = board.ReadStream();

            //Check to see if the nosepoke state has changed
            if (nosepoke_input.Any(x => x > _ir_threshold) && !NosepokeState)
            {
                NosepokeState = true;

                inc_nosepoke = true;
                session_writer.WriteEvent(BehaviorBoard.BehaviorEvent.NosepokeEnter, DateTime.Now);
                _background_properties_to_update.Add("NosepokeState");
            }
            else if (nosepoke_input.All(x => x <= _ir_threshold) && NosepokeState)
            {
                NosepokeState = false;

                session_writer.WriteEvent(BehaviorBoard.BehaviorEvent.NosepokeExit, DateTime.Now);
                _background_properties_to_update.Add("NosepokeState");
            }

            if (_execute_manual_feed)
            {
                _execute_manual_feed = false;
                board.TriggerFeeder(1);

                if (SessionState == SessionStateEnum.Running)
                {
                    last_actual_feed_time = DateTime.Now;
                    session_writer.WriteEvent(BehaviorBoard.BehaviorEvent.FeederTriggered, DateTime.Now);
                    inc_feeder = true;
                }
                
                FeederState = true;
                _feeder_timer.Restart();
                _background_properties_to_update.Add("FeederState");
            }

            if (_execute_manual_stim)
            {
                _execute_manual_stim = false;
                board.TriggerStimulator();
                _vns_trigger_timer.Restart();

                if (SessionState == SessionStateEnum.Running)
                {
                    session_writer.WriteEvent(BehaviorBoard.BehaviorEvent.VNSTriggered, DateTime.Now);
                    VNSTriggerCount++;
                    _background_properties_to_update.Add("VNSTriggerCount");
                }
                
                VNSTriggerState = true;
                _background_properties_to_update.Add("VNSTriggerState");
            }

            //Update the feeder in the GUI
            if (_feeder_timer.ElapsedMilliseconds > 1000)
            {
                FeederState = false;
                _feeder_timer.Reset();
                _background_properties_to_update.Add("FeederState");
            }

            //Update the VNS trigger in the GUI
            if (_vns_trigger_timer.ElapsedMilliseconds > 1000)
            {
                VNSTriggerState = false;
                _vns_trigger_timer.Reset();
                _background_properties_to_update.Add("VNSTriggerState");
            }

            switch (SessionState)
            {
                case SessionStateEnum.NotRunning:
                    break;
                case SessionStateEnum.SetUpNewSession:

                    //Reset all the counts to be 0
                    FeederCount = 0;
                    _background_properties_to_update.Add("FeederCount");

                    NosepokeCount = 0;
                    _background_properties_to_update.Add("NosepokeCount");

                    VNSTriggerCount = 0;
                    _background_properties_to_update.Add("VNSTriggerCount");

                    //Progress the state to be "running"
                    SessionState = SessionStateEnum.Running;

                    //Start the timer
                    _timer.Restart();

                    //Set the last time that a feed occurred to be now
                    last_trial_completion_time = DateTime.Now;

                    //Set up a new file for this rat
                    session_writer.OpenFileForWriting(RatName, StimulationEnabled);

                    break;
                case SessionStateEnum.Running:
                    
                    if (_timer.Elapsed.TotalMinutes >= JawBoothConfiguration.GetInstance().MaximumDuration)
                    {
                        //Finalize the session if we have exceeded the maximum duration of a session
                        SessionState = SessionStateEnum.FinalizeSession;
                        break;
                    }

                    if (_current_trial_state == TrialStateEnum.Completed)
                    {
                        //Check to see if it is time to feed
                        var current_time = DateTime.Now;
                        var time_elapsed = current_time - last_trial_completion_time;
                        if (time_elapsed.TotalSeconds >= JawBoothConfiguration.GetInstance().FeederTiming)
                        {
                            last_trial_completion_time = DateTime.Now;

                            //Finalize the session if we have exceeded the maximum number of feeds
                            if (FeederCount >= JawBoothConfiguration.GetInstance().MaximumFeeds)
                            {
                                SessionState = SessionStateEnum.FinalizeSession;
                                break;
                            }

                            //Now choose whether to actually feed or not
                            int rnd_result = rnd_gen.Next(1, 100);
                            if (rnd_result <= JawBoothConfiguration.GetInstance().FeederPercentage)
                            {
                                _current_trial_state = TrialStateEnum.WaitingForNosepokeEntry;

                                board.TriggerFeeder(1);
                                last_actual_feed_time = DateTime.Now;

                                session_writer.WriteEvent(BehaviorBoard.BehaviorEvent.FeederTriggered, DateTime.Now);
                                FeederState = true;
                                inc_feeder = true;
                                _feeder_timer.Restart();
                                _background_properties_to_update.Add("FeederState");
                            }
                        }
                    }

                    if (inc_feeder)
                    {
                        FeederCount++;
                        _background_properties_to_update.Add("FeederCount");
                    }

                    if (inc_nosepoke)
                    {
                        if (last_left_nosepoke_entry < last_actual_feed_time)
                        {
                            last_left_nosepoke_entry = DateTime.Now;
                            last_trial_completion_time = DateTime.Now;
                            _current_trial_state = TrialStateEnum.Completed;

                            if (StimulationEnabled)
                            {
                                inc_stim = true;
                            }
                        }

                        NosepokeCount++;
                        _background_properties_to_update.Add("NosepokeCount");
                    }

                    if (inc_stim)
                    {
                        vns_delay_active = true;
                        vns_delay_start = DateTime.Now;
                        inc_stim = false;
                    }

                    if (vns_delay_active)
                    {
                        var time_elapsed = DateTime.Now - vns_delay_start;
                        if (time_elapsed.TotalMilliseconds >= JawBoothConfiguration.GetInstance().VNS_Delay)
                        {
                            vns_delay_active = false;

                            board.TriggerStimulator();
                            _vns_trigger_timer.Restart();

                            session_writer.WriteEvent(BehaviorBoard.BehaviorEvent.VNSTriggered, DateTime.Now);

                            VNSTriggerState = true;
                            VNSTriggerCount++;
                            _background_properties_to_update.Add("VNSTriggerState");
                            _background_properties_to_update.Add("VNSTriggerCount");
                        }
                    }

                    break;
                case SessionStateEnum.FinalizeSession:

                    //Progress the state to be "not running"
                    SessionState = SessionStateEnum.NotRunning;

                    _timer.Stop();
                    //_timer.Reset();

                    session_writer.CloseFile();
                    session_writer.CopyFileToSecondaryPath(RatName);

                    break;
            }

            _background_properties_to_update.Add("SecondsElapsed");
        }

        #endregion

        #region Public properties

        public bool FeederState = false;
        public bool NosepokeState = false;
        public bool VNSTriggerState = false;

        public int FeederCount = 0;
        public int NosepokeCount = 0;
        public int VNSTriggerCount = 0;

        public string RatName
        {
            get
            {
                return _rat_name;
            }
            set
            {
                _rat_name = value;
                NotifyPropertyChanged("RatName");
            }
        }

        public string BoothLabel
        {
            get
            {
                return _booth_label;
            }
            set
            {
                _booth_label = value;
                _background_properties_to_update.Add("BoothLabel");
            }
        }

        public bool StimulationEnabled
        {
            get
            {
                return _stimulation_enabled;
            }
            set
            {
                _stimulation_enabled = value;
            }
        }

        public SessionStateEnum SessionState
        {
            get
            {
                return _session_state;
            }
            set
            {
                _session_state = value;
                _background_properties_to_update.Add("SessionState");
            }
        }

        public string SecondsElapsed
        {
            get
            {
                return _timer.Elapsed.ToString(@"hh\:mm\:ss");
            }
        }

        #endregion

        #region Public Methods

        public void StartSession()
        {
            SessionState = SessionStateEnum.SetUpNewSession;
        }

        public void StopSession()
        {
            if (SessionState == SessionStateEnum.Running)
                SessionState = SessionStateEnum.FinalizeSession;
        }

        public void ManualFeed ()
        {
            _execute_manual_feed = true;
        }

        public void ManualStim ()
        {
            _execute_manual_stim = true;
        }

        #endregion
    }
}
