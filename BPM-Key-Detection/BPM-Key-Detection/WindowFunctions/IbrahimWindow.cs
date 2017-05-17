using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class IbrahimWindow : Window
    {
        public override void WindowFunction(int windowLength)
        {

        }
        public IbrahimWindow( double lLimit, double rLimit) :base() //This window is a special snowflake and is not constructed from a windowlength
        {
            SpectralWindowFunction(lLimit, rLimit);
        }

        public void SpectralWindowFunction(double lLimit, double rLimit)
        {
            _windowArray = new double[(int)rLimit + 1];
            for (int i = (int)lLimit; i <= rLimit; i++)
            {
                _windowArray[i] = 1d - Math.Cos(2d * Math.PI * ((i - lLimit) / (rLimit - lLimit)));
            }
        }
    }
}
