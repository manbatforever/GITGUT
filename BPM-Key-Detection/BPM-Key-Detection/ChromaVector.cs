using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Represents an array of tone amplitudes for each target tone.
    internal class ChromaVector
    {
        private double[] _vectorValues;
        private int _tonesPerOctave;
        private int _numOfOctaves;

        public double[] VectorValues { get => _vectorValues; set => _vectorValues = value; }

        public ChromaVector(FramedToneAmplitudes multiFrameToneAmplitudes, int tonesPerOctave, int numOfOctaves) //For multiple frames
        {
            _tonesPerOctave = tonesPerOctave;
            _numOfOctaves = numOfOctaves;
            CalculateMultiFrameVectorValues(multiFrameToneAmplitudes);
        }

        public ChromaVector(double[] singleFrameToneAmplitudes, int tonesPerOctave, int numOfOctaves) //For single frame
        {
           _vectorValues = CalculateSingleFrameVectorValues(singleFrameToneAmplitudes);
        }

        private void CalculateMultiFrameVectorValues(FramedToneAmplitudes multiFrameToneAmplitudes)
        {
            int numOfFrames = multiFrameToneAmplitudes.ToneAmplitudeValues.Length;
            _vectorValues = new double[_tonesPerOctave];
            for (int frame = 0; frame < numOfFrames; frame++)
            {
                double[] tempSingleFrameChromaVector = CalculateSingleFrameVectorValues(multiFrameToneAmplitudes.ToneAmplitudeValues[frame]);
                for (int element = 0; element < _tonesPerOctave; element++)
                {
                    _vectorValues[element] += tempSingleFrameChromaVector[element];
                }
            }
        }

        private double[] CalculateSingleFrameVectorValues(double[] singleFrameToneAmplitudes)
        {
            double[] chromaVector = new double[_tonesPerOctave];
            for (int octave = 0; octave < _numOfOctaves; octave++)
            {
                for (int tone = 0; tone < _tonesPerOctave; tone++)
                {
                    int f = tone + octave * _tonesPerOctave;
                    chromaVector[tone] += singleFrameToneAmplitudes[f]; // * _octaveWeights[octave]
                }
            }
            return chromaVector;
        }

        private double DotProduct(double[] v2)
        {
            int length = _vectorValues.Length;
            double output = 0;
            for (int i = 0; i < length; i++)
            {
                output += _vectorValues[i] * v2[i];
            }
            return output;
        }

        private double Magnitude(double[] Vector, int length)
        {
            double output = 0;
            for (int i = 0; i < length; i++)
            {
                output += Math.Pow(Vector[i], 2d);
            }
            return Math.Sqrt(output);
        }

        public double CosineSimilarity(double[] V2)
        {
            int length = _vectorValues.Length;
            return DotProduct(V2) / (Magnitude(_vectorValues, length) * Magnitude(V2, length));
        }
        //private double[] _octaveWeights = new double[]
        //{
        //    0.39997267549999998559,
        //    0.55634425248300645173,
        //    0.52496636345143543600,
        //    0.60847548384277727607,
        //    0.59898115679999996974,
        //    0.49072435317960994006,
        //};
    }
}
