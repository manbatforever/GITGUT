using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace testapp
{
    class Kernel
    {
        private SpectralKernelStruct KernelSpecs;
        private double Q;
        private double NLength;

        public Kernel(SpectralKernelStruct KernelSpecs)
        {
            this.KernelSpecs = KernelSpecs;
            Q = 1 / (Math.Pow(2d, 1 / (double)KernelSpecs.BinsPerOctave) - 1);
            NLength = N(0); 
        }

        public Complex[][] AllBinKernels()
        {
            Complex[][] output = new Complex[KernelSpecs.BinsTotal][];
            for (int k_cq = 0; k_cq < KernelSpecs.BinsTotal; k_cq++)
            {
                output[k_cq] = SingleBinKernel(k_cq);
            }
            return output;
        }

        public Complex[] SingleBinKernel(double k_cq)
        {
            Complex[] output = new Complex[KernelSpecs.FrameSize];
            for (int k = 0; k < KernelSpecs.FrameSize; k++)
            {
                output[k] = Asdf(k, k_cq);
            }
            return output;
        }

        /// <summary>
        /// Udregner enkelte elementer i temporal kerne
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private Complex Asdf(double n, double k_cq)
        {
            Complex c1 = Complex.Exp(new Complex(0, 2d * Math.PI * n * Q / N(k_cq)));
            return Hamming(n, k_cq) * c1;
        }

        //private Complex K(double k, double k_cq)
        //{
        //    Complex output = new Complex();
        //    for (int n = 0; n < KernelSpecs.FrameSize; n++)
        //    {
        //        Complex c1 = Complex.Exp(new Complex(0, CenterFrequency(k_cq) * (n - (double)KernelSpecs.FrameSize / 2d)));
        //        Complex c2 = Complex.Exp(new Complex(0, -2d * Math.PI * k * n / (double)KernelSpecs.FrameSize));
        //        double hamming = Hamming(n - (((double)KernelSpecs.FrameSize) / 2d) - (N(k_cq) / 2d), k_cq);
        //        output += hamming * c1 * c2;
        //    }
        //    return output;
        //}

        private double Hamming(double n, double k_cq)
        {
            double alpha = 25d / 46d;
            return alpha - (1d - alpha) * Math.Cos(2d * Math.PI * n / N(k_cq));
        }

        private double N(double k_cq)
        {
            return Q * (double)KernelSpecs.Samplerate / CenterFrequency(k_cq);
        }

        private double CenterFrequency(double k_cq)
        {
            return Math.Pow(Math.Pow(2d, 1d / (double)KernelSpecs.BinsPerOctave), k_cq) * (double)KernelSpecs.BaseFrequency;
        }


        //public static Complex[][] GetSpectralKernel(SpectralKernelStruct inputKernel)
        //{
        //    Complex[][] output = new Complex[inputKernel.BinsTotal][];
        //    for (int kernelNumber = 0; kernelNumber < inputKernel.BinsTotal; kernelNumber++)
        //    {
        //        output[kernelNumber] = GetKernel(inputKernel, kernelNumber);
        //    }
        //    return output;
        //}

        //private static Complex[] GetKernel(SpectralKernelStruct Kernel, int kernelNumber)
        //{
        //    Complex[] output = new Complex[Kernel.FrameSize];
        //    for (int k = 0; k < Kernel.FrameSize; k++)
        //    {
        //        output[k] = Placeholder(Kernel, k, kernelNumber);
        //    }
        //    return output;
        //}

        //private static Complex Placeholder(SpectralKernelStruct Kernel, int k, int kernelNumber)
        //{
        //    Complex output = new Complex();
        //    foreach (Complex number in placeholder2(Kernel, k, kernelNumber))
        //    {
        //        output += number;
        //    }
        //    return output;
        //}

        //private static Complex[] placeholder2(SpectralKernelStruct Kernel, double k, double kernelNumber)
        //{
        //    Complex[] output = new Complex[Kernel.FrameSize];
        //    for (double n = 0; n < Kernel.FrameSize; n++)
        //    {
        //        Complex complexFactor1 = new Complex(0, CenterFrequency(Kernel, kernelNumber) * (n - Kernel.FrameSize / 2d));
        //        Complex complexFactor2 = new Complex(0, -2d * Math.PI * k * n / Kernel.FrameSize);
        //        double hamming = HammingWindow(Kernel, n, kernelNumber);
        //        output[(int)n] = hamming * Complex.Exp(complexFactor1) * Complex.Exp(complexFactor2);
        //    }
        //    return output;
        //}

        //private static double CenterFrequency(SpectralKernelStruct Kernel, double kernelNumber)
        //{
        //    return Math.Pow(2d, kernelNumber / Kernel.BinsPerOctave) * Kernel.BaseFrequency;
        //}

        //private static double HammingWindow(SpectralKernelStruct Kernel, double n, double kernelNumber)
        //{
        //    double alpha = 25d / 46d;
        //    return alpha - (1 - alpha) * Math.Cos(2d * Math.PI * n / WindowLength(Kernel, kernelNumber));
        //}

        //private static double WindowLength(SpectralKernelStruct Kernel, double kernelNumber)
        //{
        //    double Q = 1d / (Math.Pow(2d, 1d / Kernel.BinsPerOctave) - 1d);
        //    return Q * (double)Kernel.Samplerate / CenterFrequency(Kernel, kernelNumber);
        //}
    }
}
