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
            SpectralKernelStruct kernelSpecs = new SpectralKernelStruct(SampleRate, 110, 12, 6, 16384);
            double[][] frames = CreateFrames(ToMono(Samples, Channels), kernelSpecs);
            Complex[][] fftSamples = FastFourierTransform.FFT(frames);
            double[][] toneAmplitudes = ConstantQTransform.GetCQT(fftSamples, kernelSpecs);
            double[][] sngle = ReduceToOctave(toneAmplitudes, kernelSpecs);
            System.IO.StreamWriter file = new System.IO.StreamWriter("toneamp.txt");
            foreach (var item in toneAmplitudes)
            {
                foreach (var idtem in item)
                {
                    file.Write(idtem + ";");
                }
                file.WriteLine();
            }
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("chroma.txt");
            foreach (var item in sngle)
            {
                foreach (var idtem in item)
                {
                    file2.Write(idtem + ";");
                }
                file2.WriteLine();
            }
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

        private static double[][] CreateFrames(double[] Samples, SpectralKernelStruct kernel)
        {
            int FrameSize = kernel.FrameSize;
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

        private static double[][] ReduceToOctave(double[][] input, SpectralKernelStruct kernel)
        {
            int Frames = input.Length;
            int UniqueTones = kernel.BinsPerOctave;
            int Octaves = kernel.BinsTotal / UniqueTones;
            double[][] output = new double[Frames][];
            for (int frame = 0; frame < Frames; frame++)
            {
                output[frame] = new double[UniqueTones];
                for (int octave = 0; octave < Octaves; octave++)
                {
                    for (int tone = 0; tone < UniqueTones; tone++)
                    {
                        output[frame][tone] += input[frame][tone + octave * UniqueTones];
                    }
                }
            }
            return output;
        }
    }
}
