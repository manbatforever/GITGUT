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
        private int[] _lLimitValues;
        private int[] _rLimitValues;

        public IbrahimSpectralKernel(double samplerate, int tonesPerOctave, int tonesTotal, int samplesPerFrame, double minimumFrequency) :
            base(samplerate, tonesPerOctave, tonesTotal, samplesPerFrame, minimumFrequency)
        {
            _lLimitValues = new int[_tonesTotal];
            _rLimitValues = new int[_tonesTotal];
            _Q = 0.8d * (Math.Pow(2d, 1d / _tonesPerOctave) - 1d);
            _spectralKernelBins = GetAllSpectralKernels();
        }



        private double[][] GetAllSpectralKernels()
        {
            double[][] allSpectralKernel = new double[_tonesTotal][];
            for (int kernel = 0; kernel < _tonesTotal; kernel++)
            {
                allSpectralKernel[kernel] = GetSingleSpectralKernel(kernel);
            }
            return allSpectralKernel;
        }

        private double[] GetSingleSpectralKernel(int kernelNumber)
        {
            int leftLimit = (int)Math.Ceiling((1d - (_Q / 2d)) * WindowLengthHelpingFunction(kernelNumber));
            int rightLimit = (int)Math.Floor((1d + (_Q / 2d)) * WindowLengthHelpingFunction(kernelNumber));
            _lLimitValues[kernelNumber] = leftLimit;
            _rLimitValues[kernelNumber] = rightLimit;
            Window ibrahimWindow = new IbrahimWindow(leftLimit, rightLimit);
            double[] spectralKernel = new double[_samplesPerFrame];
            for (int b = leftLimit; b <= rightLimit; b++) //Ibrahim Equation (2.7)
            {
                double spectralWindowSum = 0;
                for (int i = leftLimit; i <= rightLimit; i++) //Ibrahim Equation (2.7)
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
            return (_toneOfInterest[k] * _samplesPerFrame) / _samplerate;
        }

        public int[] LLimitValues { get => _lLimitValues; }
        public int[] RLimitValues { get => _rLimitValues; }
    }
}
