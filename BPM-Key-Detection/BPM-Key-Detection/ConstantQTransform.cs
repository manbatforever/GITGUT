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
        private MusicFile _musicFile;

        private double[][] _cqtToneAmplitudes;

        public ConstantQTransform(MusicFile musicFile)
        {
            _musicFile = musicFile;
        }

        public void Start()
        {
            SpectralKernel spectralKernel = new SpectralKernel(_musicFile.SampleRate, 110, 12, 6);

        }

        private double[][] CreateSampleFrames(double[] samples, int samplesPerFrame, int hopsPerFrame)
        {
            int SampleLength = samples.Length;
            int NumOfFrames = (SampleLength / samplesPerFrame - 1) * hopsPerFrame;
            int HopSize = samplesPerFrame / hopsPerFrame;
            double[][] SampleFrames = new double[NumOfFrames][];
            for (int frame = 0; frame < NumOfFrames; frame++)
            {
                double[] SampleFrame = new double[samplesPerFrame];
                for (int sample = 0; sample < samplesPerFrame; sample++)
                {
                    SampleFrame[sample] = samples[HopSize * frame + sample];
                }
                SampleFrames[frame] = SampleFrame;
            }
            return SampleFrames;
        }


        //public static double[][] GetCQT(Complex[][] FFTsamples, SpectralKernelStruct KernelSpecifications)
        //{
        //    int Frames = FFTsamples.GetLength(0);
        //    int BinsTotal = KernelSpecifications.BinsTotal;
        //    int FrameSize = KernelSpecifications.FrameSize;
        //    double[][] output = new double[Frames][];
        //    Complex[][] Kernel = KernelDatabase.GetKernel(KernelSpecifications);
        //    for (int frame = 0; frame < Frames; frame++)
        //    {
        //        output[frame] = new double[BinsTotal];
        //        for (int bin = 0; bin < BinsTotal; bin++)
        //        {
        //            output[frame][bin] = BinAmplitude(Kernel[bin], FFTsamples[frame], FrameSize);
        //        }
        //    }
        //    return output;
        //}
        
        //private static double BinAmplitude(Complex[] KernelBin, Complex[] FFTSamples, int FrameSize)
        //{
        //    Complex temp = new Complex(0,0);
        //    for (int i = 0; i < FrameSize; i++)
        //    {
        //        temp += KernelBin[i] * FFTSamples[i];
        //    }
        //    return temp.Magnitude / FrameSize;
        //}
    }
}
