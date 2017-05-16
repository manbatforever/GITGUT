using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class TemporalKernelSamples : Samples
    {
        public TemporalKernelSamples(double[] sampleValues)
        {
            _sampleArray = sampleValues;
            _numOfSamples = sampleValues.Length;
        }
    }
}
