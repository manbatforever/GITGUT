﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BPM_Key_Detection
{   //WARNING: This class is outdated and not used (or compiled) as of this version.
    //Object: A temporal kernel used to calculate CQT
    internal class TemporalKernel : Kernel
    {
        private TemporalKernelSamples _temporalKernelSamples;

        private int _kernelNumber;

        internal TemporalKernelSamples TemporalKernelSamples { get => _temporalKernelSamples; }

        public TemporalKernel(double samplerate, int tonesPerOctave, int tonesTotal, int samplesPerFrame, double minimumFrequency, int kernelNumber) :
            base(samplerate, tonesPerOctave, tonesTotal, samplesPerFrame, minimumFrequency)
        {
            _kernelNumber = kernelNumber;
            _tonesPerOctave = tonesPerOctave;
            _samplesPerFrame = samplesPerFrame;
            _Q = 1d / (Math.Pow(2d, 1d / _tonesPerOctave) - 1d);
            _temporalKernelSamples = new TemporalKernelSamples(GetSingleTemporalKernel(kernelNumber));
        }

        private double[] GetSingleTemporalKernel(int k_cq)
        {
            double[] SingleTemporalKernel = new double[_samplesPerFrame];
            for (int element = 0;element < _samplesPerFrame; element++)
            {
                if (element < ((_samplesPerFrame / 2d) + (WindowLength(k_cq) / 2d)) && element > ((_samplesPerFrame / 2d) - (WindowLength(k_cq) / 2d)))
                {
                    SingleTemporalKernel[element] = CalculateSingleTemporalKernelElement(element, k_cq).Real;
                }
            }
            return SingleTemporalKernel;
        }

        private Complex CalculateSingleTemporalKernelElement(double n, int k_cq)
        {
            Complex c1 = Complex.Exp(new Complex(0, 2d * Math.PI * n * _Q / WindowLength(k_cq))); // Brown lign. (5)
            return HammingFunction(n - ((_samplesPerFrame / 2d) - (WindowLength(k_cq) / 2d)), k_cq) * c1; // Brown og Puckette lign (4)
        }

        private double HammingFunction(double n, int k_cq)
        {
            double alpha = 25d / 46d;
            return alpha - (1d - alpha) * Math.Cos(2d * Math.PI * n / WindowLength(k_cq)); // Hamming window function
        }

        private double WindowLength(int k_cq)
        {
            return _Q * _samplerate / _frequenciesOfInterest[k_cq];
        }
    }
}
