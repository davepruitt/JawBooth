using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JawBooth
{
    /// <summary>
    /// A simple class that represents a MotoTrak booth pairing with a com port
    /// </summary>
    public class BoothPairing
    {
        #region Fields

        public string BoothLabel = string.Empty;
        public string ComPort = string.Empty;
        public DateTime LastUpdated = DateTime.MinValue;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public BoothPairing()
        {
            //empty
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BoothPairing(string booth_label, string com_port, DateTime last_update)
        {
            BoothLabel = booth_label;
            ComPort = com_port;
            LastUpdated = last_update;
        }

        #endregion
    }
}
