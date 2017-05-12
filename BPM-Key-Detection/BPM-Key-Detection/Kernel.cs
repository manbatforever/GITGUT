using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BPM_Key_Detection
{
    static class Kernel
    {
        public static Complex[][] GetSpectralKernel(SpectralKernelStruct inputKernel)
        {
            Complex[][] output = new Complex[inputKernel.BinsTotal][];
            for (int kernelNumber = 0; kernelNumber < inputKernel.BinsTotal; kernelNumber++)
            {
                output[kernelNumber] = GetKernel(inputKernel, kernelNumber);
            }
            return output;
        }

        private static Complex[] GetKernel(SpectralKernelStruct Kernel, int kernelNumber)
        {
            Complex[] output = new Complex[Kernel.FrameSize];
            for (int k = 0; k < Kernel.FrameSize; k++)
            {
                output[k] = Placeholder(Kernel, k, kernelNumber);
            }
            return output;
        }

        private static Complex Placeholder(SpectralKernelStruct Kernel, int k, int kernelNumber)
        {
            Complex output = new Complex();
            foreach (Complex number in placeholder2(Kernel, k, kernelNumber))
            {
                output += number;
            }
            return output;
        }

        private static Complex[] placeholder2(SpectralKernelStruct Kernel, double k, double kernelNumber)
        {
            Complex[] output = new Complex[Kernel.FrameSize];
            for (double n = 0; n < Kernel.FrameSize; n++)
            {
                Complex complexFactor1 = new Complex(0, CenterFrequency(Kernel, kernelNumber) * (n - Kernel.FrameSize / 2d));
                Complex complexFactor2 = new Complex(0, -2d * Math.PI * k * n / Kernel.FrameSize);
                double hamming = HammingWindow(Kernel, n, kernelNumber);
                output[(int)n] = hamming * Complex.Exp(complexFactor1) * Complex.Exp(complexFactor2);
            }
            return output;
        }

        private static double CenterFrequency(SpectralKernelStruct Kernel, double kernelNumber)
        {
            return Math.Pow(2d, kernelNumber / Kernel.BinsPerOctave) * Kernel.BaseFrequency;
        }

        private static double HammingWindow(SpectralKernelStruct Kernel, double n, double kernelNumber)
        {
            double alpha = 25d / 46d;
            return alpha - (1 - alpha) * Math.Cos(2d * Math.PI * n / WindowLength(Kernel, kernelNumber));
        }

        private static double WindowLength(SpectralKernelStruct Kernel, double kernelNumber)
        {
            double Q = 1d / (Math.Pow(2d, 1d / Kernel.BinsPerOctave) - 1d);
            return Q * (double)Kernel.Samplerate / CenterFrequency(Kernel, kernelNumber);
        }
    }
}
