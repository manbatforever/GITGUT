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
            SpectralKernelStruct kernelSpecs = new SpectralKernelStruct(SampleRate, 440, 12, 1, 8192);
            double[][] frames = CreateFrames(ToMono(Samples, Channels), kernelSpecs);
            Complex[][] fftSamples = FastFourierTransform.FFT(frames);
            double[][] toneAmplitudes = ConstantQTransform.GetCQT(fftSamples, kernelSpecs);
            double[] sngle = ChromaVector(toneAmplitudes, kernelSpecs);
            EstimateKey(sngle);
            //System.IO.StreamWriter file = new System.IO.StreamWriter("toneamp.txt");
            //foreach (var item in toneAmplitudes)
            //{
            //    foreach (var idtem in item)
            //    {
            //        file.Write(idtem + ";");
            //    }
            //    file.WriteLine();
            //}
            //file.Close();
            //System.IO.StreamWriter file1 = new System.IO.StreamWriter("a.txt");
            //foreach (double idtem in sngle)
            //{
            //    file1.Write(idtem + ";");
            //}
            //file1.Close();
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
            int Frames = (Samples.Length / FrameSize) - 2;
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

        private static double[] ChromaVector(double[][] input, SpectralKernelStruct kernel)
        {
            int Frames = input.Length;
            int UniqueTones = kernel.BinsPerOctave;
            int Octaves = kernel.BinsTotal / UniqueTones;
            double[] output = new double[UniqueTones];
            for (int frame = 0; frame < Frames; frame++)
            {
                for (int octave = 0; octave < Octaves; octave++)
                {
                    for (int tone = 0; tone < UniqueTones; tone++)
                    {
                        output[tone] += input[frame][tone + octave * UniqueTones];
                    }
                }
            }
            return output;
        }

        private static double[] MajorProfile =
        {
            7.23900502618145225142, // Tonica
            3.50351166725158691406,
            3.58445177536649417505,
            2.84511816478676315967,
            5.81898892118549859731,
            4.55865057415321039969,
            2.44778850545506543313,
            6.99473192146829525484,
            3.39106613673504853068,
            4.55614256655143456953,
            4.07392666663523606019,
            4.45932757378886890365
        };

        private static double[] MinorProfile =
        {
            7.00255045060284420089,
            3.14360279015996679775,
            4.35904319714962529275,
            5.40418120718934069657,
            3.67234420879306133756,
            4.08971184917797891956,
            3.90791435991553992579,
            6.19960288562316463867,
            3.63424625625277419871,
            2.87241191079875557435,
            5.35467999794542670600,
            3.83242038595048351013
        };

        private static double DotProduct(double[] V1, double[] V2, int length)
        {
            double output = 0;
            for (int i = 0; i < length; i++)
            {
                output += V1[i] * V2[i];
            }
            return output;
        }

        private static double Magnitude(double[] Vector, int length)
        {
            double output = 0;
            for (int i = 0; i < length; i++)
            {
                output += Math.Pow(Vector[i], 2d);
            }
            return Math.Sqrt(output);
        }

        private static double CosineSimilarity(double[] V1, double[] V2, int length)
        {
            return DotProduct(V1, V2, length) / (Magnitude(V1, length) * Magnitude(V2, length));
        }

        private static double[][] CreateProfileForEachTonica(double[] Profile)
        {
            double[][] ProfileForEachTonica = new double[12][];
            for (int i = 12; i > 0; i--)
            {
                double[] currentPermutation = new double[12];
                for (int a = 0; a < 12; a++)
                {
                    currentPermutation[a] = Profile[(i + a) % 12];
                }
                ProfileForEachTonica[i - 1] = currentPermutation;
            }
            return ProfileForEachTonica;
        }

        private static void EstimateKey(double[] ChromaVector)
        {
            double[][] MajorProfiles = CreateProfileForEachTonica(MajorProfile);
            double[][] MinorProfiles = CreateProfileForEachTonica(MinorProfile);
            double[] MajorSimilarity = new double[12];
            double[] MinorSimilarity = new double[12];
            for (int i = 0; i < 12; i++)
            {
                MajorSimilarity[i] = CosineSimilarity(MajorProfiles[i], ChromaVector, 12);
                MinorSimilarity[i] = CosineSimilarity(MinorProfiles[i], ChromaVector, 12);
                Console.Write(MajorSimilarity[i] + " ");
                Console.WriteLine(MinorSimilarity[i]);
            }
            Console.Write(MajorSimilarity.ToList().IndexOf(MajorSimilarity.Max()) + " ");
            Console.WriteLine(MinorSimilarity.ToList().IndexOf(MinorSimilarity.Max()));
            Console.ReadKey();
        }
    }
}
