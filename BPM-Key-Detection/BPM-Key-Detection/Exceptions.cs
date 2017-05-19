using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    internal class SongNotLongEnoughException : Exception
    {
        public SongNotLongEnoughException() : base("The song does not have enough samples to be analyzed.") { }
        public SongNotLongEnoughException(string songName) : base($"The song {songName} does not have enough samples to be analyzed.") { }
    }
    internal class SongIsNotStereoException : Exception
    {
        public SongIsNotStereoException() : base("The song is not stereo, and can not be analysed.") { }
        public SongIsNotStereoException(string songName) : base($"The song {songName} is not stereo, and can not be analysed.") { }
    }
}
