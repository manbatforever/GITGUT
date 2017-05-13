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
            string filePath = @"C:\Users\MikHup\Desktop\p2musik\100 BPM Metronome.mp3";
            int sampleRate;
            int Channels;
            double[] samplesStereo = AudioSamples.GetRawSamples(filePath, out sampleRate, out Channels);

            int length = samplesStereo.Length;

            double[][] splitSamples = new double[Channels][];

            for (int channel = 0; channel < Channels; channel++)
            {
                splitSamples[channel] = new double[length / 2];
                for (int i = channel; i < length; i += Channels)
                {
                    splitSamples[channel][i / 2] = samplesStereo[i];
                }
            }
            BPMDetection sbd = new BPMDetection();

            sbd.start(splitSamples);

            Console.ReadKey();
        }
    }
}
