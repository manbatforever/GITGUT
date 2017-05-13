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

    class BPMDetection
    {
        static int amountOfSubbands = 16;
        
        static int BPMStart = 50;
        static int BPMEnd = 200;
        static int BPMDevisor = 10;
        static int TestBPMInt = BPMEnd - BPMStart; //navn skal ændres
        static int SampleLength = 5;
        static int amountOfBPMc = (BPMEnd - BPMStart) / BPMDevisor;
        static int N = 44100 * SampleLength;

        int fs = 44100;
        int ampMax = 1; //ampMax er 1 da NAudio gør at amplituden ligger mellem 1 og -1

        double aconst = 1810.833;
        double bconst = -1610.833;

        double[] a = new double[N];
        double[] b = new double[N];
        double[] ta = new double[N];
        double[] tb = new double[N];
        int[] Ti = new int[TestBPMInt / BPMDevisor];
        double[] eBPMMax = new double[amountOfSubbands];
        double[][] tas = new double[amountOfSubbands][];
        double[][] tbs = new double[amountOfSubbands][];
        BPMToTest[] _bpmc = new BPMToTest[amountOfBPMc];

        public void start(double[][] array)
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                _bpmc[i] = new BPMToTest();
            }
            Console.WriteLine(1);
            FillLeftRight(array);
            Console.WriteLine(2);
            DifferentiationOfLeftAndRight();
            Console.WriteLine(3);
            FFTComplexSignal();
            Console.WriteLine(4);
            CreateSubbandArray();
            Console.WriteLine(5);
            TestByBPM();
            Console.WriteLine(6);
            GenerateTrainOfImpulses();
            Console.WriteLine(7);
            FFTTrainOfImpulses();
            Console.WriteLine(8);
            TestBPM();
            Console.WriteLine(9);
            Console.WriteLine(FindMaxBPMOfSubband()); 
            throw new Exception();
        }
        //Her udvælges samples i midten af nummeret og lægges over i venstre stream og højre stream
        void FillLeftRight(double[][] splitSamples)
        {
            if(splitSamples[0].Length <= N || splitSamples[1].Length <= N)
            {
                //Lav en exception der tager hånd om dette
                throw new SongNotLongEnoughException();
            }

            int sampleMidSong = Convert.ToInt32((splitSamples[0].Length - fs * SampleLength) / 2);
            for (int i = sampleMidSong, k = 0; k < N; i++, k++)
            {
                a[k] = splitSamples[0][i];
            }
            for (int i = sampleMidSong, k = 0; k < N; i++, k++)
            {
                b[k] = splitSamples[1][i];
            }
        }

        //Derivation and combfilter algorithms
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

        void CreateSubbandArray()
        {
            int ws, ka = 0, kb = 0;

            for (int s = 0; s < amountOfSubbands; s++)
            {
                ws = CalculateSubbandWidth(s + 1);
                tas[s] = new double[ws];
                tbs[s] = new double[ws];
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

        void TestByBPM()
        {
            for (int BPMc = BPMStart, i = 0; BPMc < BPMEnd; BPMc += BPMDevisor, i++)
            {
                Ti[i] = Convert.ToInt32(CalculateTI(BPMc));
            }
        }

        double CalculateTI(double BPM)
        {
            return (60 / BPM) * Convert.ToDouble(fs);
        }

        void GenerateTrainOfImpulses()
        {
            int ws = 0, ka = 0;
            for (int i = 0; i < amountOfBPMc; i++)
            {
                _bpmc[i].L = new int[amountOfSubbands][];
                for (int s = 0; s < amountOfSubbands; s++)
                {
                    ws = CalculateSubbandWidth(s + 1);
                    _bpmc[i].L[s] = new int[ws];

                    for (int k = 0; k < ws; k++, ka++) // Ka, da vi ikke skal nulstille værdien, vi vil gerne sammenligne med nye samples og ikke gamle
                    {
                        if (ka % Ti[i] == 0)
                        {
                            _bpmc[i].L[s][k] = ampMax;
                        }
                        else
                        {
                            _bpmc[i].L[s][k] = 0;
                        }
                    }
                }
            }
        }

        int CalculateSubbandWidth(int i)
        {
            return (int)Math.Floor(aconst * i + bconst);
        }

        void FFTTrainOfImpulses()
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                _bpmc[i].Tl = new double[amountOfSubbands][];
                _bpmc[i].Tj = new double[amountOfSubbands][];
                for (int s = 0; s < amountOfSubbands; s++)
                {
                    int lengthOfTrain = _bpmc[i].L[s].Length;

                    Complex[] complexTrainOfImpulses = new Complex[lengthOfTrain];
                    Complex[] FFTComplexTrainOfImpulses = new Complex[lengthOfTrain];

                    for (int k = 0; k < lengthOfTrain; k++)
                    {
                        complexTrainOfImpulses[k] = new Complex(_bpmc[i].L[s][k], _bpmc[i].L[s][k]);
                    }
                    FFTComplexTrainOfImpulses = FastFourierTransform.FFT(complexTrainOfImpulses);
                    int ws = CalculateSubbandWidth(s + 1);
                    _bpmc[i].Tl[s] = new double[ws];
                    _bpmc[i].Tj[s] = new double[ws];

                    for (int k = 0; k < ws; k++)
                    {
                        _bpmc[i].Tl[s][k] = FFTComplexTrainOfImpulses[k].Real;
                    }
                    for (int k = 0; k < ws; k++)
                    {
                        _bpmc[i].Tj[s][k] = FFTComplexTrainOfImpulses[k].Imaginary;
                    }
                }
            }
        }

        int NextPowerOfTwo(int i)
        {
            for (int p = 0; true; p++)
            {
                if (i <= Math.Pow(2, p))
                {
                    return Convert.ToInt32(Math.Pow(2, p));
                }
            }            
        }        

        void TestBPM()
        {
            for (int p = 0; p < amountOfBPMc; p++)
            {
                double[] E = new double[amountOfSubbands];

                for (int s = 0; s < amountOfSubbands; s++)
                {
                    E[s] = 0;
                    for (int k = 0; k < CalculateSubbandWidth(s + 1); k++)
                    {
                        E[s] += Complex.Abs(new Complex(tas[s][k], tbs[s][k]) * new Complex(_bpmc[p].Tl[s][k], _bpmc[p].Tj[s][k]));
                    }
                }
                _bpmc[p].E = E;
            }
        }

        int FindMaxBPMOfSubband()
        {
            double EBPMMaxSum = 0, EBPMProductSum = 0;
            for (int i = 0; i < amountOfSubbands; i++)
            {
                double MaxBPME = 0;
                int BPMResult = 0;
                for (int k = 0; k < amountOfBPMc; k++)
                {
                    if (_bpmc[k].E[i] > MaxBPME)
                    {
                        MaxBPME = _bpmc[k].E[i];
                        BPMResult = k * BPMDevisor + BPMStart;
                    }
                }

                EBPMMaxSum += MaxBPME;
                EBPMProductSum += MaxBPME * BPMResult;
            }

            return Convert.ToInt32(EBPMProductSum / EBPMMaxSum);
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

            //BPMMAX = alle max værdier i hvert subband
            //EBPMMAX = Max væredien af alle max værdierne af subbandsne
        }

        void FindEbpmMax()
        {            
            for (int i = 0; i < amountOfBPMc; i++)
            {
                for (int s = 0; s < amountOfSubbands; s++)
                {
                    if (_bpmc[i].SubbandBPMMax[s] > eBPMMax[s])
                    {
                        eBPMMax[s] = _bpmc[i].SubbandBPMMax[s];
                    }
                }
            }
        }

        void ComputeBPM()
        {
            double BPMMax = _bpmc[23].SubbandBPMMax.Max();
            //double BPMMax = 120;
            double sum = 0, sum2 = 0;

            for (int s = 0; s < amountOfSubbands; s++)
            {
                sum += eBPMMax[s];
            }
            for (int s = 0; s < amountOfSubbands; s++)
            {
                sum2 += eBPMMax[s] * BPMMax;
            }
            Console.WriteLine((1 / sum) * sum2);
        }
    }
}*/