using System;
using BenchmarkDotNet.Running;

namespace JsonBenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<DeserializeTests>();
        }
    }
}
