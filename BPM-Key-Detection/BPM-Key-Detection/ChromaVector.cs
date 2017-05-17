using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Represents an array of tone amplitudes for each target tone.
    class ChromaVector
    {
        private double[] _vectorValues;

        public double[] VectorValues { get => _vectorValues; set => _vectorValues = value; }

        public ChromaVector(FramedToneAmplitudes toneAmplitudes)
        {
            CalculateVectorValues(toneAmplitudes);
        }

        private void CalculateVectorValues(FramedToneAmplitudes toneAmplitudes) //For multiple frames
        {
            int numOfFrames = toneAmplitudes.ToneAmplitudeValues.Length;
            _vectorValues = new double[Transformations.TonesPerOctave];
            for (int frame = 0; frame < numOfFrames; frame++)
            {
                double[] tempSingleFrameChromaVector = CalculateVectorValues(toneAmplitudes.ToneAmplitudeValues[frame]);
                for (int element = 0; element < Transformations.TonesPerOctave; element++)
                {
                    _vectorValues[element] += tempSingleFrameChromaVector[element];
                }
            }
        }

        private double[] CalculateVectorValues(double[] singleFrameToneAmplitudes) // For single frame
        {
            double[] chromaVector = new double[Transformations.TonesPerOctave];
            for (int octave = 0; octave < Transformations.NumOfOctaves; octave++)
            {
                for (int tone = 0; tone < Transformations.TonesPerOctave; tone++)
                {
                    int f = tone + octave * Transformations.TonesPerOctave;
                    chromaVector[tone] += singleFrameToneAmplitudes[tone + octave * Transformations.TonesPerOctave] * _octaveWeights[octave];
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
    }
}
