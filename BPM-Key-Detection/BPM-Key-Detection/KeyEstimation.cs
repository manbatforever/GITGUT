using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class KeyEstimation
    {
        private MusicFile _musicFile;
        

        public KeyEstimation(MusicFile musicFile)
        {
            _musicFile = musicFile;
        }

        public void Start()
        {
            double[] monoSamples = _musicFile.GetRawSamples();
            if (_musicFile.Channels != 1)
            {
                monoSamples = ToMono(monoSamples, _musicFile.Channels);
            }
            ConstantQTransform cqt = new ConstantQTransform(monoSamples, _musicFile.Samplerate);
        }

        private double[] ToMono(double[] multiChannelSamples, int channels)
        {

            int multiChannelLength = multiChannelSamples.Length;
            int monoLength = multiChannelLength / channels;
            double[] monoSamples = new double[monoLength + 2];
            int monoCounter = 0;
            for (int channel = 0; channel + channels < multiChannelLength; channel += channels)
            {
                double channelSum = 0;
                for (int a = channel; a < channel + channels; a++)
                {
                    channelSum += multiChannelSamples[a];
                }
                monoSamples[monoCounter] = channelSum / channels;
                monoCounter++;
            }
            return monoSamples;
        }

        

        

        

        private static void EstimateKey(double[] ChromaVector)
        {
            double[][] MajorProfiles = CreateProfileForEachTonica(MajorProfile);
            double[][] MinorProfiles = CreateProfileForEachTonica(MinorProfile);
            double[] MajorSimilarity = new double[12];
            double[] MinorSimilarity = new double[12];
            for (int i = 0; i < 12; i++)
            {
                MajorSimilarity[i] = CosineSimilarity(MajorProfiles[i], ChromaVector, 12);
                MinorSimilarity[i] = CosineSimilarity(MinorProfiles[i], ChromaVector, 12);
                Console.Write(MajorSimilarity[i] + " ");
                Console.WriteLine(MinorSimilarity[i]);
            }
            Console.Write(MajorSimilarity.ToList().IndexOf(MajorSimilarity.Max()) + " ");
            Console.WriteLine(MinorSimilarity.ToList().IndexOf(MinorSimilarity.Max()));
            Console.ReadKey();
        }
    }
}
