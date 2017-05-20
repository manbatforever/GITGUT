using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: A type of window
    internal class BlackmanWindow : Window
    {
        public BlackmanWindow(int windowLength) : base(windowLength)
        {
        }

        protected override void WindowFunction(int windowLength)
        {
            double a = 0.16d;
            double a0 = (1d - a) / 2d;
            double a1 = 1d / 2d;
            double a2 = a / 2d;
            double[] blackmanWindow = new double[windowLength];
            for (int n = 0; n < windowLength; n++)
            {
                blackmanWindow[n] = a0 - (a1 * Math.Cos((2d * Math.PI * n) / windowLength)) + (a2 * Math.Cos((4d * Math.PI * n) / windowLength));
            }
            _windowArray = blackmanWindow;
        }
    }
}
