using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class IbrahimSpectralKernel
    {
        private double _Q;
        private double _samplerate;
        private double _baseFrequency;
        private double _binsPerOctave;
        private double _binsTotal;
        private double _samplesPerFrame;

        public IbrahimSpectralKernel(double samplerate, double baseFrequency, double binsPerOctave, double octaves, double samplesPerFrame)
        {
            _samplerate = samplerate;
            _baseFrequency = baseFrequency;
            _binsPerOctave = binsPerOctave;
            _binsTotal = octaves * binsPerOctave;
            _samplesPerFrame = samplesPerFrame;
            _Q = 0.8d * (1d / (Math.Pow(2d, 1d / binsPerOctave) - 1d));
        }


        private double WindowLength(double k_cq)
        {
            return CenterFrequency(k_cq) * _samplesPerFrame / _samplerate;
        }

        private double CenterFrequency(double k_cq)
        {
            return Math.Pow(Math.Pow(2d, 1d / _binsPerOctave), k_cq) * _baseFrequency;
        }

        private double HammingFunction(double n, double k_cq)
        {
            double alpha = 25d / 46d;
            return alpha - (1d - alpha) * Math.Cos(2d * Math.PI * n / WindowLength(k_cq)); // Hamming window function
            //return 0.5d * (1d - Math.Cos(2d * Math.PI * n / WindowLength(k_cq))); // Hann window function
        }
    }
}
