using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Represents an array of frames containing tone amplitudes.
    internal class FramedToneAmplitudes
    {
        private double[][] _toneAmplitudeValues;
        private int _tonesTotal;
        private int _samplesPerFrame;

        public FramedToneAmplitudes(FramedFrequencyBins musicFileFFT, IbrahimSpectralKernel ibrahimSpectralKernels, int tonesTotal, int samplesPerFrame)
        {
            _tonesTotal = tonesTotal;
            _samplesPerFrame = samplesPerFrame;
            _toneAmplitudeValues = CalculateAllToneAmplitudes(musicFileFFT, ibrahimSpectralKernels);
        }

        private double[][] CalculateAllToneAmplitudes(FramedFrequencyBins musicFileFFT, IbrahimSpectralKernel ibrahimSpectralKernels)
        {
            double[][] framedToneAmplitudes = new double[musicFileFFT.NumOfFrames][];
            for (int frame = 0; frame < musicFileFFT.NumOfFrames; frame++)
            {
                framedToneAmplitudes[frame] = new double[_tonesTotal];
                for (int tone = 0; tone < _tonesTotal; tone++)
                {
                    double temp = 0;
                    for (int bin = (int)ibrahimSpectralKernels.LLimitValues[tone]; bin < ibrahimSpectralKernels.RLimitValues[tone]; bin++)
                    {
                        temp += musicFileFFT.FramedFrequencyBinValues[frame][bin].Magnitude * ibrahimSpectralKernels.SpectralKernelBins[tone][bin]; // Brown & Puckette Equation (5)
                    }
                    framedToneAmplitudes[frame][tone] = temp / _samplesPerFrame;
                }
            }
            return framedToneAmplitudes;
        }

        public double[][] ToneAmplitudeValues { get => _toneAmplitudeValues; }
    }
}
