using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace testapp
{
    static class KeyDetection
    {
        public static void GetKey(double[] Samples, int SampleRate, int Channels)
        {
            double[][] frames = CreateFrames(ToMono(Samples, Channels));
            Complex[][] fftSamples = FastFourierTransform.GetFFT(frames);
            SpectralKernelStruct kernelSpecs = new SpectralKernelStruct(SampleRate, 55, 12, 6, 1024);
            double[][] toneAmplitudes = ConstantQTransform.GetCQT(fftSamples, kernelSpecs);
            
        }

        private static double[] ToMono(double[] input, int Channels)
        {
            int Length = input.Length;
            int outLength = Length / Channels;
            double[] output = new double[outLength + 2];
            int monoLength = 0;
            for (int i = 0; i + Channels < Length; i += Channels)
            {
                double channelSum = 0;
                for (int a = i; a < i + Channels; a++)
                {
                    channelSum += input[a];
                }
                output[monoLength] = channelSum / Channels;
                monoLength++;
            }
            return output;
        }

        private static double[][] CreateFrames(double[] Samples)
        {
            int FrameSize = 1024;
            int Frames = (Samples.Length / FrameSize);
            double[][] output = new double[Frames + 1][];
            for (int frameCounter = 0; frameCounter < Frames; frameCounter++)
            {
                output[frameCounter] = new double[FrameSize];
                for (int sampleCounter = 0; sampleCounter < FrameSize; sampleCounter++)
                {
                    output[frameCounter][sampleCounter] = Samples[FrameSize * frameCounter + sampleCounter];
                }
            }
            return output;
        }
    }
}
