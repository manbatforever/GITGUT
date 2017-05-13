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

        private static double IbrahimUpper(double[] V1, double[] V2, int length)
        {
            double output = 0;
            for (int i = 0; i < length; i++)
            {
                output += (V1[i] - V1.Average()) * (V2[i] - V2.Average());
            }
            return output;
        }

        private static double IbrahimLower(double[] vector, int length)
        {
            double output = 0;
            for (int i = 0; i < length; i++)
            {
                output += Math.Pow(vector[i] - vector.Average(), 2);
            }
            return output;
        }

        public static double CosineSimilarity(double[] V1, double[] V2, int length)
        {
            return DotProduct(V1, V2, length) / (Magnitude(V1, length) * Magnitude(V2, length));
            //return IbrahimUpper(V1, V2, length) / Math.Sqrt(IbrahimLower(V1, length) * IbrahimLower(V2, length));
        }
    }
}
