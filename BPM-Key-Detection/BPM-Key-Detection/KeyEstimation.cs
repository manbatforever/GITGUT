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
        private string _musicFileKey;

        public string MusicFileKey { get => _musicFileKey; }

        public KeyEstimation(MusicFile musicFile)
        {
            _musicFile = musicFile;
            Start();
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
            int key = EstimateKey(chromaVector.VectorValues);
            _musicFileKey = FormatToCamelotNotation(key);
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

        

        

        

        private int EstimateKey(double[] chromaVectorValues)
        {
            KeyProfile majorProfile = new MajorProfile();
            KeyProfile minorProfile = new MinorProfile();

            double[] allSimilarities = new double[24];
            for (int i = 0; i < 12; i++)
            {
                allSimilarities[i] = VectorOperations.CosineSimilarity(majorProfile.CreateProfileForTonica(i), chromaVectorValues, 12);
                allSimilarities[i + 12] = VectorOperations.CosineSimilarity(minorProfile.CreateProfileForTonica(i), chromaVectorValues, 12);
            }

            return allSimilarities.ToList().IndexOf(allSimilarities.Max());

            //Dictionary<string, double> majorSimilarities = new Dictionary<string, double>();
            //Dictionary<string, double> minorSimilarities = new Dictionary<string, double>();
            //string[] arrayOfToneNames = new string[] {"A", "A#/Bb", "B", "C", "C#/Db", "D", "D#/Eb", "E", "F", "F#/Gb", "G", "G#/Ab"};
            //for (int toneIndex = 0; toneIndex < 12; toneIndex++)
            //{
            //    majorSimilarities.Add(arrayOfToneNames[toneIndex], VectorOperations.CosineSimilarity(majorProfile.CreateProfileForTonica(toneIndex), chromaVectorValues, 12)); //Key = tone name, value = Cosine similarity result 
            //    minorSimilarities.Add(arrayOfToneNames[toneIndex], VectorOperations.CosineSimilarity(minorProfile.CreateProfileForTonica(toneIndex), chromaVectorValues, 12));
            //}
            //string majorkey = majorSimilarities.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            //string minorkey = minorSimilarities.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            //string key = (majorSimilarities[majorkey] < minorSimilarities[minorkey]) ? minorkey + " mol" : majorkey + " dur";
        }

        private string FormatToMusicNotation(int key)
        {
            Dictionary<int, string> musicNotationDictionary = new Dictionary<int, string>()
            {
                {0, "A major" },
                {1, "Bb major" },
                {2, "B major" },
                {3, "C major" },
                {4, "Db major" },
                {5, "D major" },
                {6, "Eb major" },
                {7, "E major" },
                {8, "F major" },
                {9, "Gb major" },
                {10, "G major" },
                {11, "Ab major" },

                {12, "A minor" },
                {13, "Bb minor" },
                {14, "B minor" },
                {15, "C minor" },
                {16, "Db minor" },
                {17, "D minor" },
                {18, "Eb minor" },
                {19, "E minor" },
                {20, "F minor" },
                {21, "Gb minor" },
                {22, "G minor" },
                {23, "Ab minor" },
            };
            return musicNotationDictionary[key];
        }

        private string FormatToCamelotNotation(int key)
        {
            Dictionary<int, string> camelotNotationDictionary = new Dictionary<int, string>()
            {
                {0, "11B" },
                {1, "6B" },
                {2, "1B" },
                {3, "8B" },
                {4, "3B" },
                {5, "10B" },
                {6, "5B" },
                {7, "12B" },
                {8, "7B" },
                {9, "2B" },
                {10, "9B" },
                {11, "4B" },

                {12, "8A" },
                {13, "3A" },
                {14, "10A" },
                {15, "5A" },
                {16, "12A" },
                {17, "7A" },
                {18, "2A" },
                {19, "9A" },
                {20, "4A" },
                {21, "11A" },
                {22, "6A" },
                {23, "1A" },
            };
            return camelotNotationDictionary[key];
        }
    }
}
