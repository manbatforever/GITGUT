using System;
using System.Numerics;

namespace BPM_Key_Detection
{

    //Contains all data and functionality regarding BPM estimation
    internal class BPMEstimator
    {
        //The program assumes that the BPM of the song is between _startBpmToTest and _endBpmToTest.
        //The results is between (including) those two values.
        private static readonly int _startBpmToTest = 60;
        private static readonly int _endBpmToTest = 180;
        private static readonly int _bpmDivisor = 1; //Define how many BPMs we take per step. 1 is the most precise, as it takes every BPM available.
        private static readonly int _bpmEndAndStartDifference = _endBpmToTest - _startBpmToTest;
        private static readonly int _amountOfSecondsTested = 5;
        private static readonly int _amountOfBpmsToTest = _bpmEndAndStartDifference / _bpmDivisor;
        private static readonly int _maxAmplitude = 1;  //maxAplitude is 1 since NAudio converts all aplitudes to values between -1 and 1.

        private int _amountOfSamplesTested;
        private int _sampleRate;
        private MusicFileSamples _leftChannel;
        private MusicFileSamples _rightChannel;

        public uint EstimatedBPM { get; private set; }


        public BPMEstimator(int sampleRate, MusicFileSamples musicFileSamples)
        {
            if (musicFileSamples.Channels != 2)
            {
                throw new SongIsNotStereoException();
            }
            _sampleRate = sampleRate;
            _amountOfSamplesTested = sampleRate * _amountOfSecondsTested;
            _leftChannel = FillLeftChannel(musicFileSamples);
            _rightChannel = FillRightChannel(musicFileSamples);
            EstimatedBPM = (uint)GetBPM();
        }
        //Starts methods to compute the BPM and returns the computed BPM.
        private int GetBPM()
        {
            DerivationFilter(_leftChannel, _rightChannel);
            Complex[] fftetDerivationFilteredSamples = GetFFTDerivationFilteredSamples();
            BPMCombFilterMember[] BPMCombFilter = InitializeAllCombMembers();
            double[] similarityEnergies = CalculateAllSimilarityEnergies(fftetDerivationFilteredSamples, BPMCombFilter);
            return CombFilterProcess(similarityEnergies, BPMCombFilter);
        }

        private MusicFileSamples FillLeftChannel(MusicFileSamples musicFileSamples)
        {
            double[][] splitStereoSamples = SplitSamples(musicFileSamples);
            //Checks if there are enough samples to be processed.
            if (splitStereoSamples[0].Length <= _amountOfSamplesTested || splitStereoSamples[1].Length <= _amountOfSamplesTested)
            {
                throw new SongNotLongEnoughException();
            }
            int startSample = Convert.ToInt32((splitStereoSamples[0].Length - _sampleRate * _amountOfSecondsTested) / 2);
            double[] leftChannel = new double[_amountOfSamplesTested];
            for (int i = startSample, k = 0; k < _amountOfSamplesTested; i++, k++)
            {
                leftChannel[k] = splitStereoSamples[0][i];
            }

            return new MusicFileSamples(leftChannel, _sampleRate, 1);
        }

        private MusicFileSamples FillRightChannel(MusicFileSamples musicFileSamples)
        {
            double[][] splitStereoSamples = SplitSamples(musicFileSamples);
            //Checks if there are enough samples to be processed.
            if (splitStereoSamples[0].Length <= _amountOfSamplesTested || splitStereoSamples[1].Length <= _amountOfSamplesTested)
            {
                throw new SongNotLongEnoughException();
            }
            int startSample = Convert.ToInt32((splitStereoSamples[0].Length - _sampleRate * _amountOfSecondsTested) / 2);
            double[] rightChannel = new double[_amountOfSamplesTested];
            for (int i = startSample, k = 0; k < _amountOfSamplesTested; i++, k++)
            {
                rightChannel[k] = splitStereoSamples[1][i];
            }
            return new MusicFileSamples(rightChannel, _sampleRate, 1);
        }

