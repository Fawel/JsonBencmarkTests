using BenchmarkDotNet.Attributes;
using JsonBenchmarkTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonBenchmarkTest
{
    public class DeserializeTests
    {
        private readonly string _pathToJsonFile = "";
        private string _jsonString;
        public DeserializeTests(string pathToJsonFile) 
        {
        }

        [GlobalSetup]
        public void Setup()
        {
            using var stream = new StreamReader(_pathToJsonFile);
            _jsonString = stream.ReadToEnd();
        }

        [Benchmark]
        public void Use_NewtonsoftJson()
        {
            var humans = Newtonsoft.Json.JsonConvert.DeserializeObject<Human[]>(_jsonString);
        }

        [Benchmark]
        public void Use_Jil()
        {
            var humans = Jil.JSON.Deserialize<Human[]>(_jsonString);
        }

        [Benchmark]
        public void Use_Utf8Json()
        {
            var humans = Utf8Json.JsonSerializer.Deserialize<Human[]>(_jsonString);
        }
    }
}
