using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using JsonBenchmarkTest.Models;
using Newtonsoft.Json;

namespace JsonBenchmarkTest
{
    [MemoryDiagnoser]
    public class SerializeTests
    {
        private readonly Human[] _testArray;

        public SerializeTests()
        {
            _testArray = Human.Factory.GenerateNewRandom(1000);
        }

        [Benchmark]
        public string Use_NewtonsoftJson_JsonConvert_ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(_testArray);
        }

        [Benchmark]
        public string Use_Jil_ToString()
        {
            return Jil.JSON.Serialize<Human[]>(_testArray);
        }

        [Benchmark]
        public string Use_Utf8Json_ToString()
        {
            return Utf8Json.JsonSerializer.ToJsonString(_testArray);
        }

        [Benchmark]
        public void Use_NewtonsoftJson_ToStream()
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            Newtonsoft.Json.JsonSerializer.CreateDefault().Serialize(writer, _testArray);
        }

        [Benchmark]
        public void Use_Jil_ToStream()
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            Jil.JSON.Serialize<Human[]>(_testArray, writer);
        }

        [Benchmark]
        public void Use_Utf8Json_ToStream()
        {
            using var memoryStream = new MemoryStream();
            Utf8Json.JsonSerializer.Serialize<Human[]>(memoryStream, _testArray);
        }

        [Benchmark]
        public byte[] Use_Utf8Json_ToByteArray()
        {
            return Utf8Json.JsonSerializer.Serialize<Human[]>(_testArray);
        }
    }
}
