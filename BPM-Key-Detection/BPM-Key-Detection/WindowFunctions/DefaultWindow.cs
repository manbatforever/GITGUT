namespace BPM_Key_Detection
{
    //Object: A default window which has no effect when applied
    internal class DefaultWindow : Window
    {
        public DefaultWindow(int windowLength) : base(windowLength)
        {
        }

        protected override void WindowFunction()
        {
            for (int i = 0; i < _length; i++)
            {
                _windowArray[i] = 1;
            }
        }
    }
}
