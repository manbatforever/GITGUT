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
        public static Complex[][] FFT(double[][] input)
        {
            int inputLength = input.Length;
            Complex[][] output = new Complex[inputLength][];
            for (int i = 0; i < inputLength; i++)
            {
                output[i] = FFT(input[i]);
            }
            return output;
        }

        public static Complex[] FFT(double[] input)
        {
            int inputLength = input.Length;
            Complex[] output = new Complex[inputLength];
            for (int i = 0; i < inputLength; i++)
            {
                output[i] = new Complex(input[i], 0);
            }
            FFT(output);
            return output;
        }

        public static void FFT(Complex[] buffer)
        {
            int bufferLength = buffer.Length;
            CheckLength(bufferLength);

            int bits = (int)Math.Log(bufferLength, 2);
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

        private static int BitReverse(int n, int bits)
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

        private static void CheckLength(int length)
        {
            if ((length & (length - 1)) == 0)
            {
                throw new Exception("Length has to be power of 2");
            }
        }
    }
}
