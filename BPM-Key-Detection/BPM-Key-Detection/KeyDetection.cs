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
        public static void GetKey(double[] Samples, int SampleRate)
        {
            double[][] frames = CreateFrames(DownSample(Samples));
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

        private static double[][] CreateFrames(double[] Samples)
        {
            int FrameSize = 1024;
            int Frames = Samples.Length / FrameSize;
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
