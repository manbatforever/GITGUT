using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Contains all Constant Q transform values including output and all functionality
    internal class CQT
    {
        private int _cutoffFrequency = KeyEstimationLogs.CutoffFrequency;
        private int _samplesPerFrame = KeyEstimationLogs.SamplesPerFrame;
        private int _hopsPerFrame = KeyEstimationLogs.HopsPerFrame;
        private int _tonesPerOctave = KeyEstimationLogs.TonesPerOctave;
        private int _numOfOctaves = KeyEstimationLogs.NumberOfOctaves;
        private int _tonesTotal;
        private double _minimumFrequency = KeyEstimationLogs.MinimumFrequency;
        private FramedToneAmplitudes _framedToneAmplitudes;
        
        public CQT(MusicFileSamples musicFileSamples)
        {
            _tonesTotal = _tonesPerOctave * _numOfOctaves;
            ProcesMusicFileSamples(musicFileSamples);

            FramedMusicFileSamples framedMusicFileSamples = new FramedMusicFileSamples(musicFileSamples, _samplesPerFrame, _hopsPerFrame, new BlackmanWindow(SamplesPerFrame));

            FFT fft = new FFT(framedMusicFileSamples);

            IbrahimSpectralKernel ibrahimSpectralKernel = new IbrahimSpectralKernel(musicFileSamples.Samplerate, _tonesPerOctave, _tonesTotal, _samplesPerFrame, _minimumFrequency);

            _framedToneAmplitudes = new FramedToneAmplitudes(fft.FramedFrequencyBins, ibrahimSpectralKernel, _tonesTotal, _samplesPerFrame);
        }

        //Proces input samples to reduce runtime without discarding useful information
        private void ProcesMusicFileSamples(MusicFileSamples musicFileSamples)
        {
            musicFileSamples.ToMono();
            musicFileSamples.LowpassFilter(_cutoffFrequency);
            musicFileSamples.DownSample(_cutoffFrequency);
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
