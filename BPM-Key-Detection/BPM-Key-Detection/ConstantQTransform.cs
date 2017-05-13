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
        private const int SamplesPerFrame = 16384;
        private const int DownSamplingFactor = 4;

        public double[][] ToneAmplitudes { get => _toneAmplitudes; }

        public ConstantQTransform(double[] inputSamples, double sampleRate)
        {
            _inputSamples = inputSamples;
            _sampleRate = sampleRate / DownSamplingFactor;
            Start();
        }

        public void Start()
        {
            Kernel kernel = new Kernel(_sampleRate, MinimumFrequency, TonesPerOctave, NumOfOctaves, SamplesPerFrame);
            double[][] sampleFrames = CreateSampleFrames(DownSample(_inputSamples), 4);
            Complex[][] fftSamples = FastFourierTransform.FFT(sampleFrames); // X[k] brown og puckette lign. (5)
            _toneAmplitudes = EfficientCQT(kernel.AllSpectralKernels, fftSamples);
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
                        SampleFrame[sample] = BlackmanWindow(samples[sampleIndex]);
                    }
                }
                SampleFrames[frame] = SampleFrame;
            }
            return SampleFrames;
        }

        private double BlackmanWindow(double sample)
        {
            return 0.42d - (0.5d * Math.Cos((2d * Math.PI * sample) / ((double)SamplesPerFrame - 1d))) + (0.08d * Math.Cos((4d * Math.PI * sample) / ((double)SamplesPerFrame - 1d)));
        }

        private double[][] EfficientCQT(Complex[][] spectralKernels, Complex[][] fftSamples)
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

        private double ToneAmplitude(Complex[] spectralKernelBin, Complex[] fftSamples) // Udfører brown og puckette lign. (5)
        {
            double temp = 0;
            for (int i = 0; i < SamplesPerFrame; i++)
            {
                temp += spectralKernelBin[i].Magnitude * fftSamples[i].Magnitude;
            }
            return temp / SamplesPerFrame;
        }
    }
}
