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

        public KeyEstimation(MusicFile musicFile)
        {
            _musicFile = musicFile;
        }

        public void Start()
        {
            SpectralKernel spectralKernel = new SpectralKernel(_musicFile.SampleRate, 440, 12, 6);
        }
    }
}