        private double[][] SplitSamples(MusicFileSamples samplesStereo)
        {
            double[][] splitSamples = new double[samplesStereo.Channels][];

            for (int channel = 0; channel < samplesStereo.Channels; channel++)
            {
                splitSamples[channel] = new double[samplesStereo.SampleArray.Length / 2];
                for (int i = channel; i < samplesStereo.SampleArray.Length - 1; i += samplesStereo.Channels)
                {
                    splitSamples[channel][i / 2] = samplesStereo.SampleArray[i];
                }
            }
            return splitSamples;
        }

        //Makes the beats more detectable.
        private void DerivationFilter(MusicFileSamples _leftChannel, MusicFileSamples _rightChannel)
        {
            for (int k = 0; k < _amountOfSamplesTested - 1; k++)
            {
                _leftChannel.SampleArray[k] = _sampleRate * (_leftChannel.SampleArray[k + 1] - _leftChannel.SampleArray[k]);
            }
            _leftChannel.SampleArray[_amountOfSamplesTested - 1] = 0;

            for (int k = 0; k < _amountOfSamplesTested - 1; k++)
            {
                _rightChannel.SampleArray[k] = _sampleRate * (_rightChannel.SampleArray[k + 1] - _rightChannel.SampleArray[k]);
            }
            _rightChannel.SampleArray[_amountOfSamplesTested - 1] = 0;
        }

        //Passes the differentiated samples to the frequency domain.
        private Complex[] GetFFTDerivationFilteredSamples()
        {
            Complex[] complexSignal = new Complex[_amountOfSamplesTested];
            
            for (int k = 0; k < _amountOfSamplesTested; k++)
            {
                complexSignal[k] = new Complex(_leftChannel.SampleArray[k], _rightChannel.SampleArray[k]);
            }
            return new FFT(complexSignal).FrequencyBins;
        }

        private BPMCombFilterMember[] InitializeAllCombMembers()
        {
            BPMCombFilterMember[] BPMCombFilter = new BPMCombFilterMember[_amountOfBpmsToTest];
            for (int i = 0, bpm = _startBpmToTest; i < _amountOfBpmsToTest; i++, bpm += _bpmDivisor)
            {
                BPMCombFilter[i] = new BPMCombFilterMember(bpm, _amountOfSamplesTested, _maxAmplitude, _sampleRate);
            }
            return BPMCombFilter;
        }

        //Calculates and inserts the energys which give an evaluatin of the similarity, between the train of impulses and the song.
        //The larger the energy, the larger the similarity.
        private double[] CalculateAllSimilarityEnergies(Complex[] fftetDerivationFilteredSamples, BPMCombFilterMember[] BPMCombFilter)
        {
            double[] similarityEnergies = new double[_amountOfBpmsToTest];
            for (int i = 0; i < _amountOfBpmsToTest; i++)
            {
                similarityEnergies[i] = 0;

                for (int k = 0; k < _amountOfSamplesTested; k++)
                {
                    similarityEnergies[i] += Complex.Abs(fftetDerivationFilteredSamples[k] * BPMCombFilter[i].FFTTrainOfImpulses[k]);
                }
            }
            return similarityEnergies;
        }

        //Finds the tested BPM with the greatest energy
        private int CombFilterProcess(double[] similarityEnergies, BPMCombFilterMember[] BPMCombFilter)
        {
            double GreatestEnergy = 0;
            int IndexOfBPMWithGreatestEnergy = 0;
            for (int i = 0; i < _amountOfBpmsToTest; i++)
            {
                if (similarityEnergies[i] > GreatestEnergy)
                {
                    GreatestEnergy = similarityEnergies[i];
                    IndexOfBPMWithGreatestEnergy = i;
                }
            }
            return BPMCombFilter[IndexOfBPMWithGreatestEnergy].BPM;
        }
    }
}