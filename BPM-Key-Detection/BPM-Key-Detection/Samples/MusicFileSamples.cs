using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Extracted samples from a musicfile. Contains certain functionality such as signal processing
    class MusicFileSamples : Samples
    {
        private int _sampleRate;
        private int _channels;
        private bool _lowpassFiltered;
        private bool _isMono;
        private bool _downSampled;

        public int Samplerate { get => _sampleRate; }
        public int Channels { get => _channels; }
        public bool LowpassFiltered { get => _lowpassFiltered; }
        public bool IsMono { get => _isMono; }
        public bool DownSampled { get => _downSampled; }

        public MusicFileSamples(float[] sampleValues, int sampleRate, int channels)
        {
            _numOfSamples = sampleValues.Length;
            _sampleArray = new double[_numOfSamples];
            for (int i = 0; i < _numOfSamples; i++)
            {
                _sampleArray[i] = sampleValues[i];
            }
            _sampleRate = sampleRate;
            _channels = channels;
            if (_channels == 1)
            {
                _isMono = true;
            }
        }

        public MusicFileSamples(double[] sampleValues, int sampleRate, int channels)
        {
            _numOfSamples = sampleValues.Length;
            _sampleArray = sampleValues;
            _sampleRate = sampleRate;
            _channels = channels;
            if (_channels == 1)
            {
                _isMono = true;
            }
        }

        public void LowpassFilter(int cutoffFrequency)
        {
            NAudio.Dsp.BiQuadFilter lowpassFilter = NAudio.Dsp.BiQuadFilter.LowPassFilter(_sampleRate, cutoffFrequency, 1);
            double[] filteredSamples = new double[_numOfSamples];
            for (int i = 0; i < _numOfSamples; i++)
            {
                filteredSamples[i] = lowpassFilter.Transform(Convert.ToSingle(_sampleArray[i]));
            }
            _sampleArray = filteredSamples;
            _lowpassFiltered = true;
        }

        public void ToMono()
        {
            if (!_isMono) // Don't convert if samples are already mono
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
                _sampleArray = monoSamples;
                _numOfSamples = monoLength;
                _channels = 1;
                _isMono = true;
            }
        }

        public void DownSample(int cutoffFrequency)
        {
            int downSamplingFactor = _sampleRate / 2 / cutoffFrequency - 1;
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
            _sampleArray = downSampled;
            _sampleRate = newSamplerate;
            _downSampled = true;
        }

        public FramedMusicFileSamples CreateFramedMusicFileSamples(Window window = null)
        {
            return new FramedMusicFileSamples(this, window);
        }
    }
}
