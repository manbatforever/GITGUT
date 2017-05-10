using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace testapp
{
    class BPMDetection
    {
        int N = 44100 * 5;
        int fs = 44100;
        int ampMax = 32767;
        int amountOfSubbands = 16;
        int amountOfBPMc = 200 / 5 - 1;

        double aconst = 1810.833;
        double bconst = -1610.833;

        double[] a = new double[44100 * 5];
        double[] b = new double[44100 * 5];
        double[] ta = new double[44100 * 5];
        double[] tb = new double[44100 * 5];
        double[][] tas = new double[16][];
        double[][] tbs = new double[16][];
        BPMToTest[] _bpmc = new BPMToTest[200 / 5 - 1];


        //Derivation and combfilter algorithms
        void DifferentiationOfLeftAndRight()
        {
            for (int k = 0; k < N - 1; k++)
            {
                a[k] = fs * (a[k + 1] - a[k]);
            }
            for (int k = 0; k < N - 1; k++)
            {
                b[k] = fs * (b[k + 1] - b[k]);
            }
        }
        void FillLeftRight(double[][] splitSamples)
        {
            for (int k = 0; k < N; k++)
            {
                a[k] = splitSamples[0][k];
            }
            for (int k = 0; k < N; k++)
            {
                b[k] = splitSamples[1][k];
            }
        }
        void FFTComplexSignal()
        {
            Complex[] complexSignal = new Complex[N];

            for (int k = 0; k < N; k++)
            {
                complexSignal[k] = new Complex(a[k], b[k]);
            }
            FastFourierTransform.FFT(complexSignal);

            for (int k = 0; k < N; k++)
            {
                ta[k] = complexSignal[k].Real;
            }

            for (int k = 0; k < N; k++)
            {
                tb[k] = complexSignal[k].Imaginary;
            }
        }

        void CreateSubbandArray()
        {
            int ws, ka = 0, kb = 0;

            for (int s = 0; s < amountOfSubbands; s++)
            {
                ws = CalculateSubbandWidth(s + 1);

                for (int i = 0; i < ws; i++, ka++)
                {
                    tas[s][i] = ta[ka];
                }
                for (int i = 0; i < ws; i++, kb++)
                {
                    tbs[s][i] = tb[kb];
                }
            }
        }

        int CalculateSubbandWidth(int i)
        {
            return (int)Math.Floor(aconst * i + bconst);
        }

        void GenerateTrainOfImpulses()
        {
            int ws;
            for (int i = 0; i < amountOfBPMc; i++)
            {
                ws = CalculateSubbandWidth(i + 1);
                int[] l = new int[ws];
                for (int k = 0; k < ws; k++)
                {
                    if (k % _bpmc[i].Ti == 0)
                    {
                        l[k] = ampMax;
                    }
                    else
                    {
                        l[k] = 0;
                    }
                }
                _bpmc[i].L = l;
            }
        }

        void FFTTrainOfImpulses()
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                Complex[] complexTrainOfImpulses = new Complex[_bpmc[i].TrainOfImpulses.Length];
                for (int k = 0; k < _bpmc[i].TrainOfImpulses.Length; k++)
                {
                    complexTrainOfImpulses[k] = new Complex(_bpmc[i].TrainOfImpulses[k], _bpmc[i].TrainOfImpulses[k]);
                }
                FastFourierTransform.FFT(complexTrainOfImpulses);
                double[] bpmctl = new double[_bpmc[i].L.Length];
                double[] bpmctj = new double[_bpmc[i].L.Length];
                for (int k = 0; k < _bpmc[i].TrainOfImpulses.Length; k++)
                {
                    bpmctl[k] = complexTrainOfImpulses[k].Real;
                }
                for (int k = 0; k < _bpmc[i].TrainOfImpulses.Length; k++)
                {
                    bpmctj[k] = complexTrainOfImpulses[k].Imaginary;
                }
                _bpmc[i].Tl = bpmctl;
                _bpmc[i].Tj = bpmctj;
            }
        }

        void TestBPM()
        {
            for (int p = 0; p < amountOfBPMc; p++)
            {
                double[][] E = new double[amountOfSubbands][];

                for (int i = 0; i < amountOfSubbands; i++)
                {
                    double[] sum = new double[CalculateSubbandWidth(i + 1)];
                    for (int k = 0; k < CalculateSubbandWidth(i + 1); k++)
                    {
                        sum[k] = (tas[i][k] + i * tbs[i][k]) * (_bpmc[k].Tl[k] + i * _bpmc[k].Tj[k]);
                    }
                    E[i] = sum;
                }
                _bpmc[p].E = E;
            }
        }

        void FindMaxBPMInSubband()
        {
            double[] MaxBPMOfSubbands = new double[amountOfSubbands];
            for (int i = 0; i < amountOfBPMc; i++)
            {
                for (int k = 0; k < amountOfSubbands; k++)
                {
                    MaxBPMOfSubbands[k] = _bpmc[i].E[k].Max();
                }
                _bpmc[i].SubbandBPMMax = MaxBPMOfSubbands;
            }

            //BPMMAX = alle max værdier
            //EBPMMAX = Max væredien af alle max værdierne
        }

        void ComputeBPM()
        {
            double BPMMax = _bpmc[1].SubbandBPMMax.Max();
            double sum = 0;


            for (int i = 0; i < amountOfSubbands; i++)
            {
                sum += _bpmc[1].SubbandBPMMax[i] * BPMMax;
            }
            Console.WriteLine(sum * (1 / _bpmc[1].SubbandBPMMax.Sum()));

        }

        int CalculateTI(int BPM)
        {
            return 60 / BPM * fs;
        }

        public void start(double[][] array)
        {
            Console.WriteLine(1);
            FillLeftRight(array);
            Console.WriteLine(2);
            DifferentiationOfLeftAndRight();
            Console.WriteLine(3);
            FFTComplexSignal();
            Console.WriteLine(4);
            CreateSubbandArray();
            Console.WriteLine(5);

            for (int BPMc = 5, k = 0; BPMc <= 200; BPMc += 5, k++)
            {
                _bpmc[k].Ti = CalculateTI(BPMc);
            }
            Console.WriteLine(6);
            GenerateTrainOfImpulses();
            Console.WriteLine(7);
            FFTTrainOfImpulses();
            Console.WriteLine(8);
            TestBPM();
            Console.WriteLine(9);
            FindMaxBPMInSubband();
            Console.WriteLine(10);
            ComputeBPM();
            Console.WriteLine(11);
        }
    }
}
