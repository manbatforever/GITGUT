using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    internal class KeyEstimationLogs
    {
        public static int cutoffFrequency = 2000;
        public static int SamplesPerFrame = 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2 * 2;
        public static int HopsPerFrame = 4;
        public static int TonesPerOctave = 12;
        public static int NumberOfOctaves = 6;
        public static double MinimumFrequency = 27.5;

        public static int NumberOfFiles;
        public static int Counter = 0;
        public static string[] FileNames;
        public static long[] SampleFetchTime;
        public static long[] FramingTime;
        public static long[] FFTTime;
        public static long[] SpectralKernelTime;
        public static long[] ToneAmplitudeTime;
        public static long[] ChromaVectorTime;
        public static long[] KeyCalculationTime;
        public static int CorrectKeys;

        public static long TotalTime = 0;

        public static void SetNumberOfFiles(int number)
        {
            NumberOfFiles = number + 1;
            FileNames = new string[NumberOfFiles];

            SampleFetchTime = new long[NumberOfFiles];
            FramingTime = new long[NumberOfFiles];
            FFTTime = new long[NumberOfFiles];
            SpectralKernelTime = new long[NumberOfFiles];
            ToneAmplitudeTime = new long[NumberOfFiles];
            ChromaVectorTime = new long[NumberOfFiles];
            KeyCalculationTime = new long[NumberOfFiles];
        }

        public static void WriteToFile()
        {
            string directory = @"Key Estimation Logs\";
            System.IO.Directory.CreateDirectory(directory);
            string fileName = $"key {SamplesPerFrame.ToString()}-{HopsPerFrame.ToString()}-{NumberOfOctaves.ToString()}-{MinimumFrequency.ToString()}.txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(directory + fileName);

            file.WriteLine($"Cutoff Frequency (lowpassfilter): {cutoffFrequency}");
            file.WriteLine($"Samples Per Frame (buffersize):   {SamplesPerFrame}");
            file.WriteLine($"Hops Per Frame (overlap):         {HopsPerFrame}");
            file.WriteLine($"Tones Per Octave:                 {TonesPerOctave}");
            file.WriteLine($"Number Of Octaves:                {NumberOfOctaves}");
            file.WriteLine($"Minimum Frequency:                {MinimumFrequency}");
            file.WriteLine();
            file.WriteLine();
            file.WriteLine($"Correct Keys: {CorrectKeys} out of {NumberOfFiles - 1}");
            file.WriteLine();
            file.WriteLine("Time results\n\n");
            file.WriteLine($"Total time(ms): {TotalTime}");
            file.WriteLine();
            int numberPad = 15;
            int namePad = 100;
            string header = $"{"Nr.".PadRight(5)} | " +
                $"{"File Name".PadRight(namePad)} | " +
                $"{"Sample fetch(ms)".PadRight(numberPad)} |" +
                $"{"Framing(ms)".PadRight(numberPad)} | " +
                $"{"FFT(ms)".PadRight(numberPad)} | " +
                $"{"Kernel(ms)".PadRight(numberPad)} | " +
                $"{"Amplitude(ms)".PadRight(numberPad)} | " +
                $"{"Chroma vector(ms)".PadRight(numberPad)} | " +
                $"{"Key calc(ms)".PadRight(numberPad)}";
            file.WriteLine(header);
            file.WriteLine("".PadRight(header.Length, '-'));
            for (int counter = 1; counter < NumberOfFiles; counter++)
            {
                file.WriteLine($"{counter.ToString().PadRight(5)} | " +
                    $"{FileNames[counter].PadRight(namePad)} | " +
                    $"{SampleFetchTime[counter].ToString().PadRight(numberPad)} | " +
                    $"{FramingTime[counter].ToString().PadRight(numberPad)} | " +
                    $"{FFTTime[counter].ToString().PadRight(numberPad)} | " +
                    $"{SpectralKernelTime[counter].ToString().PadRight(numberPad)} | " +
                    $"{ToneAmplitudeTime[counter].ToString().PadRight(numberPad)} | " +
                    $"{ChromaVectorTime[counter].ToString().PadRight(numberPad)} | " +
                    $"{KeyCalculationTime[counter].ToString().PadRight(numberPad)} |");
            }
            file.Close();
        }
    }
}
