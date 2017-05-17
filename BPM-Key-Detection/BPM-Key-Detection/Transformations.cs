using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Contains all functionality and constants concering Transformations (FFT and CQT)
    static class Transformations
    {
        public static readonly int CutoffFrequency = 2000;
        public static readonly int DownSamplingFactor = 10;
        public static readonly int SamplesPerFrame = 16384;
        public static readonly int HopSize = 4;
        public static readonly int TonesPerOctave = 12;
        public static readonly int NumOfOctaves = 6;
        public static readonly int TonesTotal = TonesPerOctave * NumOfOctaves;
        public static readonly double MinimumFrequency = 27.5;

        public static FramedToneAmplitudes CQT(MusicFileSamples musicFileSamples)
        {
            musicFileSamples.LowpassFilter(CutoffFrequency);
            musicFileSamples.ToMono();
            musicFileSamples.DownSample(DownSamplingFactor);
            FramedMusicFileSamples framedMusicFileSamples = musicFileSamples.CreateFramedMusicFileSamples(new BlackmanWindow());
            FramedFrequencyBins ffTransformedSamples = FFT(framedMusicFileSamples);
            IbrahimSpectralKernel ibrahimSpectralKernel = new IbrahimSpectralKernel(musicFileSamples.Samplerate);

            return new FramedToneAmplitudes(ffTransformedSamples, ibrahimSpectralKernel);
        }

        public static FramedFrequencyBins FFT(FramedMusicFileSamples frames)
        {
            double[][] framedFrequencyBins = new double[frames.NumOfFrames][];
            for (int frame = 0; frame < frames.NumOfFrames; frame++)
            {
                framedFrequencyBins[frame] = FFT(frames.SampleFrames[frame]).BinValues;
            }
            return new FramedFrequencyBins(framedFrequencyBins);
        }

        public static FrequencyBins FFT(double[] samples)
        {
            int numOfSamples = samples.Length;
            MathNet.Numerics.Complex32[] tempComplex = new MathNet.Numerics.Complex32[numOfSamples];
            for (int sample = 0; sample < numOfSamples; sample++)
            {
                tempComplex[sample] = new MathNet.Numerics.Complex32((float)samples[sample], 0); // Cast to float, lose precision
            }
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(tempComplex);
            return new FrequencyBins(tempComplex);
        }

    }
}
