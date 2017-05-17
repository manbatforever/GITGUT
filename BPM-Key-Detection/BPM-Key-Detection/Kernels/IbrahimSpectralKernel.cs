using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: A spectral kernel corresponding to the one from Ibrahim (2013)
    class IbrahimSpectralKernel : SpectralKernel
    {
        private double[] _lLimitValues;
        private double[] _rLimitValues;

        public IbrahimSpectralKernel(double samplerate) : 
            base(samplerate)
        {
            _lLimitValues = new double[Transformations.TonesTotal];
            _rLimitValues = new double[Transformations.TonesTotal];
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

        private double[] GetSingleSpectralKernel(int kernelNumber)
        {
            double leftLimit = Math.Ceiling((1d - (_Q / 2d)) * WindowLengthHelpingFunction(kernelNumber));
            double rightLimit = Math.Floor((1d + (_Q / 2d)) * WindowLengthHelpingFunction(kernelNumber));
            _lLimitValues[kernelNumber] = leftLimit;
            _rLimitValues[kernelNumber] = rightLimit;
            Window ibrahimWindow = new IbrahimWindow(leftLimit, rightLimit);
            double[] spectralKernel = new double[Transformations.SamplesPerFrame];
            for (int b = (int)leftLimit; b <= rightLimit; b++) //Ibrahim Equation (2.7)
            {
                double spectralWindowSum = 0;
                for (int i = (int)leftLimit; i <= rightLimit; i++) //Ibrahim Equation (2.7)
                {
                    spectralWindowSum += ibrahimWindow.WindowArray[i];
                }
                if (spectralWindowSum != 0 && rightLimit != leftLimit)
                    spectralKernel[b] = ibrahimWindow.WindowArray[b] * _toneOfInterest[kernelNumber] / spectralWindowSum;
            }
            return spectralKernel;
        }
        private double WindowLengthHelpingFunction(int k)
        {
            return (_toneOfInterest[k] * Transformations.SamplesPerFrame) / _samplerate;
        }

        public double[] LLimitValues { get => _lLimitValues; }
        public double[] RLimitValues { get => _rLimitValues; }
    }
}
