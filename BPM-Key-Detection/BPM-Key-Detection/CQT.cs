using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class CQT
    {
        private int _cutoffFrequency;
        private int _samplesPerFrame;
        private int _hopsPerFrame;
        private int _tonesPerOctave;
        private int _numOfOctaves;
        private int _tonesTotal;
        private double _minimumFrequency;
        private FramedToneAmplitudes _framedToneAmplitudes;




        public CQT(MusicFileSamples musicFileSamples)
        {
            _cutoffFrequency = 2000;
            _samplesPerFrame = 16384;
            _hopsPerFrame = 4;
            _tonesPerOctave = 12;
            _numOfOctaves = 6;
            _tonesTotal = _tonesPerOctave * _numOfOctaves;
            _minimumFrequency = 27.5;

            musicFileSamples.ToMono();
            musicFileSamples.LowpassFilter(_cutoffFrequency);
            musicFileSamples.DownSample(_cutoffFrequency);
            FramedMusicFileSamples framedMusicFileSamples = new FramedMusicFileSamples(musicFileSamples, new BlackmanWindow());

            FFT fft = new FFT(framedMusicFileSamples);
            
            IbrahimSpectralKernel ibrahimSpectralKernel = new IbrahimSpectralKernel(musicFileSamples.Samplerate);

            _framedToneAmplitudes = new FramedToneAmplitudes(fft.FramedFrequencyBins, ibrahimSpectralKernel);
        }


        public int CutoffFrequency { get => _cutoffFrequency; set => _cutoffFrequency = value; }
        public int SamplesPerFrame { get => _samplesPerFrame; set => _samplesPerFrame = value; }
        public int HopsPerFrame { get => _hopsPerFrame; set => _hopsPerFrame = value; }
        public int TonesPerOctave { get => _tonesPerOctave; set => _tonesPerOctave = value; }
        public int NumOfOctaves { get => _numOfOctaves; set => _numOfOctaves = value; }
        public int TonesTotal { get => _tonesTotal; set => _tonesTotal = value; }
        public double MinimumFrequency { get => _minimumFrequency; set => _minimumFrequency = value; }
        internal FramedToneAmplitudes FramedToneAmplitudes { get => _framedToneAmplitudes; set => _framedToneAmplitudes = value; }
    }
}
