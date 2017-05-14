using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BPM_Key_Detection
{
    static class FastFourierTransform
    {
        public static Complex[][] FFT(double[][] input)
        {
            int length = input.Length;
            Complex[][] output = new Complex[length][];
            for (int i = 0; i < length; i++)
            {
                output[i] = FFT(input[i]);
            }
            return output;
        }

        public static Complex[] FFT(double[] input)
        {
            int length = input.Length;
            MathNet.Numerics.Complex32[] temp = new MathNet.Numerics.Complex32[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = new MathNet.Numerics.Complex32(Convert.ToSingle(input[i]), 0);
            }
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(temp);
            Complex[] output = new Complex[length];
            for (int i = 0; i < length; i++)
            {
                output[i] = new Complex(temp[i].Real, temp[i].Imaginary);
            }
            return output;
        }

        public static double[][] FFTNonComplex(double[][] input)
        {
            int length = input.Length;
            double[][] output = new double[length][];
            for (int i = 0; i < length; i++)
            {
                output[i] = FFTNonComplex(input[i]);
            }
            return output;
        }

        public static double[] FFTNonComplex(double[] input)
        {
            int length = input.Length;
            MathNet.Numerics.Complex32[] temp = new MathNet.Numerics.Complex32[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = (MathNet.Numerics.Complex32)input[i];
            }
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(temp);
            double[] output = new double[length];
            for (int i = 0; i < length; i++)
            {
                output[i] = temp[i].Magnitude;
            }
            return output;
        }
    }
}
