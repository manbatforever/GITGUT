using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public class FramedToneAmplitudes
    {
        private double[][] _toneAmplitudeValues;

        public double[][] ToneAmplitudeValues { get => _toneAmplitudeValues; }

        public FramedToneAmplitudes(FramedFrequencyBins musicFileFFT, SpectralKernel spectralKernels)
        {
            _toneAmplitudeValues = CalculateSingleToneAmplitude(musicFileFFT, spectralKernels);
        }

        private double[][] CalculateSingleToneAmplitude(FramedFrequencyBins musicFileFFT, SpectralKernel spectralKernels)
        {
            double[][] framedToneAmplitudes = new double[musicFileFFT.NumOfFrames][];
            for (int frame = 0; frame < musicFileFFT.NumOfFrames; frame++)
            {
                framedToneAmplitudes[frame] = new double[Transformations.TonesTotal];
                for (int tone = 0; tone < Transformations.TonesTotal; tone++)
                {
                    for (int bin = 0; bin < Transformations.SamplesPerFrame; bin++)
                    {
                        framedToneAmplitudes[frame][tone] += musicFileFFT.FramedFrequencyBinValues[frame][bin] * spectralKernels.SpectralKernelBins[tone][bin]; // Brown & Puckette Equation (5)
                    }
                }
            }
            return framedToneAmplitudes;
        }
    }
}
