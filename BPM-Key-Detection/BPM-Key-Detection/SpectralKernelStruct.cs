using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    struct SpectralKernelStruct
    {
        public int Samplerate { get; }
        public double BaseFrequency { get; }
        public int BinsPerOctave { get; }
        public int BinsTotal { get; }
        public int FrameSize { get; }

        public SpectralKernelStruct(int Samplerate, double BaseFrequency, int BinsPerOctave, int Octaves, int FrameSize)
        {
            this.Samplerate = Samplerate;
            this.BaseFrequency = BaseFrequency;
            this.BinsPerOctave = BinsPerOctave;
            this.FrameSize = FrameSize;
            BinsTotal = Octaves * BinsPerOctave;
        }
    }
}
