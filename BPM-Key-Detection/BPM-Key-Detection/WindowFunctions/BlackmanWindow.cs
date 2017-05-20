using System;

namespace BPM_Key_Detection
{
    //Object: A type of window
    internal class BlackmanWindow : Window
    {
        public BlackmanWindow(int windowLength) : base(windowLength)
        {
        }

        protected override void WindowFunction()
        {
            double a = 0.16d;
            double a0 = (1d - a) / 2d;
            double a1 = 1d / 2d;
            double a2 = a / 2d;
            for (int n = 0; n < _length; n++)
            {
                _windowArray[n] = a0 - (a1 * Math.Cos((2d * Math.PI * n) / _length)) + (a2 * Math.Cos((4d * Math.PI * n) / _length));
            }
        }
    }
}
