using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public abstract class SpectralKernel : Kernel
    {
        protected double[][] _spectralKernelBins;

        public double[][] SpectralKernelBins { get => _spectralKernelBins; }

        public SpectralKernel(double samplerate) : 
            base(samplerate)
        {
        }

        public override string ToString()
        {
            return _spectralKernelBins.ToString();
        }
    }
}
