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
        protected double[] _toneOfInterest;

        public Kernel(double samplerate)
        {
            _samplerate = samplerate;
            _toneOfInterest = ToneOfInterest();
        }

        private double[] ToneOfInterest()
        {
            double[] tonesOfInterest = new double[Transformations.TonesTotal];
            for (int tone = 0; tone < Transformations.TonesTotal; tone++)
            {
                tonesOfInterest[tone] = Math.Pow(Math.Pow(2d, 1d / Transformations.TonesPerOctave), tone) * Transformations.MinimumFrequency;
            }
            return tonesOfInterest;
        }
    }
}
