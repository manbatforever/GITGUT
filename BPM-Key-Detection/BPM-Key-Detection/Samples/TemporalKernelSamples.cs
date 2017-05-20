using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{   //WARNING: This class is outdated and not used (or compiled) as of this version.
    //Object: A type of samples that a temporal kernel contains
    internal class TemporalKernelSamples : Samples
    {
        public TemporalKernelSamples(double[] sampleValues)
        {
            _sampleArray = sampleValues;
            _numOfSamples = sampleValues.Length;
        }
    }
}
