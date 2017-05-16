using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public static class Transformations
    {
        public static readonly int CutoffFrequency = 2000;
        public static readonly int DownSamplingFactor = 10;
        public static readonly int SamplesPerFrame = 16384;
        public static readonly int HopSize = 4;
        public static readonly int TonesPerOctave = 12;
        public static readonly int NumOfOctaves = 6;
        public static readonly int TonesTotal = TonesPerOctave * NumOfOctaves;
        public static readonly double MinimumFrequency = 27.5;

        public static ToneAmplitudes CQT(MusicFileSamples samples)
        {
            MusicFileSamples musicFileSamples = samples.LowpassFilter(CutoffFrequency).ToMono().DownSample(DownSamplingFactor);
            MusicFileSamples[] framedMusicFileSamples = CreateSampleFrames(musicFileSamples, SamplesPerFrame, HopSize, new BlackmanWindow());
            FrequencyBins[] ffTransformedSamples = FFT(framedMusicFileSamples);
            SpectralKernel[] spectralKernel = CreateSpectralKernels(musicFileSamples.Samplerate);
            
        }

        private static SpectralKernel[] CreateSpectralKernels(int samplerate)
        {
            SpectralKernel[] spectralKernels = new SpectralKernel[TonesTotal];
            for (int kernel = 0; kernel < TonesTotal; kernel++)
            {
                spectralKernels[kernel] = new IbrahimSpectralKernel(samplerate, kernel);
            }
            return spectralKernels;
        }

        private static MusicFileSamples[] CreateSampleFrames(MusicFileSamples allSamples, int samplesPerFrame, int hopSize, Window window = null)
        {
            if (window == null)
            {
                window = new DefaultWindow(); // Mulighed for at undlade vinduefunktion.
            }
            window.WindowFunction(samplesPerFrame);
            int numOfFrames = allSamples.NumOfSamples / samplesPerFrame;
            MusicFileSamples[] sampleFrames = new MusicFileSamples[numOfFrames];
            for (int frame = 0; frame < numOfFrames; frame++)
            {
                double[] sampleFrame = new double[samplesPerFrame];
                for (int sample = 0; sample < samplesPerFrame; sample++)
                {
                    sampleFrame[sample] = allSamples.SampleArray[hopSize * frame + sample] * window.WindowArray[sample];
                }
                sampleFrames[frame] = new MusicFileSamples(sampleFrame);
            }
            return sampleFrames;
        }

        public static FrequencyBins[] FFT(Samples[] frames)
        {
            int numOfFrames = frames.Length;
            FrequencyBins[] frequencyBinFrames = new FrequencyBins[numOfFrames];
            for (int frame = 0; frame < numOfFrames; frame++)
            {
                frequencyBinFrames[frame] = FFT(frames[frame]);
            }
            return frequencyBinFrames;
        }

        public static FrequencyBins FFT(Samples samples)
        {
            MathNet.Numerics.Complex32[] tempComplex = new MathNet.Numerics.Complex32[samples.NumOfSamples];
            for (int sample = 0; sample < samples.NumOfSamples; sample++)
            {
                tempComplex[sample] = new MathNet.Numerics.Complex32((float)samples.SampleArray[sample], 0); // Mister lidt præcision
            }
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(tempComplex);
            return new FrequencyBins(tempComplex);
        }

    }
}
