﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using Newtonsoft.Json;

namespace testapp
{
    static class KernelDatabase
    {
        private static string KernelPath = @"Kernel\";

        public static Complex[][] GetKernel(SpectralKernelStruct KernelSpecs)
        {
            string kernelFileName = CreateFileName(KernelSpecs);
            bool inDatabase = CheckDatabase(kernelFileName);
            if (inDatabase)
            {
                return GetKernelFromDatabase(kernelFileName);
            }
            else
            {
                Kernel kernel = new Kernel(KernelSpecs);
                Complex[][] newKernel = kernel.AllBinKernels();
                //AddKernel(newKernel, kernelFileName);
                return newKernel;
            }
        }

        private static string CreateFileName(SpectralKernelStruct KernelSpecs)
        {
            return KernelSpecs.BaseFrequency.ToString() + KernelSpecs.BinsPerOctave.ToString() + KernelSpecs.BinsTotal.ToString() + KernelSpecs.FrameSize.ToString() + KernelSpecs.Samplerate.ToString() + ".kernel";
        }

        private static bool CheckDatabase(string kernelFileName)
        {
            string[] kernels = Directory.GetFiles(KernelPath);
            foreach (string item in kernels)
            {
                if (item.Contains(kernelFileName))
                {
                    return true;
                }
            }
            return false;
        }

        private static Complex[][] GetKernelFromDatabase(string kernelFileName)
        {
            StreamReader kernelFile = new StreamReader(KernelPath + kernelFileName);
            string JsonKernel = kernelFile.ReadToEnd();
            return JsonConvert.DeserializeObject<Complex[][]>(JsonKernel);
        }

        private static void AddKernel(Complex[][] kernel, string kernelFileName)
        {
            StreamWriter kernelFile = new StreamWriter(KernelPath + kernelFileName);
            string JsonKernel = JsonConvert.SerializeObject(kernel);
            kernelFile.Write(JsonKernel);
            kernelFile.Close();
            FileInfo info = new FileInfo(KernelPath + kernelFileName);
            info.IsReadOnly = true;
        }
    }
}