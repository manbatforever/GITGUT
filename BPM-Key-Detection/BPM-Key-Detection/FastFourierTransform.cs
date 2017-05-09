using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace testapp
{
    static class FastFourierTransform
    {
        public static Complex[][] GetFFT(double[][] Samples)
        {
            int frames = Samples.GetLength(0);
            int frameSize = Samples[0].Length;
            Complex[][] output = new Complex[frames][];
            for (int frameCounter = 0; frameCounter < frames; frameCounter++)
            {
                output[frameCounter] = FFT(Samples[frameCounter], frameSize);
            }
            return output;
        }

        private static Complex[] FFT(double[] samples, int length)
        {
            Exocortex.DSP.Complex[] temp = new Exocortex.DSP.Complex[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = (Exocortex.DSP.Complex)samples[i];
            }
            Exocortex.DSP.Fourier.FFT(temp, length, Exocortex.DSP.FourierDirection.Forward);
            Complex[] output = new Complex[length];
            for (int i = 0; i < length; i++)
            {
                output[i] = new Complex(temp[i].Re, temp[i].Im);
            }
            return output;
        }

        public static Complex[] ComplexFFT(Complex[] input)
        {
            int length = input.Length;
            Exocortex.DSP.Complex[] temp = new Exocortex.DSP.Complex[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = new Exocortex.DSP.Complex(input[i].Real, input[i].Imaginary);
            }
            Exocortex.DSP.Fourier.FFT(temp, length, Exocortex.DSP.FourierDirection.Forward);
            Complex[] output = new Complex[length];
            for (int i = 0; i < length; i++)
            {
                output[i] = new Complex(temp[i].Re, temp[i].Im);
            }
            return output;
        }



        public static int BitReverse(int n, int bits)
        {
            int reversedN = n;
            int count = bits - 1;

            n >>= 1;
            while (n > 0)
            {
                reversedN = (reversedN << 1) | (n & 1);
                count--;
                n >>= 1;
            }

            return ((reversedN << count) & ((1 << bits) - 1));
        }


        public static void FFT(Complex[] buffer)
        {

            int bits = (int)Math.Log(buffer.Length, 2);
            for (int j = 1; j < buffer.Length / 2; j++)
            {

                int swapPos = BitReverse(j, bits);
                var temp = buffer[j];
                buffer[j] = buffer[swapPos];
                buffer[swapPos] = temp;
            }

            for (int N = 2; N <= buffer.Length; N <<= 1)
            {
                for (int i = 0; i < buffer.Length; i += N)
                {
                    for (int k = 0; k < N / 2; k++)
                    {

                        int evenIndex = i + k;
                        int oddIndex = i + k + (N / 2);
                        var even = buffer[evenIndex];
                        var odd = buffer[oddIndex];

                        double term = -2 * Math.PI * k / (double)N;
                        Complex exp = new Complex(Math.Cos(term), Math.Sin(term)) * odd;

                        buffer[evenIndex] = even + exp;
                        buffer[oddIndex] = even - exp;

                    }
                }
            }
        }
    }
}
