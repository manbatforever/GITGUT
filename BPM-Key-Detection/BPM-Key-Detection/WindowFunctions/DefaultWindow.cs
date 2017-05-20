namespace BPM_Key_Detection
{
    //Object: A default window which has no effect when applied
    internal class DefaultWindow : Window
    {
        public DefaultWindow(int windowLength) : base(windowLength)
        {
        }

        protected override void WindowFunction(int windowLength)
        {
            for (int i = 0; i < windowLength; i++)
            {
                _windowArray[i] = 1;
            }
        }
    }
}
