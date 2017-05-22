using System.Collections.Generic;
using System.Linq;

namespace BPM_Key_Detection
{
    internal class KeyEstimator
    {
        private string _camelotNotation;
        private string _musicNotation;
        
        public KeyEstimator(MusicFileSamples musicFileSamples)
        {
            CQT cqt = new CQT(musicFileSamples);
            FramedToneAmplitudes allToneAmplitudes = cqt.FramedToneAmplitudes;
            ChromaVector chromaVector = new ChromaVector(allToneAmplitudes, cqt.TonesPerOctave, cqt.NumOfOctaves);
            int key = CalculateKey(chromaVector);
            _camelotNotation = FormatToCamelotNotation(key);
            _musicNotation = FormatToMusicNotation(key);
        }

        private int CalculateKey(ChromaVector chromaVector)
        {
            KeyProfile majorProfile = new MajorProfile();
            KeyProfile minorProfile = new MinorProfile();
            double[] allSimilarities = new double[24];
            for (int i = 0; i < 12; i++)
            {
                allSimilarities[i] = chromaVector.CosineSimilarity(majorProfile.CreateProfileForTonica(i));
                allSimilarities[i + 12] = chromaVector.CosineSimilarity(minorProfile.CreateProfileForTonica(i));
            }
            return allSimilarities.ToList().IndexOf(allSimilarities.Max());
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

        public string CamelotNotation { get => _camelotNotation; }
        public string MusicNotation { get => _musicNotation; }
    }
}
