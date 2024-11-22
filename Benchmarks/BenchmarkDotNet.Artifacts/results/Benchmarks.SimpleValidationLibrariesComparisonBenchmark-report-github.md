```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2314)
Unknown processor
.NET SDK 8.0.404
  [Host]               : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0             : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET Framework 4.7.2 : .NET Framework 4.8.1 (4.8.9282.0), X64 RyuJIT VectorSize=256

MaxIterationCount=20  

```
| Method           | Job                  | Runtime              | NumberOfInvalidValues | Mean        | Error       | StdDev      | Ratio | RatioSD | Gen0   | Gen1   | Gen2   | Allocated | Alloc Ratio |
|----------------- |--------------------- |--------------------- |---------------------- |------------:|------------:|------------:|------:|--------:|-------:|-------:|-------:|----------:|------------:|
| **Valigator**        | **.NET 8.0**             | **.NET 8.0**             | **all**                   |    **522.5 ns** |    **52.47 ns** |    **60.43 ns** |  **0.86** |    **0.10** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator        | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   |    605.9 ns |    18.42 ns |    21.21 ns |  1.00 |    0.05 | 0.0114 | 0.0057 | 0.0019 |      63 B |        1.00 |
|                  |                      |                      |                       |             |             |             |       |         |        |        |        |           |             |
| DataAnnotation   | .NET 8.0             | .NET 8.0             | all                   |  2,004.3 ns |   310.32 ns |   344.92 ns |  0.31 |    0.05 | 0.3834 | 0.0019 |      - |    3208 B |        0.50 |
| DataAnnotation   | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   |  6,554.1 ns |   184.00 ns |   196.88 ns |  1.00 |    0.04 | 1.0071 | 0.0076 |      - |    6371 B |        1.00 |
|                  |                      |                      |                       |             |             |             |       |         |        |        |        |           |             |
| FluentValidation | .NET 8.0             | .NET 8.0             | all                   | 14,807.4 ns | 1,564.38 ns | 1,673.86 ns |  0.49 |    0.08 | 3.4180 | 0.0610 |      - |   28616 B |        0.81 |
| FluentValidation | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   | 30,484.8 ns | 3,560.95 ns | 3,810.18 ns |  1.01 |    0.17 | 5.6152 | 0.1526 |      - |   35349 B |        1.00 |
|                  |                      |                      |                       |             |             |             |       |         |        |        |        |           |             |
| **Valigator**        | **.NET 8.0**             | **.NET 8.0**             | **none**                  |    **470.3 ns** |    **41.12 ns** |    **44.00 ns** |  **0.78** |    **0.09** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator        | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  |    609.2 ns |    50.38 ns |    51.74 ns |  1.01 |    0.11 | 0.0105 | 0.0048 | 0.0010 |      63 B |        1.00 |
|                  |                      |                      |                       |             |             |             |       |         |        |        |        |           |             |
| DataAnnotation   | .NET 8.0             | .NET 8.0             | none                  |  2,023.8 ns |   185.68 ns |   213.82 ns |  0.29 |    0.04 | 0.3815 |      - |      - |    3208 B |        0.50 |
| DataAnnotation   | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  |  7,107.5 ns |   762.71 ns |   847.75 ns |  1.01 |    0.16 | 1.0071 | 0.0076 |      - |    6371 B |        1.00 |
|                  |                      |                      |                       |             |             |             |       |         |        |        |        |           |             |
| FluentValidation | .NET 8.0             | .NET 8.0             | none                  |  6,365.5 ns |   237.09 ns |   263.53 ns |  0.40 |    0.02 | 1.8311 | 0.0305 |      - |   15448 B |        0.85 |
| FluentValidation | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  | 15,864.4 ns |   370.22 ns |   396.14 ns |  1.00 |    0.03 | 2.8687 | 0.0610 |      - |   18129 B |        1.00 |
|                  |                      |                      |                       |             |             |             |       |         |        |        |        |           |             |
| **Valigator**        | **.NET 8.0**             | **.NET 8.0**             | **one**                   |    **498.6 ns** |    **43.89 ns** |    **46.96 ns** |  **0.74** |    **0.09** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator        | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   |    680.9 ns |    59.15 ns |    60.74 ns |  1.01 |    0.12 | 0.0105 | 0.0048 | 0.0010 |      63 B |        1.00 |
|                  |                      |                      |                       |             |             |             |       |         |        |        |        |           |             |
| DataAnnotation   | .NET 8.0             | .NET 8.0             | one                   |  1,802.9 ns |   449.29 ns |   517.40 ns |  0.35 |    0.10 | 0.3357 |      - |      - |    2808 B |        0.52 |
| DataAnnotation   | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   |  5,112.7 ns |   239.80 ns |   235.52 ns |  1.00 |    0.06 | 0.8621 | 0.0076 |      - |    5440 B |        1.00 |
|                  |                      |                      |                       |             |             |             |       |         |        |        |        |           |             |
| FluentValidation | .NET 8.0             | .NET 8.0             | one                   |  9,845.9 ns |   954.90 ns | 1,099.67 ns |  0.45 |    0.05 | 2.3804 | 0.0610 |      - |   20144 B |        0.83 |
| FluentValidation | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   | 21,795.4 ns | 1,082.97 ns | 1,112.13 ns |  1.00 |    0.07 | 3.7842 |      - |      - |   24184 B |        1.00 |
