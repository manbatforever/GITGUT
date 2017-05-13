/*using System;
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
        static int BPMStart = 50;
        static int BPMEnd = 200;
        static int BPMDevisor = 10;
        static int TestBPM = BPMEnd - BPMStart;
        static int SampleLength = 10;

        int N = 44100 * SampleLength;
        int fs = 44100;
        int ampMax = 1; //ampMax er 1 da NAudio gør at amplituden ligger mellem 1 og -1
        int amountOfBPMc = (BPMEnd - BPMStart) / BPMDevisor;

        int[][] L = new int[TestBPM / BPMDevisor][];

        double[] a = new double[44100 * SampleLength];
        double[] b = new double[44100 * SampleLength];
        double[] ta = new double[44100 * SampleLength];
        double[] tb = new double[44100 * SampleLength];
        int[] Ti = new int[TestBPM / BPMDevisor];
        double[] MaxBPME = new double[TestBPM / BPMDevisor];
        double[][] tl = new double[TestBPM / BPMDevisor][];
        double[][] tj = new double[TestBPM / BPMDevisor][];
        double[] E = new double[TestBPM / BPMDevisor];

        public void start(double[][] splitSamples)
        {
            FillLeftRight(splitSamples);
            Console.WriteLine(7);
            //DifferentiationOfLeftAndRight();
            Console.WriteLine(6);
            FFTComplexSignal();
            Console.WriteLine(5);
            TestByBPM();
            Console.WriteLine(4);
            ComputeTrainOfImpulses();
            Console.WriteLine(3);
            ComputeEnergy();
            Console.WriteLine(2);
            //BPMcMaxE();
            Console.WriteLine(BPMcMaxE());
            Console.WriteLine(1);
            throw new Exception();
        }
       
        void FillLeftRight(double[][] splitSamples)
        {
            if (splitSamples[0].Length <= N || splitSamples[1].Length <= N)
            {
                //Lav en exception der tager hånd om dette
                throw new SongNotLongEnoughException();
            }

            int sampleMidSong = Convert.ToInt32(splitSamples[0].Length / 2 - fs * (SampleLength / 2));
            for (int i = sampleMidSong, k = 0; k < N; i++, k++)
            {
                a[k] = splitSamples[0][i];
            }
            for (int i = sampleMidSong, k = 0; k < N; i++, k++)
            {
                b[k] = splitSamples[1][i];
            }
        }

        void DifferentiationOfLeftAndRight()
        {
            for (int k = 0; k < N - 1; k++)
            {
                a[k] = fs * (a[k + 1] - a[k]);
            }
            a[N - 1] = 0;

            for (int k = 0; k < N - 1; k++)
            {
                b[k] = fs * (b[k + 1] - b[k]);
            }
            b[N - 1] = 0;
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
            for (int BPMc = BPMStart, i = 0; BPMc < BPMEnd; BPMc += BPMDevisor, i++)
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
                        L[i][k] = -1;
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
                E[i] = 0;

                for (int k = 0; k < N; k++)
                {
                    E[i] += Complex.Abs(new Complex(ta[k], tb[k]) * new Complex(tl[i][k], tj[i][k]));
                }
            }
        }

        int BPMcMaxE()
        {
            double MaxBPME = 0;
            int BPMResult = 0;
            for (int i = 0; i < amountOfBPMc; i++)
            {
                if(E[i] > MaxBPME)
                {
                    MaxBPME = E[i];
                    BPMResult = i;
                }
            }
            return BPMResult * BPMDevisor + BPMStart;
        }

        int ComputeBPM()
        {
            Console.WriteLine();
            return Convert.ToInt32(E.Max());
        }
    }
}*/
