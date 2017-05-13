using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPM_Key_Detection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            MusicFile music = new MusicFile(@"C:\Users\Martin\Music\Avicii_-_Levels_-_12B.mp3");
            KeyEstimation keyest = new KeyEstimation(music);
            //keyest.TestCQT();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form_BpmKeyAnalyser());
        }
    }
}
