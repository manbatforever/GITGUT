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
            Exocortex.DSP.Complex[] temp = new Exocortex.DSP.Complex[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = (Exocortex.DSP.Complex)input[i];
            }
            Exocortex.DSP.Fourier.FFT(temp, length, Exocortex.DSP.FourierDirection.Forward);
            Complex[] output = new Complex[length];
            for (int i = 0; i < length; i++)
            {
                output[i] = new Complex(temp[i].Re, temp[i].Im);
            }
            return output;
        }
    }
}
