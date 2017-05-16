using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public static class CQTConstants
    {
        private static readonly int CutoffFrequency = 2000;
        private static readonly int DownSamplingFactor = 10;
        private static readonly int SamplesPerFrame = 16384;
        private static readonly int HopSize = 4;
        private static readonly int TonesPerOctave = 12;
        private static readonly int NumOfOctaves = 6;
        private static readonly int TonesTotal = TonesPerOctave * NumOfOctaves;
        private static readonly double MinimumFrequency = 27.5;

        public static class Int
        {
            public static readonly int CutoffFrequency = CQTConstants.CutoffFrequency;
            public static readonly int DownSamplingFactor = 10;
            public static readonly int SamplesPerFrame = 16384;
            public static readonly int HopSize = 4;
            public static readonly int TonesPerOctave = 12;
            public static readonly int NumOfOctaves = 6;
            public static readonly int TonesTotal = TonesPerOctave * NumOfOctaves;
        }

        static class Double
        {
            public static readonly double CutoffFrequency = 2000;
            public static readonly double DownSamplingFactor = 10;
            public static readonly double SamplesPerFrame = 16384;
            public static readonly double HopSize = 4;
            public static readonly double TonesPerOctave = 12;
            public static readonly double NumOfOctaves = 6;
            public static readonly double TonesTotal = TonesPerOctave * NumOfOctaves;
            public static readonly double MinimumFrequency = 27.5;
        }
    }
}
