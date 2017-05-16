using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BPM_Key_Detection
{
    public class ConstantQTransform
    {
        private Samples _samples;

        public static int CutoffFrequency = 2000;
        public static int DownSamplingFactor = 10;
        public static int SamplesPerFrame = 16384;
        public static int HopSize = 4;
        public static int TonesPerOctave = 12;
        public static int NumOfOctaves = 6;
        public static int TonesTotal = TonesPerOctave * NumOfOctaves;
        public static double MinimumFrequency = 27.5;

        public ConstantQTransform(Samples samples)
        {
            FramedSamples t = samples.ApplyLowpassFilter(CutoffFrequency).ToMono().DownSample(DownSamplingFactor).CreateSampleFrames(SamplesPerFrame, HopSize);
            ApplyWindowFunction(t, BlackmanWindow());
        }

        private void ApplyWindowFunction(FramedSamples framedSamples, double[] windowFunction)
        {
            for (int frame = 0; frame < framedSamples.NumOfFrames; frame++)
            {
                for (int sample = 0; sample < framedSamples.SamplesPerFrame; sample++)
                {
                    framedSamples.SampleFrames[frame][sample] *= windowFunction[sample];
                }
            }
        }

        private double[] BlackmanWindow()
        {
            double a = 0.16d;
            double a0 = (1d - a) / 2d;
            double a1 = 1d / 2d;
            double a2 = a / 2d;
            double[] blackmanWindow = new double[SamplesPerFrame];
            for (int n = 0; n < SamplesPerFrame; n++)
            {
                blackmanWindow[n] = a0 - (a1 * Math.Cos((2d * Math.PI * n) / ((double)SamplesPerFrame))) + (a2 * Math.Cos((4d * Math.PI * n) / ((double)SamplesPerFrame)));
            }
            return blackmanWindow;
        }

        private double[] HammingWindow()
        {
            double alpha = 25d / 46d;
            double[] hammingWindow = new double[SamplesPerFrame];
            for (int n = 0; n < SamplesPerFrame; n++)
            {
                hammingWindow[n] = alpha - (1d - alpha) * Math.Cos(2d * Math.PI * (double)n / (double)SamplesPerFrame);
            }
            return hammingWindow;
        }

        private double[] HannWindow()
        {
            double[] hannWindow = new double[SamplesPerFrame];
            for (int n = 0; n < SamplesPerFrame; n++)
            {
                hannWindow[n] = 0.5d * (1d - Math.Cos(2d * Math.PI * (double)n / (double)SamplesPerFrame));
            }
            return hannWindow;
        }

        //private double[] _inputSamples;
        //private double _sampleRate;

        //private double[][] _toneAmplitudes;

        //public const int NumOfOctaves = 6;
        //public const int TonesPerOctave = 12;
        //public const int TonesTotal = NumOfOctaves * TonesPerOctave;
        //private const double MinimumFrequency = 27.5;
        //private const int SamplesPerFrame = 4096 * 2 * 2;
        //private const int DownSamplingFactor = 10;

        //public double[][] ToneAmplitudes { get => _toneAmplitudes; }

        //public ConstantQTransform(double[] inputSamples, double sampleRate)
        //{
        //    _inputSamples = inputSamples;
        //    _sampleRate = sampleRate / DownSamplingFactor;
        //    Start();
        //}

        //public void Start()
        //{

        //    IbrahimSpectralKernel kernel = new IbrahimSpectralKernel(_sampleRate, MinimumFrequency, TonesPerOctave, NumOfOctaves, SamplesPerFrame);
        //    double[][] sampleFrames = CreateSampleFrames(DownSample(_inputSamples), 4);
        //    double[][] fftSamples = FastFourierTransform.FFTNonComplex(sampleFrames); // X[k] brown og puckette lign. (5)
        //    _toneAmplitudes = EfficientCQT(kernel.GetAllSpectralKernels(), fftSamples);
        //}

        //private double[] DownSample(double[] inputSamples)
        //{
        //    int length = inputSamples.Length;
        //    double[] output = new double[length / DownSamplingFactor + 1];
        //    int j = 0;
        //    for (int i = 0; i < length - DownSamplingFactor; i += DownSamplingFactor)
        //    {
        //        output[j] = inputSamples[i];
        //        j++;
        //    }
        //    return output;
        //}


        //private double[][] CreateSampleFrames(double[] samples, int hopsPerFrame)
        //{
        //    double[] blackmanWindow = BlackmanWindow();
        //    int SampleLength = samples.Length;
        //    int NumOfFrames = (SampleLength / SamplesPerFrame + 1) * hopsPerFrame;
        //    int HopSize = SamplesPerFrame / hopsPerFrame;
        //    double[][] SampleFrames = new double[NumOfFrames][];
        //    for (int frame = 0; frame < NumOfFrames; frame++)
        //    {
        //        double[] SampleFrame = new double[SamplesPerFrame];
        //        for (int sample = 0; sample < SamplesPerFrame; sample++)
        //        {
        //            int sampleIndex = HopSize * frame + sample;
        //            if (sampleIndex < SampleLength)
        //            {
        //                SampleFrame[sample] = samples[sampleIndex] * blackmanWindow[sample];
        //            }
        //        }
        //        SampleFrames[frame] = SampleFrame;
        //    }
        //    return SampleFrames;
        //}



        //private double[][] EfficientCQT(double[][] spectralKernels, double[][] fftSamples)
        //{
        //    int numOfSampleFrames = fftSamples.Length;
        //    double[][] toneAmplitudes = new double[numOfSampleFrames][];
        //    for (int frame = 0;numOfSampleFrames == 0 || frame < numOfSampleFrames; frame++)
        //    {
        //        toneAmplitudes[frame] = new double[TonesTotal];
        //        for (int tone = 0; tone < TonesTotal; tone++)
        //        {
        //            toneAmplitudes[frame][tone] = ToneAmplitude(spectralKernels[tone], fftSamples[frame]);
        //        }
        //    }
        //    return toneAmplitudes;
        //}

        //private double ToneAmplitude(double[] spectralKernelBin, double[] fftSamples) // Udfører brown og puckette lign. (5)
        //{
        //    double temp = 0;
        //    for (int i = 0; i < SamplesPerFrame; i++)
        //    {
        //        temp += spectralKernelBin[i] * fftSamples[i];
        //    }
        //    return temp / SamplesPerFrame;
        //}
    }
}
