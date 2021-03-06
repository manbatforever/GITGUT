﻿namespace BPM_Key_Detection
{
    //Object: Represents an array of frames containing music file samples.
    internal class FramedMusicFileSamples
    {
        private double[][] _sampleFrames;
        private int _numOfFrames;
        private int _samplesPerFrame;
        private int _hopsPerFrame;

        public FramedMusicFileSamples(MusicFileSamples musicFileSamples, int samplesPerFrame, int hopsPerFrame, Window window = null)
        {
            _samplesPerFrame = samplesPerFrame;
            _hopsPerFrame = hopsPerFrame;
            _numOfFrames = (musicFileSamples.NumOfSamples / _samplesPerFrame) * _hopsPerFrame + 1; // -1 to eliminate the last incomplete frame
            _sampleFrames = CreateSampleFrames(musicFileSamples, musicFileSamples.NumOfSamples, window);
        }

        public double[][] SampleFrames { get => _sampleFrames; }
        public int NumOfFrames { get => _numOfFrames; }

        private double[][] CreateSampleFrames(MusicFileSamples musicFileSamples, int samplesLength, Window window)
        {
            if (window == null)
            {
                window = new DefaultWindow(_samplesPerFrame); // Applies a window with no effects
            }
            window.AssignWindowValues();
            int hopSize = _samplesPerFrame / _hopsPerFrame;
            double[][] sampleFrames = new double[_numOfFrames][];
            for (int frame = 0; frame < _numOfFrames; frame++)
            {
                double[] sampleFrame = new double[_samplesPerFrame];
                int overlapLength = hopSize * frame; //Determines how many samples the next frame repeats from the end of the previous frame (frame overlapping)
                for (int sample = 0; sample < _samplesPerFrame; sample++)
                {
                    if (sample + overlapLength< samplesLength)
                        sampleFrame[sample] = musicFileSamples.SampleArray[overlapLength + sample] * window.WindowArray[sample];
                }
                sampleFrames[frame] = sampleFrame;
            }
            return sampleFrames;
        }
    }
}
