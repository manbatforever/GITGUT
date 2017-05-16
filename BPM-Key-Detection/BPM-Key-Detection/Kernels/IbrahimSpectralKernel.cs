using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class IbrahimSpectralKernel : SpectralKernel
    {
        public IbrahimSpectralKernel(double samplerate, int kernelNumber) : 
            base(samplerate, kernelNumber)
        {
            _Q = 0.8d * (Math.Pow(2d, 1d / Transformations.TonesPerOctave) - 1d);
            _spectralKernelBins = new FrequencyBins(GetSingleSpectralKernel(kernelNumber));
        }

        private double[] GetSingleSpectralKernel(int k)
        {
            double leftLimit = Math.Ceiling((1d - (_Q / 2d)) * WindowLengthHelpingFunction(k));
            double rightLimit = Math.Floor((1d + (_Q / 2d)) * WindowLengthHelpingFunction(k));
            double[] spectralKernel = new double[Transformations.SamplesPerFrame];
            for (int b = (int)leftLimit; b <= rightLimit; b++)
            {
                double spectralWindowSum = 0;
                for (double i = leftLimit; i <= rightLimit; i++)
                {
                    spectralWindowSum += SpectralWindowFunction(i, k, leftLimit, rightLimit);
                }
                if (spectralWindowSum != 0 && rightLimit != leftLimit)
                    spectralKernel[b] = SpectralWindowFunction(b, k, leftLimit, rightLimit) * _toneOfInterest / spectralWindowSum;
            }
            return spectralKernel;
        }

        private double SpectralWindowFunction(double x, int k, double lLimit, double rLimit)
        {
            return 1d - Math.Cos(2d * Math.PI * ((x - lLimit) / (rLimit - lLimit)));
        }

        private double WindowLengthHelpingFunction(int k)
        {
            return (_toneOfInterest * Transformations.SamplesPerFrame) / _samplerate;
        }
    }
}
