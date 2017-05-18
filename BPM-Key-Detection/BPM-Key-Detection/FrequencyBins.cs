using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{   //WARNING: This class is outdated and not used (or compiled) as of this version.
    //Object: Represents the direct output of a transformation
    class FrequencyBins
    {
        private double[] _binValues;
        private int _numOfBins;

        public FrequencyBins(double[] binValues)
        {
            _binValues = binValues;
            _numOfBins = binValues.Length;
        }

        public FrequencyBins(MathNet.Numerics.Complex32[] binValues)
        {
            _numOfBins = binValues.Length;
            _binValues = new double[_numOfBins];
            for (int i = 0; i < _numOfBins; i++)
            {
                _binValues[i] = binValues[i].Magnitude;
            }
        }

        public double[] BinValues { get => _binValues; }
        public int NumOfBins { get => _numOfBins; }

        public override string ToString()
        {
            string toString = "";
            foreach (double bin in _binValues)
            {
                toString += bin + ";";
            }
            return toString;
        }
    }
}
