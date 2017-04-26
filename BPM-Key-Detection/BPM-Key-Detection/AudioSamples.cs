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
        public static double[] GetMonoSamples(string FilePath, out int SampleRate)
        {
            AudioFileReader File = new AudioFileReader(FilePath);
            SampleRate = File.WaveFormat.SampleRate;
            int rawLength = (int)(File.Length / 4);
            int channels = File.WaveFormat.Channels;
            double[] output = new double[rawLength / channels];

            float[] buffer = new float[rawLength];
            File.Read(buffer, 0, rawLength);

            int monoCount = 0;
            for (int i = 0; i + channels < rawLength; i += channels)
            {
                float channelSum = 0;
                for (int a = i; a < i + channels; a++)
                {
                    channelSum += buffer[a];
                }
                output[monoCount] = channelSum / channels;
                monoCount++;
            }
            return output;
        }
    }
}
