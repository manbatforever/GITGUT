using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace BPM_Key_Detection
{
    //Object: Represents the direct output of a transformation
    internal class FrequencyBins
    {
        private Complex[] _binValues;

        public FrequencyBins(Complex[] binValues)
        {
            _binValues = binValues;
        }

        public Complex[] BinValues { get => _binValues; }

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
