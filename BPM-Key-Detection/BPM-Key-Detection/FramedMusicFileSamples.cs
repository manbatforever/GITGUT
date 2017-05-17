using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Represents an array of frames containing music file samples.
    class FramedMusicFileSamples
    {
        private double[][] _sampleFrames;
        private int _numOfFrames;

        public FramedMusicFileSamples(MusicFileSamples samples, Window window = null)
        {
            _numOfFrames = (samples.NumOfSamples / Transformations.SamplesPerFrame) - 1;
            _sampleFrames = CreateSampleFrames(samples.SampleArray, samples.NumOfSamples, window);
        }

        public double[][] SampleFrames { get => _sampleFrames; }
        public int NumOfFrames { get => _numOfFrames; }

        private double[][] CreateSampleFrames(double[] samples, int samplesLength, Window window = null)
        {
            if (window == null)
            {
                window = new DefaultWindow(); // Applies a window with no effects
            }
            window.WindowFunction(Transformations.SamplesPerFrame);
            double[][] sampleFrames = new double[_numOfFrames][];
            for (int frame = 0; frame < _numOfFrames; frame++)
            {
                double[] sampleFrame = new double[Transformations.SamplesPerFrame];
                for (int sample = 0; sample < Transformations.SamplesPerFrame; sample++)
                {
                    sampleFrame[sample] = samples[Transformations.HopSize * frame + sample] * window.WindowArray[sample];
                }
                sampleFrames[frame] = sampleFrame;
            }
            return sampleFrames;
        }
    }
}
