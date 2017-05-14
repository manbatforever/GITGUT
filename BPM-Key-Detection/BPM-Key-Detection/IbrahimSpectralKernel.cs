using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class IbrahimSpectralKernel
    {
        private double _Q;
        private double _samplerate;
        private double _binsTotal;
        private double _samplesPerFrame;

        private double[] _tonesOfInterest;

        public IbrahimSpectralKernel(double samplerate, double baseFrequency, double binsPerOctave, double octaves, double samplesPerFrame)
        {
            _samplerate = samplerate;
            _binsTotal = octaves * binsPerOctave;
            _samplesPerFrame = samplesPerFrame;
            _Q = 0.8d * (Math.Pow(2d, 1d / binsPerOctave) - 1d);
            _tonesOfInterest = TonesOfInterest(binsPerOctave, baseFrequency);
        }

        public double[][] GetAllSpectralKernels()
        {
            double[][] allSpectralKernels = new double[(int)_binsTotal][];
            for (int k = 0; k < _binsTotal; k++)
            {
                allSpectralKernels[k] = GetSingleSpectralKernel(k);
            }
            return allSpectralKernels;
        }

        private double[] GetSingleSpectralKernel(int k)
        {
            int lLimit = (int)Math.Ceiling(LeftWindowLimit(k));
            int rLimit = (int)Math.Floor(RightWindowLimit(k));
            double[] spectralKernel = new double[(int)_samplesPerFrame];
            for (int b = lLimit; b <= rLimit; b++)
            {
                double spectralWindowSum = 0;
                for (double i = lLimit; i <= rLimit; i++)
                {
                    spectralWindowSum += SpectralWindowFunction(i, k, lLimit, rLimit);
                }
                if (spectralWindowSum != 0 && rLimit != lLimit)
                    spectralKernel[b] = SpectralWindowFunction(b, k, lLimit, rLimit) * _tonesOfInterest[k] / spectralWindowSum;
            }
            return spectralKernel;
        }

        private double SpectralWindowFunction(double x, int k, double lLimit, double rLimit)
        {
            return 1d - Math.Cos(2d * Math.PI * ((x - lLimit) / (rLimit - lLimit)));
        }

        private double LeftWindowLimit(int k)
        {
            return (1d - (_Q / 2d)) * WindowLengthHelpingFunction(k);
        }

        private double RightWindowLimit(int k)
        {
            return (1d + (_Q / 2d)) * WindowLengthHelpingFunction(k);
        }

        private double WindowLengthHelpingFunction(int k)
        {
            return (_tonesOfInterest[k] * _samplesPerFrame) / _samplerate;
        }

        private double[] TonesOfInterest(double binsPerOctave, double baseFrequency)
        {
            double[] tonesOfInterest = new double[(int)_binsTotal];
            for (int k = 0; k < _binsTotal; k++)
            {
                tonesOfInterest[k] = Math.Pow(Math.Pow(2d, 1d / binsPerOctave), k) * baseFrequency;
            }
            return tonesOfInterest;
        }
    }
}
