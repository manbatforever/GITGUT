using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    static class VectorOperations
    {
        private static double DotProduct(double[] V1, double[] V2, int length)
        {
            double output = 0;
            for (int i = 0; i < length; i++)
            {
                output += V1[i] * V2[i];
            }
            return output;
        }

        private static double Magnitude(double[] Vector, int length)
        {
            double output = 0;
            for (int i = 0; i < length; i++)
            {
                output += Math.Pow(Vector[i], 2d);
            }
            return Math.Sqrt(output);
        }

        public static double CosineSimilarity(double[] V1, double[] V2, int length)
        {
            return DotProduct(V1, V2, length) / (Magnitude(V1, length) * Magnitude(V2, length));
        }
    }
}
