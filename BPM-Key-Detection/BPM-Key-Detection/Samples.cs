using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public class Samples
    {

        private double[] _sampleArray;
        private int _numOfSamples;
        private int _sampleRate;
        private int _channels;

        public double[] SampleArray { get => _sampleArray; }
        public int NumOfSamples { get => _numOfSamples; }
        public int SampleRate { get => _sampleRate; }
        public int Channels { get => _channels; }

        public Samples(float[] samples, int numOfSamples, int sampleRate, int channels)
        {
            _sampleArray = new double[numOfSamples];
            for (int i = 0; i < numOfSamples; i++)
            {
                _sampleArray[i] = samples[i];
            }
            _numOfSamples = numOfSamples;
            _sampleRate = sampleRate;
            _channels = channels;
        }

        public Samples(double[] samples, int numOfSamples, int sampleRate, int channels)
        {
            _sampleArray = samples;
            _numOfSamples = numOfSamples;
            _sampleRate = sampleRate;
            _channels = channels;
        }


        public Samples ApplyLowpassFilter(int cutoffFrequency)
        {
            NAudio.Dsp.BiQuadFilter lowpassFilter = NAudio.Dsp.BiQuadFilter.LowPassFilter(_sampleRate, cutoffFrequency, 1);
            double[] filteredSamples = new double[_numOfSamples];
            for (int i = 0; i < _numOfSamples; i++)
            {
                filteredSamples[i] = lowpassFilter.Transform(Convert.ToSingle(_sampleArray[i]));
            }
            return new Samples(filteredSamples, _numOfSamples, _sampleRate, _channels);
        }

        public Samples ToMono()
        {
            if (_channels == 1)
            {
                return this;
            }
            else
            {
                int monoLength = _numOfSamples / _channels;
                double[] monoSamples = new double[monoLength];
                for (int monoSample = 0; monoSample < monoLength; monoSample++)
                {
                    double channelSum = 0;
                    for (int channel = 0; channel < _channels; channel++)
                    {
                        channelSum += _sampleArray[monoSample * _channels + channel];
                    }
                    monoSamples[monoSample] = channelSum;
                }
                return new Samples(monoSamples, monoLength, _sampleRate, 1);
            }
            
        }

        public Samples DownSample(int downSamplingFactor)
        {
            int newSamplerate = _sampleRate / downSamplingFactor;
            int newLength = _numOfSamples / downSamplingFactor;
            double[] downSampled = new double[newLength];
            for (int step = 0; step < newLength; step++)
            {
                for (int channel = 0; channel < _channels; channel++)
                {
                    downSampled[step] = _sampleArray[step * downSamplingFactor + channel];
                }
            }
            return new Samples(downSampled, newLength, newSamplerate, _channels);
        }

        public FramedSamples CreateSampleFrames(int samplesPerFrame, int hopSize)
        {
            return new FramedSamples(this, samplesPerFrame, hopSize);
        }
    }
}