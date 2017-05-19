using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class FFT
    {
        private FramedFrequencyBins _framedFrequencyBins; //1st constructer "output"
        private Complex[] _frequencyBins; //2nd constructor "output"


        public FFT(FramedMusicFileSamples frames) //Constructor for FramedMusicFileSamples (CQT)
        {
            MathNet.Numerics.Complex32[][] framedFrequencyBins = new MathNet.Numerics.Complex32[frames.NumOfFrames][];
            for (int frame = 0; frame < frames.NumOfFrames; frame++)
            {
                framedFrequencyBins[frame] = SingleFrameFFT(frames.SampleFrames[frame]);
            }
            _framedFrequencyBins = new FramedFrequencyBins(framedFrequencyBins);
        }


        public FFT(Complex[] input) //Constructor for default input (BPM)
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
            _frequencyBins = output;
        }

        public  MathNet.Numerics.Complex32[] SingleFrameFFT(double[] samples)
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


        public FramedFrequencyBins FramedFrequencyBins { get => _framedFrequencyBins; set => _framedFrequencyBins = value; }
        public Complex[] FrequencyBins { get => _frequencyBins; set => _frequencyBins = value; }

    }
}
