using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testapp
{
    static class Program
    {
        static void Main()
        {
            string FilePath = @"C:\Users\Martin\Music\Chromatic.wav";
            double[] RawSamples = AudioSamples.GetRawSamples(FilePath, out int SampleRate, out int Channels);
            KeyDetection.GetKey(RawSamples, SampleRate, Channels);
            //System.IO.StreamWriter file = new System.IO.StreamWriter("kerneltest.txt");
            //SpectralKernelStruct specs = new SpectralKernelStruct(44100, 440, 12, 6, 4096);
            //Kernel kernel = new Kernel(specs);
            //System.Numerics.Complex[][] k = kernel.AllBinKernels();
            //foreach (var item in k)
            //{
            //    foreach (var aitem in item)
            //    {
            //        file.Write(aitem.Magnitude / 4096 + ";");
            //    }
            //    file.WriteLine();
            //}
        }
    }
}
