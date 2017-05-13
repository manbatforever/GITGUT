using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    abstract class KeyProfile
    {
        private double[] _profile;

        public double[] Profile { get => _profile; protected set => _profile = value; }

        // Rearranges the values in the profile, returning an array of profiles for each tonica
        public double[][] CreateProfileForEachTonica()
        {
            double[][] ProfileForEachTonica = new double[12][];
            for (int i = 12; i > 0; i--)
            {
                double[] currentPermutation = new double[12];
                for (int a = 0; a < 12; a++)
                {
                    currentPermutation[a] = _profile[(i + a) % 12];
                }
                ProfileForEachTonica[i - 1] = currentPermutation;
            }
            return ProfileForEachTonica;
        }
    }
}
