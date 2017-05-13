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
        private const int SamplesPerFrame = 4096 ;

        public double[][] ToneAmplitudes { get => _toneAmplitudes; }

        public ConstantQTransform(double[] inputSamples, double sampleRate)
        {
            _inputSamples = inputSamples;
            _sampleRate = sampleRate;
            Start();
        }

        public void Start()
        {
            Kernel kernel = new Kernel(_sampleRate, MinimumFrequency, TonesPerOctave, NumOfOctaves, SamplesPerFrame);
            double[][] sampleFrames = CreateSampleFrames(_inputSamples, SamplesPerFrame, 2);
            Complex[][] fftSamples = FastFourierTransform.FFT(sampleFrames); // X[k] brown og puckette lign. (5)
            _toneAmplitudes = EfficientCQT(kernel.AllSpectralKernels, fftSamples);
        }


        private double[][] CreateSampleFrames(double[] samples, int samplesPerFrame, int hopsPerFrame)
        {
            int SampleLength = samples.Length;
            int NumOfFrames = (SampleLength / samplesPerFrame + 1) * hopsPerFrame;
            int HopSize = samplesPerFrame / hopsPerFrame;
            double[][] SampleFrames = new double[NumOfFrames][];
            for (int frame = 0; frame < NumOfFrames; frame++)
            {
                double[] SampleFrame = new double[samplesPerFrame];
                for (int sample = 0; sample < samplesPerFrame; sample++)
                {
                    int sampleIndex = HopSize * frame + sample;
                    if (sampleIndex < SampleLength)
                    {
                        SampleFrame[sample] = samples[sampleIndex];
                    }
                }
                SampleFrames[frame] = SampleFrame;
            }
            return SampleFrames;
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
            Complex temp = new Complex(0, 0);
            for (int i = 0; i < SamplesPerFrame; i++)
            {
                temp += spectralKernelBin[i] * fftSamples[i];
            }
            return (temp / SamplesPerFrame).Magnitude;
        }
    }
}
