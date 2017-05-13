using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    class MultiFrameChromaVector : AbstractChromaVector
    {
        private SingleFrameChromaVector _singleFrameChromaVector;

        public MultiFrameChromaVector(double[][] toneAmplitudes, int tonesTotal, int tonesPerOctave) : base(toneAmplitudes, tonesTotal, tonesPerOctave)
        {
 
        }

        protected override void CreateVector()
        {
            throw new NotImplementedException();
        }

        //protected override void CreateVector(double[][] toneAmplitudes)
        //{
        //    int numOfFrames = toneAmplitudes.Length;
        //    double[] VectorValues = new double[_tonesPerOctave];
        //    for (int frame = 0; frame < numOfFrames; frame++)
        //    {
        //        double[] tempSingleFrameChromaVector = _singleFrameChromaVector.CreateVector(toneAmplitudes[frame]);
        //        for (int element = 0; element < _tonesPerOctave; element++)
        //        {
        //            VectorValues[element] += tempSingleFrameChromaVector[element];
        //        }
        //    }
        //}
    }
}
