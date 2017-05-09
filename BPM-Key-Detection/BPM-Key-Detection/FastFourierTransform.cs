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
            int frames = Samples.Length;
            int frameSize = Samples[0].Length;
            Complex[][] output = new Complex[frames][];
            for (int frameCounter = 0; frameCounter < frames; frameCounter++)
            {
                output[frameCounter] = FFT(Samples[frameCounter]);
            }
            return output;
        }

        private static Complex[] FFT(double[] samples)
        {
            int length = samples.Length;
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

        public static Complex[] FFT(Complex[] input)
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
    }
}
