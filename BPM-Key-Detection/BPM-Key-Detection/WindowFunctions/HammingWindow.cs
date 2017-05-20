using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: A type of window
    internal class HammingWindow : Window
    {
        public HammingWindow(int windowLength) : base(windowLength)
        {
        }

        protected override void WindowFunction(int windowLength)
        {
            double alpha = 25d / 46d;
            double[] hammingWindow = new double[windowLength];
            for (int n = 0; n < windowLength; n++)
            {
                hammingWindow[n] = alpha - (1d - alpha) * Math.Cos(2d * Math.PI * n / windowLength);
            }
            _windowArray = hammingWindow;
        }
    }
}
