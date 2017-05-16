using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class DefaultWindow : Window
    {
        public DefaultWindow()
        {
        }

        public override void WindowFunction(int windowLength)
        {
            double[] defaultWindow = new double[windowLength];
            for (int i = 0; i < windowLength; i++)
            {
                defaultWindow[i] = 1;
            }
            _windowArray = defaultWindow;
        }
    }
}
