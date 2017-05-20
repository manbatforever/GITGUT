using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using MathNet.Numerics;

namespace BPM_Key_Detection.Kernels
{   //WARNING: This class is outdated and not used (or compiled) as of this version.
    //Object: A spectral kernel corresponding to the one from Brown and Puckette (1992)
    class BrownPucketteSpectralKernel : SpectralKernel
    {
        private int _kernelNumber;

        public BrownPucketteSpectralKernel(double samplerate, int tonesPerOctave, int tonesTotal, int samplesPerFrame, double minimumFrequency, int kernelNumber) : 
            base(samplerate, tonesPerOctave, tonesTotal, samplesPerFrame, minimumFrequency)
        {
            _kernelNumber = kernelNumber;
            FFT fft = new FFT();
            _spectralKernelBins = fft.SingleFrameFFT(new TemporalKernel(_samplerate, _tonesPerOctave, _tonesTotal, _samplesPerFrame, _minimumFrequency, _kernelNumber).TemporalKernelSamples.SampleArray).Magnitude; //This spectral kernel is simply FFT applied on a temporal kernel
        }
    }
}
