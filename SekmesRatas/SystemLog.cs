using System;
using System.IO;
using System.Windows.Forms;

namespace SekmesRatas
{
    class SystemLog
    {
        private string PATH = Application.StartupPath + "\\log.txt";
        private DateTime _date;


        public SystemLog(string action)
        {
            _date = DateTime.Now;
            if (!File.Exists(PATH))
            {
                using (var sw = File.CreateText(PATH))
                {
                    sw.WriteLine(_date + " " + action);
                }
            }
            else
            {
                using (var sw = new StreamWriter(PATH,true))
                {
                    sw.WriteLine(_date + " " + action);
                }
            }
        }

        public void LogAction(string action)
        {
            _date = DateTime.Now;
            using (TextWriter tw = new StreamWriter(PATH, true))
            {
                tw.WriteLine(_date + " " + action);
            }
        }
    }
}
