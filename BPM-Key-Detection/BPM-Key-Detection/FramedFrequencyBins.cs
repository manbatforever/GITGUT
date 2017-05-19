using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Represents an array of frames containing frequency bins.
    internal class FramedFrequencyBins
    {
        private MathNet.Numerics.Complex32[][] _framedFrequencyBinValues;
        private int _numOfFrames;

        public FramedFrequencyBins(MathNet.Numerics.Complex32[][] framedFrequencyBinValues)
        {
            _framedFrequencyBinValues = framedFrequencyBinValues;
            _numOfFrames = framedFrequencyBinValues.Length;
        }

        public MathNet.Numerics.Complex32[][] FramedFrequencyBinValues { get => _framedFrequencyBinValues; }
        public int NumOfFrames { get => _numOfFrames; }
    }
}
