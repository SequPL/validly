```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2314)
AMD Ryzen 7 PRO 8840HS w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.404
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=.NET 8.0  Runtime=.NET 8.0  MaxIterationCount=20  

```
| Method               | NumberOfInvalidValues | Mean        | Error      | StdDev     | Median      | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|--------------------- |---------------------- |------------:|-----------:|-----------:|------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **DataAnnotation**       | **all**                   | **1,667.16 ns** |  **49.264 ns** |  **52.712 ns** | **1,652.16 ns** |  **8.78** |    **0.61** | **0.3834** | **0.0019** |    **3208 B** |          **NA** |
| Validly              | all                   |   190.63 ns |  10.146 ns |  11.684 ns |   196.03 ns |  1.00 |    0.09 |      - |      - |         - |          NA |
| &#39;Validot (IsValid)&#39;  | all                   |    56.41 ns |   0.315 ns |   0.263 ns |    56.40 ns |  0.30 |    0.02 | 0.0057 |      - |      48 B |          NA |
| &#39;Validot (Validate)&#39; | all                   | 1,056.36 ns |  82.549 ns |  88.326 ns | 1,048.69 ns |  5.56 |    0.57 | 0.3948 | 0.0038 |    3304 B |          NA |
| FluentValidation     | all                   | 5,948.46 ns | 113.619 ns | 106.279 ns | 5,927.95 ns | 31.32 |    2.02 | 1.6632 | 0.0229 |   13928 B |          NA |
|                      |                       |             |            |            |             |       |         |        |        |           |             |
| **DataAnnotation**       | **none**                  | **1,677.16 ns** |  **30.304 ns** |  **25.305 ns** | **1,678.70 ns** |  **7.08** |    **0.20** | **0.3834** | **0.0019** |    **3208 B** |          **NA** |
| Validly              | none                  |   237.16 ns |   5.687 ns |   6.085 ns |   235.36 ns |  1.00 |    0.03 |      - |      - |         - |          NA |
| &#39;Validot (IsValid)&#39;  | none                  |   316.64 ns |   1.869 ns |   1.748 ns |   317.01 ns |  1.34 |    0.03 | 0.0057 |      - |      48 B |          NA |
| &#39;Validot (Validate)&#39; | none                  |   759.69 ns |  68.972 ns |  79.429 ns |   813.60 ns |  3.21 |    0.34 | 0.2842 | 0.0019 |    2384 B |          NA |
| FluentValidation     | none                  |   697.55 ns |   6.102 ns |   5.095 ns |   697.81 ns |  2.94 |    0.07 | 0.0906 |      - |     760 B |          NA |
|                      |                       |             |            |            |             |       |         |        |        |           |             |
| **DataAnnotation**       | **one**                   | **1,589.07 ns** |  **14.376 ns** |  **13.447 ns** | **1,590.97 ns** |  **7.02** |    **0.06** | **0.3357** |      **-** |    **2808 B** |          **NA** |
| Validly              | one                   |   226.34 ns |   0.981 ns |   0.918 ns |   225.97 ns |  1.00 |    0.01 |      - |      - |         - |          NA |
| &#39;Validot (IsValid)&#39;  | one                   |    66.53 ns |   0.272 ns |   0.241 ns |    66.46 ns |  0.29 |    0.00 | 0.0057 |      - |      48 B |          NA |
| &#39;Validot (Validate)&#39; | one                   |   865.82 ns |   7.534 ns |   6.678 ns |   865.97 ns |  3.83 |    0.03 | 0.3271 | 0.0029 |    2736 B |          NA |
| FluentValidation     | one                   | 3,032.77 ns |  18.918 ns |  17.696 ns | 3,035.72 ns | 13.40 |    0.09 | 0.6523 | 0.0038 |    5456 B |          NA |
