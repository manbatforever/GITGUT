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

        public Samples GetRawSamples()
        {
            AudioFileReader file = new AudioFileReader(Filepath);
            int samplerate = file.WaveFormat.SampleRate;
            int channels = file.WaveFormat.Channels;
            int lengthFloat = (int)file.Length / 4;
            float[] buffer = new float[lengthFloat];
            file.Read(buffer, 0, lengthFloat);
            file.Close();
            return new Samples(buffer, lengthFloat, samplerate, channels);
        }

        public string FileName { get { return _fileName; } }
        public string Titel { get { return _titel; } }
        public string Album { get { return _album; } }
        public string Artists { get { return _artists; } }
        public string Filepath { get { return _filepath; } }
        public string Key { get { return _key; } }
        public string Comment { get { return _comment; } }
        public uint Bpm { get { return _bpm; } }
        public bool BadFile { get { return _badFile; } }
    }
}
