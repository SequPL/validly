```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2314)
Unknown processor
.NET SDK 8.0.404
  [Host]               : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0             : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET Framework 4.7.2 : .NET Framework 4.8.1 (4.8.9282.0), X64 RyuJIT VectorSize=256

MaxIterationCount=20  

```
| Method         | Job                  | Runtime              | NumberOfInvalidValues | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Gen2   | Allocated | Alloc Ratio |
|--------------- |--------------------- |--------------------- |---------------------- |-----------:|----------:|----------:|------:|--------:|-------:|-------:|-------:|----------:|------------:|
| **Valigator**      | **.NET 8.0**             | **.NET 8.0**             | **all**                   |   **477.3 ns** |   **2.86 ns** |   **2.39 ns** |  **0.76** |    **0.01** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator      | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   |   631.9 ns |   6.55 ns |   5.12 ns |  1.00 |    0.01 | 0.0124 | 0.0057 | 0.0010 |      75 B |        1.00 |
|                |                      |                      |                       |            |           |           |       |         |        |        |        |           |             |
| DataAnnotation | .NET 8.0             | .NET 8.0             | all                   | 1,692.8 ns |  16.77 ns |  14.01 ns |  0.22 |    0.00 | 0.3834 | 0.0019 |      - |    3208 B |        0.50 |
| DataAnnotation | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   | 7,737.0 ns |  71.85 ns |  60.00 ns |  1.00 |    0.01 | 1.0071 | 0.0076 |      - |    6371 B |        1.00 |
|                |                      |                      |                       |            |           |           |       |         |        |        |        |           |             |
| **Valigator**      | **.NET 8.0**             | **.NET 8.0**             | **none**                  |   **630.3 ns** |   **6.79 ns** |   **5.30 ns** |  **0.81** |    **0.01** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator      | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  |   777.9 ns |   8.62 ns |   7.20 ns |  1.00 |    0.01 | 0.0124 | 0.0057 | 0.0010 |      75 B |        1.00 |
|                |                      |                      |                       |            |           |           |       |         |        |        |        |           |             |
| DataAnnotation | .NET 8.0             | .NET 8.0             | none                  | 2,452.2 ns | 229.95 ns | 264.81 ns |  0.32 |    0.03 | 0.3815 |      - |      - |    3208 B |        0.50 |
| DataAnnotation | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  | 7,784.1 ns |  76.63 ns |  59.83 ns |  1.00 |    0.01 | 1.0071 |      - |      - |    6371 B |        1.00 |
|                |                      |                      |                       |            |           |           |       |         |        |        |        |           |             |
| **Valigator**      | **.NET 8.0**             | **.NET 8.0**             | **one**                   |   **581.2 ns** |   **6.53 ns** |   **5.46 ns** |  **0.74** |    **0.01** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator      | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   |   784.0 ns |  12.01 ns |   9.37 ns |  1.00 |    0.02 | 0.0124 | 0.0057 | 0.0010 |      75 B |        1.00 |
|                |                      |                      |                       |            |           |           |       |         |        |        |        |           |             |
| DataAnnotation | .NET 8.0             | .NET 8.0             | one                   | 1,743.9 ns |  18.64 ns |  17.44 ns |  0.29 |    0.01 | 0.3357 |      - |      - |    2808 B |        0.52 |
| DataAnnotation | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   | 6,082.0 ns | 110.14 ns | 117.84 ns |  1.00 |    0.03 | 0.8621 | 0.0076 |      - |    5440 B |        1.00 |
