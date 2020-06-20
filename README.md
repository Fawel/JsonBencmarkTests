# JsonBencmarkTests
Пытаемся найти быстрейший сериализатор/десериализатор JSON на айтишном западе

Цель - найти самый шустрый десериализатор для Json больших размеров. Цель номер два - найти самый шустрый сериализатор большого кол-ва объектов в Json файл.

В рамках бенчмарка будут тестироваться:
* Newtonsoft.Json
* Jil
* utf8json

Json для тестов десериализации расположен в файле проекта. Для бенчмарка будет использоваться Benchmark.net

При большом кол-ве объектов JSON занимает довольно много места, так что хранить сами тестовые файлы не получится, оставил только файл на 1к записей. В проекте есть метод который генерит файл JSON с нужным кол-вом объектов, но там нужно менять путь.

В тестах считывается файл, путь для него нужно указать внутри класса-теста.

В тестах десериализации на x64 на большом кол-ве объектов из файла (50к объектов) Jil и Utf8Json показали очень похожее время, но второй жрал больше памяти, так что можно сказать, что победа за Jil в этом раунде.

Результат 1к объектов (FromString методы - сначала JSON был прочита в строку и потом уже десериализована, в тестовых данных ошибочно у всех объектов Item.Type == null)

|                        Method |      Mean |    Error |   StdDev |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|------------------------------ |----------:|---------:|---------:|----------:|----------:|----------:|----------:|
|            Use_NewtonsoftJson | 125.75 ms | 0.856 ms | 0.801 ms | 2000.0000 | 1000.0000 |         - |  13.38 MB |
|                       Use_Jil |  62.67 ms | 0.422 ms | 0.395 ms | 2000.0000 | 1000.0000 |         - |  13.33 MB |
|                  Use_Utf8Json |  67.35 ms | 0.305 ms | 0.285 ms | 2625.0000 | 1250.0000 |  750.0000 |  27.92 MB |
| Use_NewtonsoftJson_FromString | 143.14 ms | 2.801 ms | 4.277 ms | 5000.0000 | 2000.0000 | 1000.0000 |  34.13 MB |
|            Use_Jil_FromString |  77.78 ms | 1.008 ms | 0.943 ms | 5000.0000 | 2000.0000 | 1000.0000 |  34.08 MB |
|       Use_Utf8Json_FromString |  92.30 ms | 1.840 ms | 2.191 ms | 5000.0000 | 2166.6667 |  833.3333 |  39.31 MB |

Машина и подробности запуска:
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.900 (1903/May2019Update/19H1)
Intel Core i5-3570 CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  DefaultJob : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT


```
Результат для 100к объектов (Use_CustomDeserializer - использует прямолинейный самописный десериализатор, использующий JsonReader)
|                        Method |     Mean |    Error |   StdDev |       Gen 0 |       Gen 1 |     Gen 2 | Allocated |
|------------------------------ |---------:|---------:|---------:|------------:|------------:|----------:|----------:|
|            Use_NewtonsoftJson | 15.462 s | 0.0830 s | 0.0736 s | 269000.0000 |  96000.0000 | 1000.0000 |   1.57 GB |
|                       Use_Jil |  9.084 s | 0.0914 s | 0.0855 s | 268000.0000 |  96000.0000 | 1000.0000 |   1.56 GB |
|                  Use_Utf8Json |  8.787 s | 0.0302 s | 0.0282 s | 247000.0000 |  88000.0000 | 2000.0000 |   3.44 GB |
|        Use_CustomDeserializer | 20.625 s | 0.4083 s | 0.4010 s | 416000.0000 | 144000.0000 | 6000.0000 |   2.39 GB |
| Use_NewtonsoftJson_FromString | 16.517 s | 0.0498 s | 0.0466 s | 452000.0000 | 157000.0000 | 2000.0000 |    3.7 GB |
|            Use_Jil_FromString | 10.545 s | 0.0378 s | 0.0353 s | 452000.0000 | 156000.0000 | 2000.0000 |   3.69 GB |
|       Use_Utf8Json_FromString | 12.064 s | 0.0233 s | 0.0207 s | 430000.0000 | 148000.0000 | 2000.0000 |   4.27 GB |

Машина и подробности запуска:
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.900 (1903/May2019Update/19H1)
Intel Core i5-3570 CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  DefaultJob : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
```

