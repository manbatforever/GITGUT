using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection.Kernels
{
    class BrownPucketteSpectralKernel : SpectralKernel
    {
        public BrownPucketteSpectralKernel(double samplerate, int kernelNumber) : 
            base(samplerate, kernelNumber)
        {
            _spectralKernelBins = Transformations.FFT(new TemporalKernel(samplerate, kernelNumber).TemporalKernelSamples);
        }
    }
}
