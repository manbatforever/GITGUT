using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testapp
{
    static class Program
    {
        static void Main()
        {
            string FilePath = @"C:\Users\Martin\Music\440randomfast.wav";
            int SampleRate;
            int Channels;
            double[] RawSamples = AudioSamples.GetRawSamples(FilePath, out SampleRate, out Channels);
            KeyDetection.GetKey(RawSamples, SampleRate, Channels);
        }
    }
}
