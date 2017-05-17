using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public abstract class SpectralKernel : Kernel
    {
        protected FrequencyBins _spectralKernelBins;

        public FrequencyBins SpectralKernelBins { get => _spectralKernelBins; }

        public SpectralKernel(double samplerate, int kernelNumber) : 
            base(samplerate, kernelNumber)
        {
        }

        public override string ToString()
        {
            return _spectralKernelBins.ToString();
        }
    }
}
