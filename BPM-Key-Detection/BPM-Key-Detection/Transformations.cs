using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Contains all functionality and constants concering Transformations (FFT and CQT)
    static class Transformations
    {
        public static readonly int CutoffFrequency = 2000;
        public static readonly int SamplesPerFrame = 16384;
        public static readonly int HopsPerFrame = 4;
        public static readonly int TonesPerOctave = 12;
        public static readonly int NumOfOctaves = 6;
        public static readonly int TonesTotal = TonesPerOctave * NumOfOctaves;
        public static readonly double MinimumFrequency = 27.5;

        public static FramedToneAmplitudes CQT(MusicFileSamples musicFileSamples)
        {
            musicFileSamples.ToMono();
            musicFileSamples.LowpassFilter(CutoffFrequency);
            musicFileSamples.DownSample(CutoffFrequency);
            FramedMusicFileSamples framedMusicFileSamples = musicFileSamples.CreateFramedMusicFileSamples(new BlackmanWindow());
            FramedFrequencyBins ffTransformedSamples = FFT(framedMusicFileSamples);
            IbrahimSpectralKernel ibrahimSpectralKernel = new IbrahimSpectralKernel(musicFileSamples.Samplerate);

            return new FramedToneAmplitudes(ffTransformedSamples, ibrahimSpectralKernel);
        }

        public static FramedFrequencyBins FFT(FramedMusicFileSamples frames)
        {
            MathNet.Numerics.Complex32[][] framedFrequencyBins = new MathNet.Numerics.Complex32[frames.NumOfFrames][];
            for (int frame = 0; frame < frames.NumOfFrames; frame++)
            {
                framedFrequencyBins[frame] = FFT(frames.SampleFrames[frame]);
            }
            return new FramedFrequencyBins(framedFrequencyBins);
        }

        public static MathNet.Numerics.Complex32[] FFT(double[] samples)
        {
            int numOfSamples = samples.Length;
            MathNet.Numerics.Complex32[] frequencyBins = new MathNet.Numerics.Complex32[numOfSamples];
            for (int sample = 0; sample < numOfSamples; sample++)
            {
                frequencyBins[sample] = new MathNet.Numerics.Complex32((float)samples[sample], 0); // Cast to float, lose precision
            }
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(frequencyBins);
            return frequencyBins;
        }

        public static Complex[] FFT(Complex[] input)
        {
            int length = input.Length;
            MathNet.Numerics.Complex32[] temp = new MathNet.Numerics.Complex32[length];
            for (int i = 0; i < length; i++)
            {
                temp[i] = new MathNet.Numerics.Complex32(Convert.ToSingle(input[i].Real), Convert.ToSingle(input[i].Imaginary));
            }
            MathNet.Numerics.IntegralTransforms.Fourier.Forward(temp);
            Complex[] output = new Complex[length];
            for (int i = 0; i < length; i++)
            {
                output[i] = new Complex(temp[i].Real, temp[i].Imaginary);
            }
            return output;
        }
    }
}
