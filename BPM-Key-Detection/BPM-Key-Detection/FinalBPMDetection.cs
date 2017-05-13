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
        int amountOfBPMc = (180 - 60) / 3;
        int _BPMcChange = 3;

        double[] a = new double[44100 * 5];
        double[] b = new double[44100 * 5];
        double[] ta = new double[44100 * 5];
        double[] tb = new double[44100 * 5];
        int[] Ti = new int[(180 - 60) / 3];
        double[] MaxBPME = new double[(180 - 60) / 3];
        BPMc[] _BPMc = new BPMc[(180 - 60) / 3];


        public void start(double[][] splitSamples)
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                _BPMc[i] = new BPMc();
            }
            FillLeftRight(splitSamples);
            FFTComplexSignal();
            TestByBPM();
            ComputeTrainOfImpulses();
            ComputeEnergy();
            BPMcMaxE();
            Console.WriteLine(ComputeBPM());
        }

        void FillLeftRight(double[][] splitSamples)
        {
            if (splitSamples[0].Length < N || splitSamples[1].Length < N)
            {
                //Lav en exception der tager hånd om dette
                throw new SongNotLongEnoughException();
            }

            int sampleMidSong = Convert.ToInt32(splitSamples[0].Length / 2 - fs * 2.5);
            for (int i = sampleMidSong, k = 0; k < N; i++, k++)
            {
                a[k] = splitSamples[0][i];
            }
            for (int i = sampleMidSong, k = 0; k < N; i++, k++)
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
            for (int i = 0; i < amountOfBPMc; i++)
            {
                Ti[i] = Convert.ToInt32(CalculateTi(i * _BPMcChange + _BPMcChange));
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
                _BPMc[i].L = new int[N];
                for (int k = 0; k < N; k++)
                {
                    if ((k % Ti[i]) == 0)
                    {
                        _BPMc[i].L[k] = ampMax;
                    }
                    else
                    {
                        _BPMc[i].L[k] = 0;
                    }
                }
            }
            Complex[] complexSignal = new Complex[N];
            Complex[] FFTComplexSignal = new Complex[N];

            for (int i = 0; i < amountOfBPMc; i++)
            {

                for (int k = 0; k < N; k++)
                {
                    complexSignal[k] = new Complex(_BPMc[i].L[k], _BPMc[i].L[k]);
                }

                FFTComplexSignal = FastFourierTransform.FFT(complexSignal);
                _BPMc[i].TL = new double[N];
                _BPMc[i].TJ = new double[N];

                for (int k = 0; k < N; k++)
                {
                    _BPMc[i].TL[k] = FFTComplexSignal[k].Real;
                }
                for (int k = 0; k < N; k++)
                {
                    _BPMc[i].TJ[k] = FFTComplexSignal[k].Imaginary;
                }
            }
        }

        void ComputeEnergy()
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                _BPMc[i].E = new double[N];

                for (int k = 0; k < N; k++)
                {
                    _BPMc[i].E[k] = (ta[k] + i * tb[k]) * (_BPMc[i].TL[k] + i * _BPMc[i].TJ[k]);
                }
            }
        }

        void BPMcMaxE()
        {
            for (int i = 0; i < amountOfBPMc; i++)
            {
                MaxBPME[i] = _BPMc[i].E.Max();
            }
        }

        int ComputeBPM()
        {
            return Convert.ToInt32(MaxBPME.Max());
        }
    }
}
