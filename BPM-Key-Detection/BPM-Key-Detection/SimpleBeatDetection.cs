using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication24
{
    class SimpleBeatDetection
    {
        int i0;
        double C = 0;
        double[] a = new double[44032];
        double[] b = new double[44032];
        double[,] B = new double[2, 44032];
        double[] soundEnergyHistoryBuffer = new double[43];

        //Beregn instant energien, 'e' (R1)
        double ComputeInstantEnergy()
        {
            double instantEnergy = 0;

            for (int k = i0; k < i0 + 1024; k++)
            {
                instantEnergy += Math.Pow(a[k], 2) + Math.Pow(b[k], 2);
            }
            i0++;
            return instantEnergy;
        }
        // Algoritme 2 (R3)
        double ComputeLocalAverageEnergy()
        {
            double localAverageEnergy = 0, sum = 0;

            for (int i = 0; i < 43; i++)
            {
                sum += Math.Pow(soundEnergyHistoryBuffer[i], 2);
            }

            localAverageEnergy = 1 / 43 * sum;

            return localAverageEnergy;
        }

        void CreateEnergyHistoryBuffer()
        {
            i0 = 0;
            for (int i = 0; i < 43; i++)
            {
                soundEnergyHistoryBuffer[i] = ComputeInstantEnergy();
            }
        }

        void UpdateEnergyHistoryBuffer()
        {
            double[] CopyOfsoundEnergyHistoryBuffer = new double[43];

            soundEnergyHistoryBuffer.CopyTo(CopyOfsoundEnergyHistoryBuffer, 1);
            CopyOfsoundEnergyHistoryBuffer.CopyTo(soundEnergyHistoryBuffer, 0);

            soundEnergyHistoryBuffer[43 - 1] = ComputeInstantEnergy();
        }

        // Beregner <E> Local Average Energy
        /*double ComputeLocalAverageEnergy()
        {
            double localAverageEnergy = 0, sum = 0;

            for (int i = 0; i < 44032; i++)
            {
                sum += Math.Pow(B[0,i],2) + Math.Pow(B[1,i], 2);
            }

            localAverageEnergy = 1024 / 44100 * sum;

            return localAverageEnergy;
        }*/


        //Tester e for om der er et beat
        bool IsBeat()
        {
            return ComputeInstantEnergy() > C * ComputeLocalAverageEnergy();
        }


        //Algoritme 3 (R4)
        double ComputeVariance()
        {
            double sum = 0, Variance = 0;
            for (int i = 0; i < 43; i++)
            {
                sum += Math.Pow((soundEnergyHistoryBuffer[i] - ComputeInstantEnergy()), 2);
            }
            Variance = 1 / 43 * sum;
            return Variance;
        }

        //Beregn konstanten C
        double ComputeConstant()
        {
            return (-0.0025714 * ComputeVariance()) + 1.5142857;
        }

        public int BeatDetection(double[,] SplitSamples)
        {
            int beat = 0;

            CreateEnergyHistoryBuffer();

            for (int i = 0; i < 58462; i++)
            {
                if (IsBeat())
                {
                    beat++;
                    Console.WriteLine("beat");
                }

                UpdateEnergyHistoryBuffer();
            }

            return beat;
        }
    }
}
