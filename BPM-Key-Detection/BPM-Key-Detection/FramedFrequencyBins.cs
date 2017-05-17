using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Represents an array of frames containing frequency bins.
    public class FramedFrequencyBins
    {
        private double[][] _framedFrequencyBinValues;
        private int _numOfFrames;

        public FramedFrequencyBins(double[][] framedFrequencyBinValues)
        {
            _framedFrequencyBinValues = framedFrequencyBinValues;
            _numOfFrames = framedFrequencyBinValues.Length;
        }

        public double[][] FramedFrequencyBinValues { get => _framedFrequencyBinValues; }
        public int NumOfFrames { get => _numOfFrames; }
    }
}
