using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BPM_Key_Detection
{
    class ConstantQTransform
    {
        private double[] _inputSamples;
        private double _sampleRate;

        private double[][] _toneAmplitudes;

        public const int NumOfOctaves = 6;
        public const int TonesPerOctave = 12;
        public const int TonesTotal = NumOfOctaves * TonesPerOctave;
        private const double MinimumFrequency = 27.5;
        private const int SamplesPerFrame = 4096 * 2 * 2;
        private const int DownSamplingFactor = 10;

        public double[][] ToneAmplitudes { get => _toneAmplitudes; }

        public ConstantQTransform(double[] inputSamples, double sampleRate)
        {
            _inputSamples = inputSamples;
            _sampleRate = sampleRate / DownSamplingFactor;
            Start();
        }

        public void Start()
        {
            IbrahimSpectralKernel kernel = new IbrahimSpectralKernel(_sampleRate, MinimumFrequency, TonesPerOctave, NumOfOctaves, SamplesPerFrame);

            

            //Kernel kernel = new Kernel(_sampleRate, MinimumFrequency, TonesPerOctave, NumOfOctaves, SamplesPerFrame);
            double[][] sampleFrames = CreateSampleFrames(DownSample(_inputSamples), 4);


            double[][] fftSamples = FastFourierTransform.FFTNonComplex(sampleFrames); // X[k] brown og puckette lign. (5)
            _toneAmplitudes = EfficientCQT(kernel.GetAllSpectralKernels(), fftSamples);
            //System.IO.StreamWriter file = new System.IO.StreamWriter("cqttest.txt");
            //foreach (var item in _toneAmplitudes)
            //{
            //    foreach (var iatem in item)
            //    {
            //        file.Write(iatem + ";");
            //    }
            //    file.WriteLine();
            //}
            //file.Close();
        }

        private double[] DownSample(double[] inputSamples)
        {
            int length = inputSamples.Length;
            double[] output = new double[length / DownSamplingFactor + 1];
            int j = 0;
            for (int i = 0; i < length - DownSamplingFactor; i += DownSamplingFactor)
            {
                output[j] = inputSamples[i];
                j++;
            }
            return output;
        }


        private double[][] CreateSampleFrames(double[] samples, int hopsPerFrame)
        {
            double[] blackmanWindow = BlackmanWindow();
            int SampleLength = samples.Length;
            int NumOfFrames = (SampleLength / SamplesPerFrame + 1) * hopsPerFrame;
            int HopSize = SamplesPerFrame / hopsPerFrame;
            double[][] SampleFrames = new double[NumOfFrames][];
            for (int frame = 0; frame < NumOfFrames; frame++)
            {
                double[] SampleFrame = new double[SamplesPerFrame];
                for (int sample = 0; sample < SamplesPerFrame; sample++)
                {
                    int sampleIndex = HopSize * frame + sample;
                    if (sampleIndex < SampleLength)
                    {
                        SampleFrame[sample] = samples[sampleIndex] * blackmanWindow[sample];
                    }
                }
                SampleFrames[frame] = SampleFrame;
            }
            return SampleFrames;
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

        private double[][] EfficientCQT(double[][] spectralKernels, double[][] fftSamples)
        {
            int numOfSampleFrames = fftSamples.Length;
            double[][] toneAmplitudes = new double[numOfSampleFrames][];
            for (int frame = 0;numOfSampleFrames == 0 || frame < numOfSampleFrames; frame++)
            {
                toneAmplitudes[frame] = new double[TonesTotal];
                for (int tone = 0; tone < TonesTotal; tone++)
                {
                    toneAmplitudes[frame][tone] = ToneAmplitude(spectralKernels[tone], fftSamples[frame]);
                }
            }
            return toneAmplitudes;
        }

        private double ToneAmplitude(double[] spectralKernelBin, double[] fftSamples) // Udfører brown og puckette lign. (5)
        {
            double temp = 0;
            for (int i = 0; i < SamplesPerFrame; i++)
            {
                temp += spectralKernelBin[i] * fftSamples[i];
            }
            return temp / SamplesPerFrame;
        }
    }
}
