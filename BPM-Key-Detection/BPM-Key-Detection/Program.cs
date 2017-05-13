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
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form_BpmKeyAnalyser());

            KeyEstimation keyEst = new KeyEstimation();
            double[] chrVector = new double[] { 0.12314, 0.145134, 0.1245, 0.1354893, 0.893159, 0.83195831, 0.91358315, 0.89134518378, 0.9135, 0.347341, 0.173843, 0.138473 };
            keyEst.EstimateKey(chrVector);
        }
    }
}
