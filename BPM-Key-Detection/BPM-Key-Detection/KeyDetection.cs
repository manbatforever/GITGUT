using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BPM_Key_Detection
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

        private static double[] DownSample(double[] inputSamples)
        {
            int length = inputSamples.Length;
            double[] output = new double[length / 4];
            int j = 0;
            for (int i = 0; i < length; i += 4)
            {
                output[j] = inputSamples[i];
                j++;
            }
            return output;
        }

        private static double[] ToMono(double[] input, int Channels)
        {
            int Length = input.Length;
            int outLength = Length / Channels;
            double[] output = new double[outLength];
            for (int i = 0; i < outLength; i++)
            {
                for (int a = i * Channels; a < a + Channels; a++)
                {
                    output[i] += input[a];
                }
                output[i] /= Channels;
            }
            return output;
        }

        private static double[][] CreateFrames(double[] Samples)
        {
            int FrameSize = 1024;
            int Frames = (Samples.Length / FrameSize) - 1;
            double[][] output = new double[Frames][];
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
