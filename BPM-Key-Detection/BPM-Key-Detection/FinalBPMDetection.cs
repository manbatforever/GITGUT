using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace testapp
{
    class SongNotLongEnoughException : Exception
    {
        public SongNotLongEnoughException() : base("The song does not have enough samples to be analyzed.") { }
        public SongNotLongEnoughException(string songName) : base($"The song {songName} does not have enough samples to be analyzed.") { }
    }

    class FinalBPMDetection
    {
        int N = 44100 * 5;
        int fs = 44100;
        int ampMax = 1; //ampMax er 1 da NAudio gør at amplituden ligger mellem 1 og -1
        int amountOfBPMc = (180 - 60) / 1;

        int[][] L = new int[(180 - 60) / 1][];

        double[] a = new double[44100 * 5 + 1];
        double[] b = new double[44100 * 5 + 1];
        double[] ta = new double[44100 * 5];
        double[] tb = new double[44100 * 5];
        int[] Ti = new int[(180 - 60) / 1];
        double[] MaxBPME = new double[(180 - 60) / 1];
        double[][] tl = new double[(180 - 60) / 1][];
        double[][] tj = new double[(180 - 60) / 1][];
        double[][] E = new double[(180 - 60) / 1][];

        public void start(double[][] splitSamples)
        {
            FillLeftRight(splitSamples);
            Console.WriteLine(7);
            FFTComplexSignal();
            Console.WriteLine(6);
            TestByBPM();
            Console.WriteLine(5);
            ComputeTrainOfImpulses();
            Console.WriteLine(4);
            ComputeEnergy();
            Console.WriteLine(3);
            BPMcMaxE();
            Console.WriteLine(2);
            Console.WriteLine(ComputeBPM());
            Console.WriteLine(1);
            throw new Exception();
        }

        void FillLeftRight(double[][] splitSamples)
        {
            if (splitSamples[0].Length < N + 1 || splitSamples[1].Length < N + 1)
            {
                //Lav en exception der tager hånd om dette
                throw new SongNotLongEnoughException();
            }

            int sampleMidSong = Convert.ToInt32(splitSamples[0].Length / 2 - fs * 2.5);
            for (int i = sampleMidSong, k = 0; k < N + 1; i++, k++)
            {
                a[k] = splitSamples[0][i];
            }
            for (int i = sampleMidSong, k = 0; k < N + 1; i++, k++)
            {
                b[k] = splitSamples[1][i];
            }
        }

        void FFTComplexSignal()
        {
            Complex[] complexSignal = new Complex[N];
            Complex[] FFTComplexSignal = new Complex[N];

            for (int k = 0; k < N; k++)
            {
                complexSignal[k] = new Complex(a[k], b[k]);
            }
            FFTComplexSignal = FastFourierTransform.FFT(complexSignal);

            for (int k = 0; k < N; k++)
            {
                ta[k] = FFTComplexSignal[k].Real;
            }

            for (int k = 0; k < N; k++)
            {
                tb[k] = FFTComplexSignal[k].Imaginary;
            }
        }

        void TestByBPM()
        {
            for (int BPMc = 60, i = 0; BPMc < 180; BPMc += 1, i++)
            {
                Ti[i] = Convert.ToInt32(CalculateTi(BPMc));
            }
        }

        double CalculateTi(double BPMc)
        {
            return 60 / BPMc * Convert.ToDouble(fs);
        }

        void ComputeTrainOfImpulses()
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                L[i] = new int[N];
                for (int k = 0; k < N; k++)
                {
                    if ((k % Ti[i]) == 0)
                    {
                        L[i][k] = ampMax;
                    }
                    else
                    {
                        L[i][k] = 0;
                    }
                }
            }
            Complex[] complexSignal = new Complex[N];
            Complex[] FFTComplexSignal = new Complex[N];

            for (int i = 0; i < amountOfBPMc; i++)
            {

                for (int k = 0; k < N; k++)
                {
                    complexSignal[k] = new Complex(L[i][k], L[i][k]);
                }

                FFTComplexSignal = FastFourierTransform.FFT(complexSignal);
                tl[i] = new double[N];
                tj[i] = new double[N];

                for (int k = 0; k < N; k++)
                {
                    tl[i][k] = FFTComplexSignal[k].Real;
                }
                for (int k = 0; k < N; k++)
                {
                    tj[i][k] = FFTComplexSignal[k].Imaginary;
                }
            }
        }

        void ComputeEnergy()
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                E[i] = new double[N];

                for (int k = 0; k < N; k++)
                {
                    E[i][k] = (ta[k] + i * tb[k]) * (tl[i][k] + i * tj[i][k]);
                }
            }
        }

        void BPMcMaxE()
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                MaxBPME[i] = E[i].Max();
            }
        }

        int ComputeBPM()
        {
            Console.WriteLine(MaxBPME.Max());
            return Convert.ToInt32(MaxBPME.Max());
        }
    }
}
