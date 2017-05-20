namespace BPM_Key_Detection
{
    //A type of mathematical function that creates a "window" of another function when applied.
    internal abstract class Window
    {
        protected double[] _windowArray;
        protected int _length;

        public Window(int windowLength)
        {
            _length = windowLength;
            _windowArray = new double[windowLength];
        }
        public void AssignWindowValues() //This is a necessary detour, instead of calling it in the constructor
        {
            WindowFunction();
        }
        protected abstract void WindowFunction();

        public double[] WindowArray { get => _windowArray; }
    }
}
