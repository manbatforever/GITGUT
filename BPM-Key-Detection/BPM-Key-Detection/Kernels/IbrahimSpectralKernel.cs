using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class IbrahimSpectralKernel : SpectralKernel
    {
        public IbrahimSpectralKernel(double samplerate) : 
            base(samplerate)
        {
            _Q = 0.8d * (Math.Pow(2d, 1d / Transformations.TonesPerOctave) - 1d);
            _spectralKernelBins = GetAllSpectralKernels();
        }

        private double[][] GetAllSpectralKernels()
        {
            double[][] allSpectralKernel = new double[Transformations.TonesTotal][];
            for (int kernel = 0; kernel < Transformations.TonesTotal; kernel++)
            {
                allSpectralKernel[kernel] = GetSingleSpectralKernel(kernel);
            }
            return allSpectralKernel;
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
                    spectralKernel[b] = SpectralWindowFunction(b, k, leftLimit, rightLimit) * _toneOfInterest[k] / spectralWindowSum;
            }
            return spectralKernel;
        }

        private double SpectralWindowFunction(double x, int k, double lLimit, double rLimit)
        {
            return 1d - Math.Cos(2d * Math.PI * ((x - lLimit) / (rLimit - lLimit)));
        }

        private double WindowLengthHelpingFunction(int k)
        {
            return (_toneOfInterest[k] * Transformations.SamplesPerFrame) / _samplerate;
        }
    }
}
