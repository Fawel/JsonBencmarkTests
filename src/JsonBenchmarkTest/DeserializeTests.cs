﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using JsonBenchmarkTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonBenchmarkTest
{
    [MemoryDiagnoser]
    public class DeserializeTests
    {
        private readonly string _pathToJsonFile =
            "C:\\Users\\Fawel\\Desktop\\Progs\\JsonBencmarkTests\\src\\TestObjects10k.json";

        [Benchmark]
        public Human[] Use_NewtonsoftJson()
        {
            using var jsonStream = new StreamReader(_pathToJsonFile);

            var serializer = new JsonSerializer();
            using var jsonTextReader = new JsonTextReader(jsonStream);
            return serializer.Deserialize<Human[]>(jsonTextReader);
        }

        [Benchmark]
        public Human[] Use_Jil()
        {
            using var jsonStream = new StreamReader(_pathToJsonFile);
            return Jil.JSON.Deserialize<Human[]>(jsonStream);
        }

        [Benchmark]
        public Human[] Use_Utf8Json()
        {
            using var jsonStream = new StreamReader(_pathToJsonFile);
            return Utf8Json.JsonSerializer.Deserialize<Human[]>(jsonStream.BaseStream);
        }
    }
}
