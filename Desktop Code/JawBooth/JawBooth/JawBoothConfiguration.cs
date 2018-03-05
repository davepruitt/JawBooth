using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JawBooth
{
    public class JawBoothConfiguration
    {
        #region Singleton

        private static JawBoothConfiguration _instance = null;
        private static object _instance_lock = new object();

        private JawBoothConfiguration()
        {
            //empty
        }

        public static JawBoothConfiguration GetInstance()
        {
            if (_instance == null)
            {
                lock (_instance_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new JawBoothConfiguration();
                    }
                }
            }

            return _instance;
        }

        #endregion

        #region Privata data members

        private string _configuration_file_name = "config.txt";
        private string _booth_pairings_file_name = "booth_pairings.txt";

        public ConcurrentBag<BoothPairing> BoothPairings = new ConcurrentBag<BoothPairing>();

        public Dictionary<string, int> Booth_Specific_IR_Thresholds = new Dictionary<string, int>();
        public int IR_Threshold = 500;
        public int MaximumFeeds = Int32.MaxValue;
        public int MaximumDuration = Int32.MaxValue;
        public int VNS_Delay = 0;
        public int FeederTiming = 0;
        public int FeederPercentage = 0;
        public string FeederChoice = string.Empty;
        public string SavePath = string.Empty;
        public string SecondarySavePath = string.Empty;

        #endregion

        #region Methods

        public void LoadConfigurationFile()
        {
            //Load the config file
            List<string> file_lines = TxBDC_Common.ConfigurationFileLoader.LoadConfigurationFile(_configuration_file_name);

            //Iterate through each line and parse out the data
            for (int i = 0; i < file_lines.Count; i++)
            {
                string this_line = file_lines[i];
                string[] split_string = this_line.Split(new char[] { ':' }, 2);
                string key = split_string[0].Trim();
                string value = split_string[1].Trim();

                int result = 0;
                bool success = false;

                if (key.Equals("VNS Delay", StringComparison.InvariantCultureIgnoreCase))
                {
                    success = Int32.TryParse(value, out result);
                    if (success)
                    {
                        VNS_Delay = result;
                    }
                }
                else if (key.Equals("Feeder Timing"))
                {
                    success = Int32.TryParse(value, out result);
                    if (success)
                    {
                        FeederTiming = result;
                    }
                }
                else if (key.Equals("Maximum Feeds"))
                {
                    success = Int32.TryParse(value, out result);
                    if (success)
                    {
                        MaximumFeeds = result;
                    }
                }
                else if (key.Equals("Maximum Duration"))
                {
                    success = Int32.TryParse(value, out result);
                    if (success)
                    {
                        MaximumDuration = result;
                    }
                }
                else if (key.Equals("Feeder Percentage"))
                {
                    success = Int32.TryParse(value, out result);
                    if (success)
                    {
                        FeederPercentage = result;
                    }
                }
                else if (key.Equals("Save Path"))
                {
                    SavePath = value;
                }
                else if (key.Equals("Secondary Save Path"))
                {
                    SecondarySavePath = value;
                }
                else if (key.Equals("IR Threshold"))
                {
                    success = Int32.TryParse(value, out result);
                    if (success)
                    {
                        IR_Threshold = result;
                    }
                }
                else if (key.Equals("Booth Specific IR Threshold"))
                {
                    var value_split = value.Split(new char[] { ',' }, 2).ToList();
                    if (value_split.Count >= 2)
                    {
                        string booth_name = value_split[0];
                        int booth_thresh = 0;

                        success = Int32.TryParse(value_split[1], out booth_thresh);
                        if (success)
                        {
                            Booth_Specific_IR_Thresholds[booth_name] = booth_thresh;
                        }
                    }
                }
            }
        }

        public void UpdateBoothPairing(string com_port, string booth_label)
        {
            var pairing = BoothPairings.Where(x => x.ComPort.Equals(com_port)).FirstOrDefault();
            if (pairing != null)
            {
                //If the booth pairing already exists in our collection, let's update it.
                pairing.BoothLabel = booth_label;
                pairing.LastUpdated = DateTime.Now;
            }
            else
            {
                //Otherwise, create a new booth pairing to be stored.
                BoothPairing new_pairing = new BoothPairing(booth_label, com_port, DateTime.Now);
                BoothPairings.Add(new_pairing);
            }
        }

        public void ReadBoothPairings()
        {
            string booth_pairings_file_name = _booth_pairings_file_name;

            FileInfo booth_pairings_file_info = new FileInfo(booth_pairings_file_name);
            if (booth_pairings_file_info.Exists)
            {
                //Open a stream to read the booth pairings configuration file
                try
                {
                    //Load the configuration file
                    List<string> lines = TxBDC_Common.ConfigurationFileLoader.LoadConfigurationFile(booth_pairings_file_name);

                    //Now parse the input
                    for (int i = 0; i < lines.Count; i++)
                    {
                        string thisLine = lines[i];
                        string[] splitString = thisLine.Split(new char[] { ',' }, 4);

                        string booth_name = splitString[0].Trim();
                        string com_port = splitString[1].Trim();
                        DateTime last_updated = DateTime.MinValue;
                        
                        //Parse out the "last updated" date and time if it exists
                        if (splitString.Length >= 3)
                        {
                            string last_updated_string = splitString[2].Trim();
                            last_updated = DateTime.Parse(last_updated_string);
                        }
                        
                        //Instantiate a booth pairing object
                        BoothPairing pairing = new BoothPairing(booth_name, com_port, last_updated);

                        //Add the booth pairing to our set
                        BoothPairings.Add(pairing);
                    }
                }
                catch
                {
                    //empty
                }
            }
        }
        
        public void SaveBoothPairings()
        {
            try
            {
                string booth_pairings_file_name = _booth_pairings_file_name;

                //Open a stream to write to the file
                StreamWriter writer = new StreamWriter(booth_pairings_file_name, false);

                //Write each booth pairing to the file
                foreach (var pairing in BoothPairings)
                {
                    string last_updated_string = pairing.LastUpdated.ToString();
                    string output_string = pairing.BoothLabel + ", " + pairing.ComPort + ", " + last_updated_string;
                    writer.WriteLine(output_string);
                }

                //Close the file handle
                writer.Close();
            }
            catch
            {
                //empty
            }
        }

        #endregion
    }
}
