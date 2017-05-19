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
        private uint _estimatedBPM;
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

        public void EstimateKey(MusicFileSamples musicFileSamples)
        {
            KeyEstimator keyEstimator = new KeyEstimator(musicFileSamples);
            _camelotNotation = keyEstimator.CamelotNotation;
            _musicNotation = keyEstimator.MusicNotation;
        }


        public void EstimateBPM(MusicFileSamples musicFileSamples)
        {
            BPMEstimator BPMEstimator = new BPMEstimator(musicFileSamples.Samplerate, musicFileSamples);
            _estimatedBPM = BPMEstimator.EstimatedBPM;
        }

       

        public void WriteMetadata(bool writeBPM, bool writeKey)
        {
            File file = File.Create(_filepath);
            TagLib.Id3v2.Tag fileTag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, false);
            if (writeBPM)
            {
                file.Tag.BeatsPerMinute = _estimatedBPM;
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
        public uint EstimatedBPM { get => _estimatedBPM; }
        public uint Bpm { get { return _bpm; } }
        public bool BadFile { get { return _badFile; } set { _badFile = value; } }

    }
}
