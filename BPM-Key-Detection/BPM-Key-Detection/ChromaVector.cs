using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class ChromaVector
    {
        private SingleFrameToneAmplitudes[] _toneAmplitudes;
        private double[] _vectorValues;

        public double[] VectorValues { get => _vectorValues; set => _vectorValues = value; }

        public ChromaVector(SingleFrameToneAmplitudes[] toneAmplitudes)
        {
            _toneAmplitudes = toneAmplitudes;
            GetVectorValues(toneAmplitudes);
        }
        public ChromaVector(SingleFrameToneAmplitudes singleFrameToneAmplitudes)
        {
            _vectorValues = GetVectorValues(singleFrameToneAmplitudes);
        }

        private void GetVectorValues(SingleFrameToneAmplitudes[] toneAmplitudes) //For multiple frames
        {
            int numOfFrames = toneAmplitudes.Length;
            _vectorValues = new double[Transformations.TonesPerOctave];
            for (int frame = 0; frame < numOfFrames; frame++)
            {
                double[] tempSingleFrameChromaVector = GetVectorValues(toneAmplitudes[frame]);
                for (int element = 0; element < Transformations.TonesPerOctave; element++)
                {
                    _vectorValues[element] += tempSingleFrameChromaVector[element];
                }
            }
        }
        private double[] GetVectorValues(SingleFrameToneAmplitudes singleFrameToneAmplitudes) // For single frame
        {
            double[] chromaVector = new double[Transformations.TonesPerOctave];
            for (int octave = 0; octave < Transformations.NumOfOctaves; octave++)
            {
                for (int tone = 0; tone < Transformations.TonesPerOctave; tone++)
                {
                    int f = tone + octave * Transformations.TonesPerOctave;
                    chromaVector[tone] += singleFrameToneAmplitudes.ToneAmplitudeValues[tone + octave * Transformations.TonesPerOctave] * _octaveWeights[octave];
                }
            }
            return chromaVector;
        }

        private double[] _octaveWeights = new double[]
        {
            0.39997267549999998559,
            0.55634425248300645173,
            0.52496636345143543600,
            0.60847548384277727607,
            0.59898115679999996974,
            0.49072435317960994006,
        };

        //private double[] CreateMultiFrameChromaVector(double[][] toneAmplitudes) //Gammel metode
        //{
        //    int numOfFrames = toneAmplitudes.Length;
        //    double[] chromaVector = new double[_tonesPerOctave];
        //    for (int frame = 0; frame < numOfFrames; frame++)
        //    {
        //        double[] tempSingleFrameChromaVector = CreateSingleFrameChromaVector(toneAmplitudes[frame]);
        //        for (int element = 0; element < _tonesPerOctave; element++)
        //        {
        //            chromaVector[element] += tempSingleFrameChromaVector[element];
        //        }
        //    }
        //    return chromaVector;
        //}

        //public double[] GetSingleFrameChromaVector(int frameNumber) //Gammel metode
        //{
        //    return CreateSingleFrameChromaVector(_toneAmplitudes[frameNumber]);
        //}

        //private double[] CreateSingleFrameChromaVector(double[] singleFrame)
        //{
        //    double[] chromaVector = new double[_tonesPerOctave];
        //    for (int tone = 0; tone < _tonesPerOctave; tone++)
        //    {
        //        for (int octave = 0; octave < _numOfOctaves; octave++)
        //        {
        //            chromaVector[tone] += singleFrame[tone + octave * _tonesPerOctave];
        //        }
        //    }
        //    return chromaVector;
        //}

    }
}
