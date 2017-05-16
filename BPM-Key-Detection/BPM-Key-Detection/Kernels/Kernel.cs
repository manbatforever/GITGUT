using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public abstract class Kernel
    {
        protected double _Q;
        protected double _samplerate;
        protected double _binsTotal;
        protected double _samplesPerFrame;
        protected double _toneOfInterest;

        public Kernel(double samplerate, int kernelNumber)
        {
            _samplerate = samplerate;
            _toneOfInterest = ToneOfInterest(kernelNumber);
        }

        private double ToneOfInterest(int kernelNumber)
        {
            return Math.Pow(Math.Pow(2d, 1d / Transformations.TonesPerOctave), kernelNumber) * Transformations.MinimumFrequency;
        }
    }
}
