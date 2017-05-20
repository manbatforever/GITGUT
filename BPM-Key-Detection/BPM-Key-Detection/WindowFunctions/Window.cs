namespace BPM_Key_Detection
{
    //A type of mathematical function that creates a "window" of another function when applied.
    internal abstract class Window
    {
        protected double[] _windowArray;
        private int _length;

        public Window(int windowLength)
        {
            _length = windowLength;
            _windowArray = new double[windowLength];
            WindowFunction(windowLength);
        }
        protected abstract void WindowFunction(int windowLength);

        public double[] WindowArray { get => _windowArray; }
    }
}
