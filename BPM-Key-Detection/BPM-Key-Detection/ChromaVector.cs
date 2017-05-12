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

        public ChromaVector(double[][] toneAmplitudes, int tonesTotal, int tonesPerOctave)
        {
            _toneAmplitudes = toneAmplitudes;
            _tonesTotal = tonesTotal;
            _tonesPerOctave = tonesPerOctave;
            _numOfOctaves = tonesTotal / tonesPerOctave;
        }

        

        private double[] CreateChromaVectorForMultipleFrames(double[][] toneAmplitudes)
        {
            int numOfFrames = toneAmplitudes.Length;
            double[] chromaVector = new double[_tonesPerOctave];
            for (int frame = 0; frame < numOfFrames; frame++)
            {
                double[] tempSingleFrameChromaVector = CreateChromaVectorForSingleFrame(toneAmplitudes[frame]);
                for (int element = 0; element < _tonesPerOctave; element++)
                {
                    chromaVector[element] += tempSingleFrameChromaVector[element];
                }
            }
            return chromaVector;
        }

        private double[] CreateChromaVectorForSingleFrame(double[] singleFrame)
        {
            double[] chromaVector = new double[_tonesPerOctave];
            for (int tone = 0; tone < _tonesPerOctave; tone++)
            {
                for (int octave = 0; octave < _numOfOctaves; octave++)
                {
                    chromaVector[tone] += singleFrame[tone + octave * _tonesPerOctave];
                }
            }
            return chromaVector;
        }
    }
}
