using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BPM_Key_Detection
{
    class Kernel
    {
        private double _Q;

        private double _samplerate;
        private double _baseFrequency;
        private double _binsPerOctave;
        private double _binsTotal;
        private double _samplesPerFrame;
        private double[][] _allTemporalKernels;
        private Complex[][] _allSpectralKernels;

        public double Samplerate { get => _samplerate; }
        public double BaseFrequency { get => _baseFrequency; }
        public double BinsPerOctave { get => _binsPerOctave;  }
        public double BinsTotal { get => _binsTotal; }
        public double SamplesPerFrame { get => _samplesPerFrame; }
        public double[][] AllTemporalKernels { get => _allTemporalKernels; }
        public Complex[][] AllSpectralKernels { get => _allSpectralKernels; }

        public Kernel(double samplerate, double baseFrequency, double binsPerOctave, double octaves, double samplesPerFrame)
        {
            _samplerate = samplerate;
            _baseFrequency = baseFrequency;
            _binsPerOctave = binsPerOctave;
            _binsTotal = octaves * binsPerOctave;
            _Q = 1d / (Math.Pow(2d, 1d / binsPerOctave) - 1d); // Constant Q
            _samplesPerFrame = samplesPerFrame; // NextPowerOf2(WindowLength(0)); // For at være sikker på at få hele den længste temporal kernel med.
            Start();
        }

        public void Start()
        {
            _allTemporalKernels = GetAllTemporalKernels();
            _allSpectralKernels = GetAllSpectralKernels(_allTemporalKernels);
        }

        private Complex[][] GetAllSpectralKernels(double[][] AllTemporalKernels)
        {
            int numOfKernels = AllTemporalKernels.Length;
            Complex[][] allSpectralKernels = new Complex[numOfKernels][];
            for (int kernel = 0; kernel < numOfKernels; kernel++)
            {
                Complex[] spectralKernel = FastFourierTransform.FFT(AllTemporalKernels[kernel]);
                for (int i = 0; i < _samplesPerFrame; i++)
                {
                    if (spectralKernel[i].Magnitude < 0.1)
                    {
                        spectralKernel[i] = 0;
                    }
                }
                allSpectralKernels[kernel] = spectralKernel;
            }
            return allSpectralKernels;
        }

        private double[][] GetAllTemporalKernels()
        {
            double[][] AllTemporalKernels = new double[(int)_binsTotal][];
            for (int k_cq = 0; k_cq < _binsTotal; k_cq++)
            {
                AllTemporalKernels[k_cq] = GetSingleTemporalKernel(k_cq);
            }
            return AllTemporalKernels;
        }

        private double[] GetSingleTemporalKernel(double k_cq)
        {
            double[] SingleTemporalKernel = new double[(int)_samplesPerFrame];
            for (int element = 0;element < _samplesPerFrame; element++)
            {
                if (element < ((_samplesPerFrame / 2d) + (WindowLength(k_cq) / 2d)) && element > ((_samplesPerFrame / 2d) - (WindowLength(k_cq) / 2d)))
                {
                    SingleTemporalKernel[element] = CalculateSingleTemporalKernelElement(element, k_cq).Real;
                }
            }
            return SingleTemporalKernel;
        }

        private Complex CalculateSingleTemporalKernelElement(double n, double k_cq)
        {
            Complex c1 = Complex.Exp(new Complex(0, 2d * Math.PI * n * _Q / WindowLength(k_cq))); // Brown lign. (5)
            return HammingFunction(n - ((_samplesPerFrame / 2d) - (WindowLength(k_cq) / 2d)), k_cq) * c1; // Brown og Puckette lign (4)
        }

        private double HammingFunction(double n, double k_cq)
        {
            //double alpha = 25d / 46d;
            //return alpha - (1d - alpha) * Math.Cos(2d * Math.PI * n / WindowLength(k_cq)); // Hamming window function
            return 0.5d * (1d - Math.Cos(2d * Math.PI * n / WindowLength(k_cq))); // Hann window function
        }

        private double WindowLength(double k_cq)
        {
            return _Q * _samplerate / CenterFrequency(k_cq);
        }

        private double CenterFrequency(double k_cq)
        {
            return Math.Pow(Math.Pow(2d, 1d / _binsPerOctave), k_cq) * _baseFrequency;
        }

        private int NextPowerOf2(double input)
        {
            int power = 0;
            int result = 0;
            do
            {
                result = (int)Math.Pow(2, power);
                power++;
            } while (result <= input);
            return result;
        }
    }
}
