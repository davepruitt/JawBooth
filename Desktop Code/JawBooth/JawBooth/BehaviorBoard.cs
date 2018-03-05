using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JawBooth
{
    public class BehaviorBoard
    {
        public enum BehaviorEvent
        {
            NosepokeEnter,
            NosepokeExit,
            FeederTriggered,
            VNSTriggered,
            None
        }

        private SerialPort SerialConnection;

        public BehaviorBoard()
        {

        }

        public bool ConnectToArduino(string portName)
        {
            try
            {
                SerialConnection = new SerialPort(portName, 115200);
                SerialConnection.DtrEnable = true;
                SerialConnection.ReadTimeout = 5;
                SerialConnection.Open();

                while (SerialConnection.BytesToRead < 1)
                {
                    Thread.Sleep(10);
                }
                
                string ardyResponse = SerialConnection.ReadLine().Trim();
                while (true)
                {
                    if (ardyResponse.Equals("READY"))
                    {
                        break;
                    }
                    else if (SerialConnection.BytesToRead < 1)
                    {
                        break;
                    }
                    else
                    {
                        ardyResponse = SerialConnection.ReadLine().Trim();
                    }
                }

                //Clear the buffer
                if (SerialConnection.BytesToRead > 0)
                {
                    SerialConnection.ReadExisting();
                }
                
                return true;
            }
            catch
            {
                //If any exception occurred, return false, indicating that we were
                //not able to connect to the serial port.
                return false;
            }
        }

        public void DisconnectFromArduino()
        {
            if (SerialConnection != null && SerialConnection.IsOpen)
            {
                SerialConnection.Close();
            }
        }
        
        public void TriggerFeeder(int feeder_number)
        {
            if (SerialConnection != null && SerialConnection.IsOpen)
            {
                SerialConnection.Write("F");
            }
        }

        public void TriggerStimulator()
        {
            if (SerialConnection != null && SerialConnection.IsOpen)
            {
                SerialConnection.Write("T");
            }
        }

        public void SetBoothNumber (int number)
        {
            var byte_number = Convert.ToByte(number);
            if (SerialConnection != null && SerialConnection.IsOpen)
            {
                SerialConnection.Write("S");
                SerialConnection.Write(new byte[] { byte_number }, 0, 1);
            }
        }

        public int GetBoothNumber ()
        {
            int result = -1;

            try
            {
                if (SerialConnection != null && SerialConnection.IsOpen)
                {
                    //Clear out anything left in the buffer
                    if (SerialConnection.BytesToRead > 0)
                    {
                        SerialConnection.ReadExisting();
                    }

                    //Send the command indicating we would like to know the booth number
                    SerialConnection.Write("L");

                    //Sleep for 500 ms to allow the Arduino time to respond
                    while (SerialConnection.BytesToRead < 1)
                    {
                        Thread.Sleep(10);
                    }
                    
                    //Read the returned booth number
                    int parsedInt = 0;
                    string stringToParse = SerialConnection.ReadLine();
                    bool parsedCorrectly = Int32.TryParse(stringToParse, out parsedInt);
                    if (parsedCorrectly)
                    {
                        result = parsedInt;
                    }
                }
            }
            catch
            {
                //empty
            }

            return result;
        }

        public void StreamEnable(bool enable)
        {
            if (SerialConnection != null && SerialConnection.IsOpen)
            {
                if (enable)
                {
                    SerialConnection.Write("E");
                }
                else
                {
                    SerialConnection.Write("D");
                }
            }
        }
        
        public List<int> ReadStream()
        {
            List<int> result = new List<int>();

            try
            {
                while (SerialConnection.BytesToRead > 0)
                {
                    var k = SerialConnection.ReadLine();
                    int v = 0;
                    bool success = Int32.TryParse(k, out v);
                    if (success)
                    {
                        result.Add(v);
                    }
                }
            }
            catch (Exception)
            {
                //empty
            }

            return result;
        }

        public static bool QueryIndividualArduinoDevice (string com_port)
        {
            BehaviorBoard b = new BehaviorBoard();
            bool success = b.ConnectToArduino(com_port);
            if (success)
            {
                //Get the booth number
                int booth_number = b.GetBoothNumber();

                //Disconnect from the Arduino
                b.DisconnectFromArduino();

                //Update the booth pairing
                JawBoothConfiguration.GetInstance().UpdateBoothPairing(com_port, booth_number.ToString());
                JawBoothConfiguration.GetInstance().SaveBoothPairings();

                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<USBDeviceInfo> QueryConnectedArduinoDevices()
        {
            //Create a list to hold information from USB devices
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            //Query all connected devices
            var searcher = new ManagementObjectSearcher(@"SELECT * FROM WIN32_SerialPort");
            var collection = searcher.Get();

            //Grab the information we need
            foreach (var device in collection)
            {
                string id = (string)device.GetPropertyValue("DeviceID");
                string desc = (string)device.GetPropertyValue("Description");
                USBDeviceInfo d = new USBDeviceInfo(desc, id, false);

                //Check to see if the available serial port is a connected Arduino device
                if (d.Description.Contains("Arduino"))
                {
                    //If so, add the Arduino to our list of devices
                    devices.Add(d);
                }
            }

            //Dispose of the collection of queried devices
            collection.Dispose();

            //Return the list of devices that were found
            return devices;
        }
    }
}
