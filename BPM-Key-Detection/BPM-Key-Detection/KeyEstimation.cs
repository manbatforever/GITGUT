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
        public KeyEstimation()
        {

        }

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
            ChromaVector chromaVector = new ChromaVector(cqt.ToneAmplitudes, ConstantQTransform.TonesTotal, ConstantQTransform.TonesPerOctave); //TonesTotal and TonesPerOctave are accessed through type (they are constants)
            EstimateKey(chromaVector.MultiFrameVectorValues);
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

        

        

        

        public void EstimateKey(double[] chromaVectorValues)
        {
            KeyProfile majorProfile = new MajorProfile();
            KeyProfile minorProfile = new MinorProfile();
            double[][] MajorProfiles = majorProfile.CreateProfileForEachTonica();
            double[][] MinorProfiles = majorProfile.CreateProfileForEachTonica();

            Dictionary<string, double> majorSimilarities = new Dictionary<string, double>();
            Dictionary<string, double> minorSimilarities = new Dictionary<string, double>();
            string[] arrayOfToneNames = new string[] {"A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#"};
            //double[] MajorSimilarity = new double[12]; //Gammelt array før dictionaries
            //double[] MinorSimilarity = new double[12]; //Gammelt array før dictionaries
            for (int toneIndex = 0; toneIndex < 12; toneIndex++) //Loop igennem alle 12 toner, og fyld dictionaries med keys og values
            {
                majorSimilarities.Add(arrayOfToneNames[toneIndex], VectorOperations.CosineSimilarity(majorProfile.Profile, chromaVectorValues, 12)); //Key = tone name, value = Cosine similarity result 
                minorSimilarities.Add(arrayOfToneNames[toneIndex], VectorOperations.CosineSimilarity(minorProfile.Profile, chromaVectorValues, 12));
                //MajorSimilarity[i] = VectorOperations.CosineSimilarity(MajorProfiles[i], chromaVector, 12);
                //MinorSimilarity[i] = VectorOperations.CosineSimilarity(MinorProfiles[i], chromaVector, 12);
                //Console.Write(MajorSimilarity[i] + " ");
                //Console.WriteLine(MinorSimilarity[i]);
                Console.WriteLine(majorSimilarities.ElementAt(toneIndex) + " " + minorSimilarities.ElementAt(toneIndex));
            }

            //Console.Write(MajorSimilarity.ToList().IndexOf(MajorSimilarity.Max()) + " ");
            //Console.WriteLine(MinorSimilarity.ToList().IndexOf(MinorSimilarity.Max()));
            Console.ReadKey();
        }
    }
}
