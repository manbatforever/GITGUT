using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //A type of mathematical function that creates a "window" of another function when applied.
    abstract class Window
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
