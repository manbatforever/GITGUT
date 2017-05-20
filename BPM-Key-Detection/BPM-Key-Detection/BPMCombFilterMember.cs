using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    internal class BPMCombFilterMember
    {
        private int _bpm;
        private int[] _trainOfImpulses;
        private FrequencyBins _fftTrainOfImpulses;

        public BPMCombFilterMember(int bpm, int amountOfSamplesTested, int maxAmplitude, int sampleRate)
        {
            _bpm = bpm;
            _trainOfImpulses = CreateTrainOfImpulses(maxAmplitude, amountOfSamplesTested, sampleRate);
            _fftTrainOfImpulses = CalculateFFTTrainOfImpulses(amountOfSamplesTested);
        }

        //Calculates the period between impulses.
        private int CalculatePeriodBetweenImpulses(int sampleRate)
        {
            return Convert.ToInt32(60 / Convert.ToDouble(BPM) * Convert.ToDouble(sampleRate));
        }

        //Creates the train of impulses.
        private int[] CreateTrainOfImpulses(int maxAmplitude, int amountOfSamplesTested, int sampleRate)
        {
            int[] trainOfImpulses = new int[amountOfSamplesTested];
            int periodBetweenImpulses = CalculatePeriodBetweenImpulses(sampleRate);

                for (int k = 0; k < amountOfSamplesTested; k += periodBetweenImpulses)
                {
                    trainOfImpulses[k] = maxAmplitude;
                }
            return trainOfImpulses;
        }

        //Runs FFT on each element in the train of impulses.
        private FrequencyBins CalculateFFTTrainOfImpulses(int amountOfSamplesTested)
        {
            Complex[] ComplexTrainOfImpulsesSignal = new Complex[amountOfSamplesTested];

            for (int k = 0; k < amountOfSamplesTested; k++)
            {
                ComplexTrainOfImpulsesSignal[k] = new Complex(TrainOfImpulses[k], TrainOfImpulses[k]);
            }
            return new FFT(ComplexTrainOfImpulsesSignal).FrequencyBins;
        }

        public int BPM { get => _bpm; }
        public int[] TrainOfImpulses { get => _trainOfImpulses; }
        public FrequencyBins FFTTrainOfImpulses { get => _fftTrainOfImpulses; }
    }
}
