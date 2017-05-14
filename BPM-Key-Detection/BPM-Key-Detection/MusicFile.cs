using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using NAudio.Wave;

namespace BPM_Key_Detection
{
    public class MusicFile
    {
        private string _fileName;
        private string _titel;
        private string _album;
        private string _artists;
        private string _filepath;
        private string _key;
        private string _comment;
        private uint _bpm;
        private int _sampleRate;
        private int _channels;
        private bool _badFile;

        public MusicFile(string FilePath)
        {
            _badFile = false;
            GetMetadata(FilePath);
            GetSamplerateAndChannels(FilePath);
            
        }

        private void GetMetadata(string FilePath)
        {
            _filepath = FilePath;
            _fileName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
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

        private void GetSamplerateAndChannels(string FilePath)
        {
            AudioFileReader File = new AudioFileReader(FilePath);
            _sampleRate = File.WaveFormat.SampleRate;
            _channels = File.WaveFormat.Channels;
            File.Close();
        }

        public double[] GetRawSamples()
        {
            AudioFileReader File = new AudioFileReader(Filepath);

            int floatLength = (int)(File.Length / 4);
            float[] buffer = new float[floatLength];
            File.Read(buffer, 0, floatLength);

            NAudio.Dsp.BiQuadFilter lowPassFilter = NAudio.Dsp.BiQuadFilter.LowPassFilter(_sampleRate, 2000, 1);
            double[] output = new double[floatLength];
            for (int i = 0; i < floatLength; i++)
            {
                output[i] = lowPassFilter.Transform(buffer[i]);
            }
            return output;
        }

        

        public string FileName { get { return _fileName; } }
        public string Titel { get { return _titel; } }
        public string Album { get { return _album; } }
        public string Artists { get { return _artists; } }
        public string Filepath { get { return _filepath; } }
        public string Key { get { return _key; } }
        public string Comment { get { return _comment; } }
        public uint Bpm { get { return _bpm; } }
        public int Samplerate { get { return _sampleRate; } }
        public int Channels { get { return _channels; } }
        public bool BadFile { get { return _badFile; } }
    }
}
