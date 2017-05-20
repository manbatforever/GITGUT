using System;

namespace BPM_Key_Detection
{
    //Object: A type of window
    internal class HannWindow : Window
    {
        public HannWindow(int windowLength) : base(windowLength)
        {
        }

        protected override void WindowFunction()
        {
            for (int n = 0; n < _length; n++)
            {
                _windowArray[n] = 0.5d * (1d - Math.Cos(2d * Math.PI * n / _length));
            }
        }
    }
}
