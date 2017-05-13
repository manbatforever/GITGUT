using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    abstract class AbstractChromaVector
    {
        protected double[] _vectorValues;
        protected int _tonesTotal;
        protected int _tonesPerOctave;
        protected int _numOfOctaves;
        public double[] VectorValues { get => _vectorValues; set => _vectorValues = value; }


        public AbstractChromaVector(double[][] toneAmplitudes, int tonesTotal, int tonesPerOctave)
        {

            _tonesTotal = tonesTotal;
            _tonesPerOctave = tonesPerOctave;
            _numOfOctaves = tonesTotal / tonesPerOctave;
            CreateVector();
        }



        protected abstract void CreateVector();




    }
}
