using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class SingleFrameChromaVector : AbstractChromaVector
    {
        public SingleFrameChromaVector(double[][] toneAmplitudes, int tonesTotal, int tonesPerOctave) : base(toneAmplitudes, tonesTotal, tonesPerOctave)
        {
        }

        protected override void CreateVector()
        {
            throw new NotImplementedException();
        }

        //protected override void CreateVector(double[] singleFrame)
        //{
        //    double[] chromaVector = new double[_tonesPerOctave];
        //    for (int tone = 0; tone < _tonesPerOctave; tone++)
        //    {
        //        for (int octave = 0; octave < _numOfOctaves; octave++)
        //        {
        //            chromaVector[tone] += singleFrame[tone + octave * _tonesPerOctave];
        //        }
        //    }

        //}
    }
}
