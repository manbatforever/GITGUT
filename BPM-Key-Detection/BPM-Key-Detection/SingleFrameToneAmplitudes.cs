using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public class SingleFrameToneAmplitudes
    {
        private double[] _toneAmplitudeValues;

        public double[] ToneAmplitudeValues { get => _toneAmplitudeValues; }

        public SingleFrameToneAmplitudes(FrequencyBins musicFileFFT, SpectralKernel[] spectralKernels)
        {
            _toneAmplitudeValues = new double[Transformations.TonesTotal];
            CalculateSingleToneAmplitude(musicFileFFT, spectralKernels);
        }

        private void CalculateSingleToneAmplitude(FrequencyBins musicFileFFT, SpectralKernel[] spectralKernels)
        {
            for (int tone = 0; tone < Transformations.TonesTotal; tone++)
            {
                double singleToneAmplitude = 0;
                for (int bin = 0; bin < Transformations.SamplesPerFrame; bin++)
                {
                    singleToneAmplitude += musicFileFFT.BinValues[bin] * spectralKernels[tone].SpectralKernelBins.BinValues[bin]; // Brown & Puckette Equation (5)
                }
                _toneAmplitudeValues[tone] = singleToneAmplitude;
            }
        }
    }
}
