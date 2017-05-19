using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //An abstract data type used to represent music file data and kernel data
    internal abstract class Samples
    {
        protected double[] _sampleArray;
        protected int _numOfSamples;

        public double[] SampleArray { get => _sampleArray; }
        public int NumOfSamples { get => _numOfSamples; }

        public override string ToString()
        {
            string toString = "";
            foreach (double sample in _sampleArray)
            {
                toString += sample + ";";
            }
            return toString;
        }
    }
}