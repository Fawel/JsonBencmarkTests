using System;
using System.IO;
using BenchmarkDotNet.Running;
using JsonBenchmarkTest.Models;
using Newtonsoft.Json;

namespace JsonBenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<DeserializeTests>();
        }

        /// <summary>
        /// Использовать для генерации тестового json файла
        /// </summary>
        static void GenerateFile(int count)
        {
            count = count <= 0 ? 1 : count;
            using var writer = new StreamWriter($"C:\\Users\\Fawel\\Desktop\\Progs\\JsonBencmarkTests\\src\\TestObjects{count}.json");
            Jil.JSON.Serialize(Human.Factory.GenerateNewRandom(count), writer);
        }
    }
}
