using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection.Kernels
{   //WARNING: This class is outdated and not used (or compiled) as of this version.
    //Object: A spectral kernel corresponding to the one from Brown and Puckette (1992)
    class BrownPucketteSpectralKernel : SpectralKernel
    {
        public BrownPucketteSpectralKernel(double samplerate, int kernelNumber) : 
            base(samplerate, kernelNumber)
        {
            _spectralKernelBins = Transformations.FFT(new TemporalKernel(samplerate, kernelNumber).TemporalKernelSamples); //This spectral kernel is simply FFT applied on a temporal kernel
        }
    }
}
