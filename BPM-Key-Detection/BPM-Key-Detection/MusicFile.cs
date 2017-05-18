using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using NAudio.Wave;

namespace BPM_Key_Detection
{
    //Object: A data representation of a music file, containing functionality regarding music files as well as information about the specific file at hand.
    class MusicFile
    {
        private string _fileName;
        private string _titel;
        private string _album;
        private string _artists;
        private string _filepath;
        private string _key;
        private string _comment;
        private string _camelotNotation;
        private string _musicNotation;
        private uint _estimatedBpm;
        private uint _bpm;
        private bool _badFile;

        public MusicFile(string FilePath)
        {
            _badFile = false;
            GetMetadata(FilePath);
            _filepath = FilePath;
            _fileName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
        }

        private void GetMetadata(string FilePath)
        {
            try
            {
                File file = File.Create(FilePath);

                TagLib.Id3v2.Tag fileTag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, false);
                _titel = file.Tag.Title; // string
                _album = file.Tag.Album;
                _bpm = file.Tag.BeatsPerMinute; // int
                _artists = file.Tag.JoinedPerformers; //String

                ByteVector tagIDKey = "TKEY";
                _key = TagLib.Id3v2.TextInformationFrame.Get(fileTag, tagIDKey, true).ToString();
                _comment = file.Tag.Comment;
            }
            catch (System.IndexOutOfRangeException)
            {
                _badFile = true;
            }
        }

        public MusicFileSamples GetRawSamples()
        {
            AudioFileReader file = new AudioFileReader(Filepath);
            int samplerate = file.WaveFormat.SampleRate;
            int channels = file.WaveFormat.Channels;
            int lengthFloat = (int)file.Length / 4;
            float[] buffer = new float[lengthFloat];
            file.Read(buffer, 0, lengthFloat);
            file.Close();
            return new MusicFileSamples(buffer, samplerate, channels);
        }

        public void EstimateKey()
        {
            MusicFileSamples musicFileSamples = null;
            try
            {
                musicFileSamples = GetRawSamples();
            }
            catch (InvalidOperationException)
            {
                _badFile = true;
            }
            if (musicFileSamples != null)
            {
                FramedToneAmplitudes allToneAmplitudes = Transformations.CQT(musicFileSamples);
                ChromaVector chromaVector = new ChromaVector(allToneAmplitudes);
                int key = CalculateKey(chromaVector);
                _camelotNotation = FormatToCamelotNotation(key);
                _musicNotation = FormatToMusicNotation(key);
            }
        }

        private int CalculateKey(ChromaVector chromaVector)
        {
            KeyProfile majorProfile = new MajorProfile();
            KeyProfile minorProfile = new MinorProfile();
            double[] allSimilarities = new double[24];
            for (int i = 0; i < 12; i++)
            {
                allSimilarities[i] = VectorOperations.CosineSimilarity(majorProfile.CreateProfileForTonica(i), chromaVector.VectorValues, 12);
                allSimilarities[i + 12] = VectorOperations.CosineSimilarity(minorProfile.CreateProfileForTonica(i), chromaVector.VectorValues, 12);
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

        public void WriteMetadata(bool writeBPM, bool writeKey)
        {
            File file = File.Create(_filepath);
            TagLib.Id3v2.Tag fileTag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, false);
            if (writeBPM)
            {
                file.Tag.BeatsPerMinute = _estimatedBpm;
            }
            if (writeKey)
            {
                ByteVector tagIDKey = "TKEY";
                TagLib.Id3v2.TextInformationFrame metadata = TagLib.Id3v2.TextInformationFrame.Get(fileTag, tagIDKey, true);
                string[] key = { _camelotNotation };
                metadata.Text = key;
            }
            file.Save();
        }



        public string FileName { get { return _fileName; } }
        public string Titel { get { return _titel; } }
        public string Album { get { return _album; } }
        public string Artists { get { return _artists; } }
        public string Filepath { get { return _filepath; } }
        public string Key { get { return _key; } }
        public string Comment { get { return _comment; } }
        public string CamelotNotation { get => _camelotNotation; }
        public string MusicNotation { get => _musicNotation; }
        public uint EstimatedBpm { get => _estimatedBpm; }
        public uint Bpm { get { return _bpm; } }
        public bool BadFile { get { return _badFile; } }

    }
}
