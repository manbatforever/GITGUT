using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    internal class KeyEstimationLogs
    {
        public static int SamplesPerFrame = 131072;
        public static int HopsPerFrame = 4;
        public static int TonesPerOctave = 12;
        public static int NumberOfOctaves = 6;
        public static double MinimumFrequency = 27.5;
        public static int CutoffFrequency = (int)(MinimumFrequency * Math.Pow(2, NumberOfOctaves));

        public static void Start()
        {
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(@"C:\Users\Martin\Music\test\");
            System.IO.FileInfo[] fileInfo = dirInfo.GetFiles();
            List<MusicFile> listOfFiles = new List<MusicFile>();
            System.IO.StreamWriter output = new System.IO.StreamWriter("octaves.txt");
            for (int octaves = 1; octaves <= 8; octaves++)
            {
                NumberOfOctaves = octaves;
                CutoffFrequency = (int)(MinimumFrequency * Math.Pow(2, NumberOfOctaves));
                int correctCounter = 0;
                System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
                foreach (var filename in fileInfo)
                {
                    MusicFile musicfile = new MusicFile(filename.FullName);
                    MusicFileSamples samples = musicfile.GetRawSamples();
                    musicfile.EstimateKey(samples);
                    if (musicfile.FileName.Contains(musicfile.CamelotNotation))
                        correctCounter++;
                }
                timer.Stop();
                Console.WriteLine($"{NumberOfOctaves}; {CutoffFrequency}; {MinimumFrequency}; {correctCounter}; {timer.Elapsed.TotalSeconds}");
                output.WriteLine($"{NumberOfOctaves}; {CutoffFrequency}; {MinimumFrequency}; {correctCounter}; {timer.Elapsed.TotalSeconds}");
            }
            output.Close();
            Console.ReadKey();
        }
    }
}
