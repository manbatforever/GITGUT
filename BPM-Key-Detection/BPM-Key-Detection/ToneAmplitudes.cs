using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    public class ToneAmplitudes
    {
        private double[] _toneAmplitudeValues;

        public double[] ToneAmplitudeValues { get => _toneAmplitudeValues; }

        public ToneAmplitudes(FrequencyBins musicFileFFT, SpectralKernel spectralKernels)
        {

        }

        private double GetSingleToneAmplitude(FrequencyBins musicFileFrame, SpectralKernel spectralKernel)
        {
            double toneAmplitude = 0;
            for (int bin = 0; bin < musicFileFrame.NumOfBins; bin++)
            {
                toneAmplitude += musicFileFrame.BinValues[bin] * 
            }
        }
    }
}
