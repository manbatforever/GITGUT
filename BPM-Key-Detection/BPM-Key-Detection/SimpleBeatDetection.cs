using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testapp
{
    class SimpleBeatDetection
    {
        int i0;
        double C = 0;
        double[] soundEnergyHistoryBuffer = new double[43];
        double[][] SplitSamples = new double[2][];

        //Beregn instant energien, 'e' (R1)
        double ComputeInstantEnergy()
        {
            double instantEnergy = 0;

            for (int k = i0; k < i0 + 1024; k++)
            {
                instantEnergy += Math.Pow(SplitSamples[0][k], 2) + Math.Pow(SplitSamples[1][k], 2);
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

            //Vi gør dette da det ikke er hensigtsmæssigt at springe mellem to arrays i en for-løkke

            for (int i = 0; i < 43; i++)
            {
                CopyOfsoundEnergyHistoryBuffer[i] = soundEnergyHistoryBuffer[i];
            }

            for (int i = 0, k = 1; k < 43; i++, k++)
            {
                soundEnergyHistoryBuffer[i] = CopyOfsoundEnergyHistoryBuffer[k];
            }

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

        public int BeatDetection(double[][] SplitSamples)
        {
            this.SplitSamples[0] = new double[SplitSamples[0].Length + 1];
            this.SplitSamples[1] = new double[SplitSamples[1].Length + 1];

            int AmountOfChannels = SplitSamples.Length;
            //Vi gør dette da det ikke er hensigtsmæssigt at springe mellem to arrays i en for-løkke
            for (int i = 0, AmountOfSamples; i < AmountOfChannels; i++)
            {
                AmountOfSamples = SplitSamples[i].Length;

                for (int k = 0; k < AmountOfSamples; k++)
                {
                    this.SplitSamples[i][k] = SplitSamples[i][k];
                }
            }

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
