using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class ChromaVector
    {
        private double[][] _toneAmplitudes;
        private int _tonesTotal;
        private int _tonesPerOctave;
        private int _numOfOctaves;
        private double[] _vectorValues;

        public double[] VectorValues { get => _vectorValues; set => _vectorValues = value; }

        public ChromaVector(double[][] toneAmplitudes, int tonesTotal, int tonesPerOctave)
        {
            _toneAmplitudes = toneAmplitudes;
            _tonesTotal = tonesTotal;
            _tonesPerOctave = tonesPerOctave;
            _numOfOctaves = tonesTotal / tonesPerOctave;

            GetVectorValues(toneAmplitudes);
        }
        public ChromaVector(double[] singleFrame, int tonesTotal, int tonesPerOctave)
        {
            _tonesTotal = tonesTotal;
            _tonesPerOctave = tonesPerOctave;
            _numOfOctaves = tonesTotal / tonesPerOctave;

            _vectorValues = GetVectorValues(singleFrame);
        }

        private void GetVectorValues(double[][] toneAmplitudes) //For multiple frames
        {
            int numOfFrames = toneAmplitudes.Length;
            _vectorValues = new double[_tonesPerOctave];
            for (int frame = 0; frame < numOfFrames; frame++)
            {
                double[] tempSingleFrameChromaVector = GetVectorValues(toneAmplitudes[frame]);
                for (int element = 0; element < _tonesPerOctave; element++)
                {
                    _vectorValues[element] += tempSingleFrameChromaVector[element];
                }
            }
        }
        private double[] GetVectorValues(double[] singleFrame) // For single frame
        {
            double[] chromaVector = new double[_tonesPerOctave];
            for (int octave = 0; octave < _numOfOctaves; octave++)
            {
                for (int tone = 0; tone < _tonesPerOctave; tone++)
                {
                    int f = tone + octave * _tonesPerOctave;
                    chromaVector[tone] += singleFrame[tone + octave * _tonesPerOctave];
                }
            }
            return chromaVector;
        }

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
