using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: A type of window
    internal class HannWindow : Window
    {
        public HannWindow(int windowLength) : base(windowLength)
        {
        }

        protected override void WindowFunction(int windowLength)
        {
            double[] hannWindow = new double[windowLength];
            for (int n = 0; n < windowLength; n++)
            {
                hannWindow[n] = 0.5d * (1d - Math.Cos(2d * Math.PI * n / windowLength));
            }
            _windowArray = hannWindow;
        }
    }
}
