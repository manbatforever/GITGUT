using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class SongNotLongEnoughException : Exception
    {
        public SongNotLongEnoughException() : base("The song does not have enough samples to be analyzed.") { }
        public SongNotLongEnoughException(string songName) : base($"The song {songName} does not have enough samples to be analyzed.") { }
    }
    class SongIsNotStereoException : Exception
    {
        public SongIsNotStereoException() : base("The song is not stereo, and can not be analysed.") { }
        public SongIsNotStereoException(string songName) : base($"The song {songName} is not stereo, and can not be analysed.") { }
    }
    //This class is able to compute the BPM of a song
    static class BPMDetector
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

        //Starts methods to compute the BPM and returns the computed BPM.
        public static int GetBPM(int sampleRate, MusicFileSamples musicFileSamples)
        {
            if (musicFileSamples.Channels != 2)
            {
                throw new SongIsNotStereoException();
            }
            int amountOfSamplesTested = sampleRate * _amountOfSecondsTested;
            MusicFileSamples leftChannel = FillLeftChannel(musicFileSamples, amountOfSamplesTested, sampleRate);
            MusicFileSamples rightChannel = FillRightChannel(musicFileSamples, amountOfSamplesTested, sampleRate);
            DerivationFilter(leftChannel, rightChannel, sampleRate, amountOfSamplesTested);
            Complex[] fftetDerivationFilteredSamples = GetFFTDerivationFilteredSamples(leftChannel, rightChannel, amountOfSamplesTested);

            BPMCorrelationFilterMember[] BPMCorrelationFilter = InitializeAllCorrelationMembers(sampleRate, amountOfSamplesTested);
            double[] similarityEnergies = ComputeAllSimilarityEnergies(fftetDerivationFilteredSamples, BPMCorrelationFilter, amountOfSamplesTested);
            return ComputeBPM(similarityEnergies, BPMCorrelationFilter);
        }

        private static MusicFileSamples FillLeftChannel(MusicFileSamples musicFileSamples, int amountOfSamplesTested, int sampleRate)
        {
            double[][] splitStereoSamples = SplitSamples(musicFileSamples);
            //Checks if there are enough samples to be processed.
            if (splitStereoSamples[0].Length <= amountOfSamplesTested || splitStereoSamples[1].Length <= amountOfSamplesTested)
            {
                throw new SongNotLongEnoughException();
            }
            int startSample = Convert.ToInt32((splitStereoSamples[0].Length - sampleRate * _amountOfSecondsTested) / 2);
            double[] leftChannel = new double[amountOfSamplesTested];
            for (int i = startSample, k = 0; k < amountOfSamplesTested; i++, k++)
            {
                leftChannel[k] = splitStereoSamples[0][i];
            }

            return new MusicFileSamples(leftChannel, sampleRate, 1);
        }

        private static MusicFileSamples FillRightChannel(MusicFileSamples musicFileSamples, int amountOfSamplesTested, int sampleRate)
        {
            double[][] splitStereoSamples = SplitSamples(musicFileSamples);
            //Checks if there are enough samples to be processed.
            if (splitStereoSamples[0].Length <= amountOfSamplesTested || splitStereoSamples[1].Length <= amountOfSamplesTested)
            {
                throw new SongNotLongEnoughException();
            }
            int startSample = Convert.ToInt32((splitStereoSamples[0].Length - sampleRate * _amountOfSecondsTested) / 2);
            double[] rightChannel = new double[amountOfSamplesTested];
            for (int i = startSample, k = 0; k < amountOfSamplesTested; i++, k++)
            {
                rightChannel[k] = splitStereoSamples[1][i];
            }
            return new MusicFileSamples(rightChannel, sampleRate, 1);
        }

        private static double[][] SplitSamples(MusicFileSamples samplesStereo)
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
        private static void DerivationFilter(MusicFileSamples leftChannel, MusicFileSamples rightChannel, int sampleRate, int amountOfSamplesTested)
        {
            for (int k = 0; k < amountOfSamplesTested - 1; k++)
            {
                leftChannel.SampleArray[k] = sampleRate * (leftChannel.SampleArray[k + 1] - leftChannel.SampleArray[k]);
            }
            leftChannel.SampleArray[amountOfSamplesTested - 1] = 0;

            for (int k = 0; k < amountOfSamplesTested - 1; k++)
            {
                rightChannel.SampleArray[k] = sampleRate * (rightChannel.SampleArray[k + 1] - rightChannel.SampleArray[k]);
            }
            rightChannel.SampleArray[amountOfSamplesTested - 1] = 0;
        }

        //Passes the differentiated samples to the frequency domain.
        private static Complex[] GetFFTDerivationFilteredSamples(MusicFileSamples leftChannel, MusicFileSamples rightChannel, int amountOfSamplesTested)
        {
            Complex[] complexSignal = new Complex[amountOfSamplesTested];
            
            for (int k = 0; k < amountOfSamplesTested; k++)
            {
                complexSignal[k] = new Complex(leftChannel.SampleArray[k], rightChannel.SampleArray[k]);
            }
            return Transformations.FFT(complexSignal);
        }

        private static BPMCorrelationFilterMember[] InitializeAllCorrelationMembers(int sampleRate, int amountOfSamplesTested)
        {
            BPMCorrelationFilterMember[] BPMCorrelationFilter = new BPMCorrelationFilterMember[_amountOfBpmsToTest];
            for (int i = 0, bpm = _startBpmToTest; i < _amountOfBpmsToTest; i++, bpm += _bpmDivisor)
            {
                BPMCorrelationFilter[i] = new BPMCorrelationFilterMember(bpm, amountOfSamplesTested, _maxAmplitude, sampleRate);
            }
            return BPMCorrelationFilter;
        }

        //Computes and inserts the energys which give an evaluatin of the similarity, between the train of impulses and the song.
        //The larger the energy, the larger the similarity.
        private static double[] ComputeAllSimilarityEnergies(Complex[] fftetDerivationFilteredSamples, BPMCorrelationFilterMember[] BPMCorrelationFilter, 
                                                             int amountOfSamplesTested)
        {
            double[] similarityEnergies = new double[_amountOfBpmsToTest];
            for (int i = 0; i < _amountOfBpmsToTest; i++)
            {
                similarityEnergies[i] = 0;

                for (int k = 0; k < amountOfSamplesTested; k++)
                {
                    similarityEnergies[i] += Complex.Abs(fftetDerivationFilteredSamples[k] * BPMCorrelationFilter[i].FFTTrainOfImpulses[k]);
                }
            }
            return similarityEnergies;
        }

        //Finds the tested BPM with the greatest energy
        private static int ComputeBPM(double[] similarityEnergies, BPMCorrelationFilterMember[] BPMCorrelationFilter)
        {
            double GreatestEnergy = 0;
            int IndexOfBPMWithGreatestEnergy = 0;
            for (int i = 0; i < _amountOfBpmsToTest; i++)
            {
                if (similarityEnergies[i] > GreatestEnergy && (GreatestEnergy + 0.2) < similarityEnergies[i])
                {
                    GreatestEnergy = similarityEnergies[i];
                    IndexOfBPMWithGreatestEnergy = i;
                }
            }
            return BPMCorrelationFilter[IndexOfBPMWithGreatestEnergy].BPM;
        }
    }
}
