using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection.TestClasses
{
    class MusicFile
    {
        private TagLib.File _file;
        private string _filePath;
        private string _fileName;

        public MusicFile(string filePath)
        {
            try
            {
                _file = TagLib.File.Create(filePath);
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("CorruptedMetadataException");
            }

            _filePath = filePath;
            _fileName = filePath.Substring(FilePath.LastIndexOf("\\") + 1);
        }

        public Samples GetRawSamples()
        {
            NAudio.Wave.AudioFileReader file = new NAudio.Wave.AudioFileReader(_filePath);
            int samplerate = file.WaveFormat.SampleRate;
            int channels = file.WaveFormat.Channels;
            int lengthFloat = (int)file.Length / 4;
            float[] buffer = new float[lengthFloat];
            file.Read(buffer, 0, lengthFloat);
            file.Close();
            return new Samples(buffer, lengthFloat, samplerate, channels);
        }

        public string FilePath { get => _filePath; }
        public string FileName { get => _fileName; }
        public string Titel { get => _file.Tag.Title; }
        public string Album { get => _file.Tag.Album; }
        public string Artists { get => _file.Tag.JoinedPerformers; }
        public string Comment { get => _file.Tag.Comment; }
        public string Key
        {
            get
            {
                TagLib.Id3v2.Tag fileTag = (TagLib.Id3v2.Tag)_file.GetTag(TagLib.TagTypes.Id3v2, false);
                TagLib.ByteVector ID = "TKEY";
                return TagLib.Id3v2.TextInformationFrame.Get(fileTag, ID, true).ToString();
            }
            set
            {
                string[] key = { value };
                TagLib.Id3v2.Tag fileTag = (TagLib.Id3v2.Tag)_file.GetTag(TagLib.TagTypes.Id3v2, false);
                TagLib.ByteVector ID = "TKEY";
                TagLib.Id3v2.TextInformationFrame metadataKey = TagLib.Id3v2.TextInformationFrame.Get(fileTag, ID, true);
                metadataKey.Text = key;
            }
        }
        public uint Bpm { get => _file.Tag.BeatsPerMinute; set => _file.Tag.BeatsPerMinute = value; }
    }
}
