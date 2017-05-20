using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace BPM_Key_Detection
{   //WARNING: This class is outdated and not used (or compiled) as of this version.
    //Object: Represents the direct output of a transformation
    internal class FrequencyBins
    {
        private Complex[] _binValues;
        private int _numOfBins;

        public FrequencyBins(Complex[] binValues)
        {
            _binValues = binValues;
            _numOfBins = binValues.Length;
        }

        public Complex[] BinValues { get => _binValues; }
        public int NumOfBins { get => _numOfBins; }

        public override string ToString()
        {
            string toString = "";
            foreach (Complex bin in _binValues)
            {
                toString += bin + ";";
            }
            return toString;
        }
    }
}
