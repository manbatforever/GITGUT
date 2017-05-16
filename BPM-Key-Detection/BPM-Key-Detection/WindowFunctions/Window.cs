using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public abstract class Window
    {
        protected double[] _windowArray;
        private int _length;

        public Window()
        {
        }

        public Window(int windowLength)
        {
            _length = windowLength;
            WindowFunction(windowLength);
        }

        public double[] WindowArray { get => _windowArray; }

        public abstract void WindowFunction(int windowLength);
    }
}
