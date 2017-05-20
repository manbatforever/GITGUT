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

        protected override void WindowFunction()
        {
            for (int i = _lLimit; i < _length; i++)
            {
                _windowArray[i] = 1d - Math.Cos(2d * Math.PI * (((double)i - (double)_length) / ((double)_length - (double)_lLimit)));
            }
        }
    }
}
