using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPM_Key_Detection
{
    internal static class Program
    {
        // The main entry point for the application.
        [STAThread]
        static void Main()
        {
            KeyEstimationLogs.Start();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form_BpmKeyAnalyser());
        }
    }
}
