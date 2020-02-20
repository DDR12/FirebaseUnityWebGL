using Firebase;
using System;
using System.Threading.Tasks;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new TaskCompletionSource<bool>();

            Task t = new TaskCompletionSource<bool>().Task;
            GenerateAppOptionsFile();
            Console.ReadLine();
        }

        private static void GenerateAppOptionsFile()
        {
            AppOptions options = AppOptions.LoadDefaultOptions();
            Console.WriteLine(options.ApiKey);
        }
        public static byte[] ToByteArray(float[] arrayToConvert)
        {
            int floatByteSize = sizeof(float);
            // create a byte array and copy the floats into it...
            var byteArray = new byte[arrayToConvert.Length * floatByteSize];
            Buffer.BlockCopy(arrayToConvert, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        public static float[] FromByteArray(byte[] byteArray)
        {
            int floatByteSize = sizeof(float);
            // create a second float array and copy the bytes into it...
            var result = new float[byteArray.Length / floatByteSize];
            Buffer.BlockCopy(byteArray, 0, result, 0, byteArray.Length);
            return result;
        }
    }
}
