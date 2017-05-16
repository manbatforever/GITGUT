using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public class FramedSamples
    {
        private double[][] _sampleFrames;
        private int _samplesPerFrame;
        private int _numOfFrames;
        private int _hopSize;

        public FramedSamples(Samples samples, int samplesPerFrame, int hopSize)
        {
            _samplesPerFrame = samplesPerFrame;
            _numOfFrames = (samples.NumOfSamples / samplesPerFrame) - 1;
            _hopSize = hopSize;
            _sampleFrames = CreateSampleFrames(samples.SampleArray, samples.NumOfSamples, samplesPerFrame, hopSize);
        }

        public double[][] SampleFrames { get => _sampleFrames; }
        public int SamplesPerFrame { get => _samplesPerFrame; }
        public int NumOfFrames { get => _numOfFrames; }
        public int HopSize { get => _hopSize; }

        private double[][] CreateSampleFrames(double[] samples, int samplesLength, int samplesPerFrame, int hopSize)
        {
            double[][] sampleFrames = new double[_numOfFrames][];
            for (int frame = 0; frame < _numOfFrames; frame++)
            {
                double[] sampleFrame = new double[samplesPerFrame];
                for (int sample = 0; sample < samplesPerFrame; sample++)
                {
                    sampleFrame[sample] = samples[hopSize * frame + sample];
                }
                sampleFrames[frame] = sampleFrame;
            }
            return sampleFrames;
        }


    }
}
