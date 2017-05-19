using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //A abstract data type for CQT calculations
    internal abstract class Kernel
    {
        protected double _Q;
        protected double _samplerate;
        protected double[] _toneOfInterest;
        protected double _minimumFrequency;
        protected int _tonesPerOctave;
        protected int _tonesTotal;
        protected int _samplesPerFrame;
       

        public Kernel(double samplerate, int tonesPerOctave,  int tonesTotal, int samplesPerFrame, double minimumFrequency)
        {
            _samplerate = samplerate;
            _tonesPerOctave = tonesPerOctave;
            _tonesTotal = tonesTotal;
            _samplesPerFrame = samplesPerFrame;
            _minimumFrequency = minimumFrequency;
            _toneOfInterest = CalculateToneOfInterest();
        }

        private double[] CalculateToneOfInterest()
        {
            double[] tonesOfInterest = new double[_tonesTotal];
            for (int tone = 0; tone < _tonesTotal; tone++)
            {
                tonesOfInterest[tone] = Math.Pow(Math.Pow(2d, 1d / _tonesPerOctave), tone) * _minimumFrequency;
            }
            return tonesOfInterest;
        }
    }
}
