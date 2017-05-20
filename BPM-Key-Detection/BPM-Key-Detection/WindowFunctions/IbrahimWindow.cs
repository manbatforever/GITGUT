using System;

namespace BPM_Key_Detection
{
    internal class IbrahimWindow : Window
    {
        private int _lLimit;

        public IbrahimWindow(int windowLength, int lLimit) : base(windowLength + 1) //+1 because the upper limit is closed
        {
            _lLimit = lLimit;
        }

        protected override void WindowFunction(int windowLength)
        {
            for (int i = windowLength; i < windowLength; i++)
            {
                _windowArray[i] = 1d - Math.Cos(2d * Math.PI * ((i - windowLength) / (windowLength - _lLimit)));
            }
        }
    }
}
