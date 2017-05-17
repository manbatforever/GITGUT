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

        public FramedMusicFileSamples(MusicFileSamples musicFileSamples, Window window = null)
        {
            _numOfFrames = (musicFileSamples.NumOfSamples / Transformations.SamplesPerFrame - 1) * Transformations.HopsPerFrame; // -1 to eliminate the last incomplete frame
            _sampleFrames = CreateSampleFrames(musicFileSamples, musicFileSamples.NumOfSamples, window);
        }

        public double[][] SampleFrames { get => _sampleFrames; }
        public int NumOfFrames { get => _numOfFrames; }

        private double[][] CreateSampleFrames(MusicFileSamples musicFileSamples, int samplesLength, Window window)
        {
            if (window == null)
            {
                window = new DefaultWindow(); // Applies a window with no effects
            }
            int hopSize = Transformations.SamplesPerFrame / Transformations.HopsPerFrame;
            window.WindowFunction(Transformations.SamplesPerFrame);
            double[][] sampleFrames = new double[_numOfFrames][];
            for (int frame = 0; frame < _numOfFrames; frame++)
            {
                double[] sampleFrame = new double[Transformations.SamplesPerFrame];
                int overlapLength = hopSize * frame; //Determines how many samples the next frame repeats from the end of the previous frame (frame overlapping)
                for (int sample = 0; sample < Transformations.SamplesPerFrame; sample++)
                {
                    sampleFrame[sample] = musicFileSamples.SampleArray[overlapLength + sample] * window.WindowArray[sample];
                }
                sampleFrames[frame] = sampleFrame;
            }
            return sampleFrames;
        }
    }
}
