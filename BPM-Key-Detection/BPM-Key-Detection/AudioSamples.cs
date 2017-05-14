using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace testapp
{
    static class AudioSamples
    {
        public static double[] GetRawSamples(string FilePath, out int SampleRate, out int Channels)
        {
            AudioFileReader File = new AudioFileReader(FilePath);

            SampleRate = File.WaveFormat.SampleRate;
            Channels = File.WaveFormat.Channels;

            NAudio.Dsp.BiQuadFilter lowPassFilter = NAudio.Dsp.BiQuadFilter.LowPassFilter(SampleRate, 10000, 1);

            int floatLength = (int)(File.Length / 4);
            double[] output = new double[floatLength];
            float[] buffer = new float[floatLength];
            File.Read(buffer, 0, floatLength);
            for (int i = 0; i < floatLength; i++)
            {
                output[i] = lowPassFilter.Transform(buffer[i]);
            }
            return output;
        }
    }
}
