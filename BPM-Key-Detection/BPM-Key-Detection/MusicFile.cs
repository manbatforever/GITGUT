using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace testapp
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
        private float[] _samples;
        private bool _badFile;

        public MusicFile(string FileName)
        {
            _badFile = false;
            GetMetadata(FileName);
        }

        private void GetMetadata(string FileName)
        {
            _filepath = FileName;
            _fileName = FileName.Substring(FileName.LastIndexOf("\\") + 1);
            try
            {
                File file = File.Create(FileName);

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
        public string FileName { get { return _fileName; } }
        public string Titel { get { return _titel; } }
        public string Album { get { return _album; } }
        public string Artists { get { return _artists; } }
        public string Filepath { get { return _filepath; } }
        public string Key { get { return _key; } }
        public string Comment { get { return _comment; } }
        public uint Bpm { get { return _bpm; } }
        public float[] Samples { get { return _samples; } set { _samples = value; } }
        public bool BadFile { get { return _badFile; } }
    }
}
