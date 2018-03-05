using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JawBooth
{
    public class SessionWriter
    {
        StreamWriter _writer = null;
        string PrimaryFullPath = string.Empty;

        public SessionWriter()
        {
            //empty
        }

        public void OpenFileForWriting(string RatName, bool StimEnabled)
        {
            string base_path = JawBoothConfiguration.GetInstance().SavePath;

            //If the base path doesn't end with a back-slash, then add one to the end of it
            if (!base_path.EndsWith(@"\"))
            {
                base_path = base_path + @"\";
            }

            string base_path_plus_rat_folder = base_path + RatName + @"\";
            string file_name = RatName + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".txt";
            string full_path = base_path_plus_rat_folder + file_name;

            this.PrimaryFullPath = full_path;

            //Create directory if it doesn't exist
            new FileInfo(full_path).Directory.Create();

            _writer = new StreamWriter(full_path, false);
            _writer.WriteLine(RatName);
            _writer.WriteLine("Stimulation: " + StimEnabled.ToString());
            _writer.WriteLine("Start: " + DateTime.Now.ToString("yyyyMMddTHHmmss"));
        }

        public void WriteEvent(BehaviorBoard.BehaviorEvent e, DateTime t)
        {
            if (_writer != null)
            {
                _writer.WriteLine(e.ToString() + ", " + DateTime.Now.ToString("yyyyMMddTHHmmssfff"));
            }
        }

        public void CloseFile()
        {
            if (_writer != null)
            {
                _writer.WriteLine("End: " + DateTime.Now.ToString("yyyyMMddTHHmmss"));
                _writer.Close();
            }

            _writer = null;
        }

        public void CopyFileToSecondaryPath(string RatName)
        {
            string base_path = JawBoothConfiguration.GetInstance().SavePath;
            string secondary_base_path = JawBoothConfiguration.GetInstance().SecondarySavePath;

            //If the base path doesn't end with a back-slash, then add one to the end of it
            if (!base_path.EndsWith(@"\"))
            {
                base_path = base_path + @"\";
            }

            //If the base path doesn't end with a back-slash, then add one to the end of it
            if (!secondary_base_path.EndsWith(@"\"))
            {
                secondary_base_path = secondary_base_path + @"\";
            }

            string full_path = this.PrimaryFullPath;

            string file_name = RatName + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".txt";
            string secondary_base_path_plus_rat_folder = secondary_base_path + RatName + @"\";
            string secondary_full_path = secondary_base_path_plus_rat_folder + file_name;

            //Create directory if it doesn't exist
            new FileInfo(secondary_full_path).Directory.Create();

            //FileInfo k = new FileInfo(full_path);
            //k.CopyTo(secondary_full_path);
            System.IO.File.Copy(full_path, secondary_full_path);
        }
    }
}