* Тесты сериализации

Для теста использован массив объектов размером в 1000 элементов. Для utf8Json помимо стандартных методов также была проверена конвертация в массив байт. Методы ToString конвертируют объект в строку, методы ToStream - исходный массив записывается в поток, который передаётся в аргументы методов.

Параметры тестовой машины:
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.900 (1903/May2019Update/19H1)
Intel Core i5-3570 CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  DefaultJob : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT


```
Рез-ты тестов:

|                                  Method |     Mean |    Error |   StdDev |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|---------------------------------------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| Use_NewtonsoftJson_JsonConvert_ToString | 74.68 ms | 0.795 ms | 0.744 ms | 2142.8571 | 1000.0000 |  428.5714 |  21.75 MB |
|                        Use_Jil_ToString | 41.37 ms | 0.139 ms | 0.109 ms | 2416.6667 | 1416.6667 |  666.6667 |  21.74 MB |
|                   Use_Utf8Json_ToString | 32.63 ms | 0.616 ms | 0.576 ms |  656.2500 |  656.2500 |  656.2500 |  26.61 MB |
|             Use_NewtonsoftJson_ToStream | 81.85 ms | 0.143 ms | 0.127 ms | 1857.1429 | 1857.1429 | 1857.1429 |  21.25 MB |
|                        Use_Jil_ToStream | 36.98 ms | 0.616 ms | 0.576 ms | 1928.5714 | 1928.5714 | 1928.5714 |  21.09 MB |
|                   Use_Utf8Json_ToStream | 25.22 ms | 0.192 ms | 0.179 ms | 1000.0000 | 1000.0000 | 1000.0000 |  22.86 MB |
|                Use_Utf8Json_ToByteArray | 21.26 ms | 0.406 ms | 0.483 ms |  875.0000 |  875.0000 |  875.0000 |  23.32 MB |

Тесты на 100000 объектов

Параметры тестовой машины
``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.900 (1903/May2019Update/19H1)
Intel Core i5-3570 CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  DefaultJob : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT


```
Результаты тестов:

|                                  Method |    Mean |    Error |   StdDev |       Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|---------------------------------------- |--------:|---------:|---------:|------------:|-----------:|----------:|----------:|
| Use_NewtonsoftJson_JsonConvert_ToString | 7.920 s | 0.0877 s | 0.0821 s | 186000.0000 | 63000.0000 | 2000.0000 |   2.14 GB |
|                        Use_Jil_ToString | 4.745 s | 0.0061 s | 0.0051 s | 185000.0000 | 63000.0000 | 2000.0000 |   2.13 GB |
|                   Use_Utf8Json_ToString | 3.738 s | 0.0037 s | 0.0033 s |           - |          - |         - |   3.06 GB |
|             Use_NewtonsoftJson_ToStream | 8.033 s | 0.0137 s | 0.0121 s |   5000.0000 |  3000.0000 | 3000.0000 |   2.66 GB |
|                        Use_Jil_ToStream | 4.105 s | 0.0138 s | 0.0129 s |   3000.0000 |  3000.0000 | 3000.0000 |   2.62 GB |
|                   Use_Utf8Json_ToStream | 3.075 s | 0.0043 s | 0.0038 s |   3000.0000 |  3000.0000 | 3000.0000 |   2.71 GB |
|                Use_Utf8Json_ToByteArray | 2.478 s | 0.0055 s | 0.0043 s |           - |          - |         - |    2.7 GB |
