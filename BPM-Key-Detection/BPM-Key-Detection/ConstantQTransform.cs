using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace testapp
{
    static class ConstantQTransform
    {
        public static double[][] GetCQT(Complex[][] FFTsamples, SpectralKernelStruct KernelSpecifications)
        {
            int Frames = FFTsamples.Length;
            int BinsTotal = KernelSpecifications.BinsTotal;
            int FrameSize = KernelSpecifications.FrameSize;
            Complex[][] Kernel = KernelDatabase.GetKernel(KernelSpecifications);

            double[][] output = new double[Frames][];
            for (int frame = 0; frame < Frames; frame++)
            {
                output[frame] = new double[BinsTotal];
                for (int bin = 0; bin < BinsTotal; bin++)
                {
                    output[frame][bin] = BinAmplitude(Kernel[bin], FFTsamples[frame], FrameSize);
                }
            }
            return output;
        }
        
        private static double BinAmplitude(Complex[] KernelBin, Complex[] FFTSamples, int FrameSize)
        {
            Complex temp = new Complex(0,0);
            for (int i = 0; i < FrameSize; i++)
            {
                temp += KernelBin[i] * FFTSamples[i];
            }
            return (temp / FrameSize).Magnitude;
        }
    }
}
