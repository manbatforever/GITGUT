using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //A data type specifying the musical note distribution in a key
    internal abstract class KeyProfile
    {
        protected double[] _profile;

        // Rearranges the values in the profile, returning an array of a profile for the specified tonica
        public double[] CreateProfileForTonica(int tonica)
        {
            double[] currentPermutation = new double[12];
            for (int a = 0; a < 12; a++)
            {
                currentPermutation[a] = _profile[(a - tonica + 12) % 12];
            }
            return currentPermutation;
        }
    }
}
