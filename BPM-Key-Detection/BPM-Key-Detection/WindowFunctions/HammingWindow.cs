using System;

namespace BPM_Key_Detection
{
    //Object: A type of window
    internal class HammingWindow : Window
    {
        public HammingWindow(int windowLength) : base(windowLength)
        {
        }

        protected override void WindowFunction()
        {
            double alpha = 25d / 46d;
            for (int n = 0; n < _length; n++)
            {
                _windowArray[n] = alpha - (1d - alpha) * Math.Cos(2d * Math.PI * n / _length);
            }
        }
    }
}
