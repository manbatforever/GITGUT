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
        public int BPM { get; private set; }
        public int[] TrainOfImpulses { get; private set; }
        public Complex[] FFTTrainOfImpulses { get; private set; }

        public BPMCombFilterMember(int bpm, int amountOfSamplesTested, int maxAmplitude, int sampleRate)
        {
            BPM = bpm;
            TrainOfImpulses = ComputeTrainOfImpulses(maxAmplitude, amountOfSamplesTested, sampleRate);
            FFTTrainOfImpulses = ComputeFFTTrainOfImpulses(amountOfSamplesTested);
        }

        //Computes the period between impulses.
        private int CalculatePeriodeBetweenImpulses(int sampleRate)
        {
            return Convert.ToInt32(60 / Convert.ToDouble(BPM) * Convert.ToDouble(sampleRate));
        }

        //Computes the train of impulses.
        private int[] ComputeTrainOfImpulses(int maxAmplitude, int amountOfSamplesTested, int sampleRate)
        {
            int[] trainOfImpulses = new int[amountOfSamplesTested];
            int periodBetweenImpulses = CalculatePeriodeBetweenImpulses(sampleRate);

            for (int i = 0; i < amountOfSamplesTested; i++)
            {
                for (int k = 0; k < amountOfSamplesTested; k += periodBetweenImpulses)
                {
                    trainOfImpulses[k] = maxAmplitude;
                }
            }
            return trainOfImpulses;
        }

        //Runs FFT on each element in the train of impulses.
        private Complex[] ComputeFFTTrainOfImpulses(int amountOfSamplesTested)
        {
            Complex[] ComplexTrainOfImpulsesSignal = new Complex[amountOfSamplesTested];

            for (int k = 0; k < amountOfSamplesTested; k++)
            {
                ComplexTrainOfImpulsesSignal[k] = new Complex(TrainOfImpulses[k], TrainOfImpulses[k]);
            }
            return new FFT(ComplexTrainOfImpulsesSignal).FrequencyBins;
        }
    }
}
