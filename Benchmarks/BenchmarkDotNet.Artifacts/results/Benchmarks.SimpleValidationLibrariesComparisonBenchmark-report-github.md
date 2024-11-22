```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2314)
Unknown processor
.NET SDK 8.0.404
  [Host]               : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0             : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET Framework 4.7.2 : .NET Framework 4.8.1 (4.8.9282.0), X64 RyuJIT VectorSize=256

MaxIterationCount=20  

```
| Method               | Job                  | Runtime              | NumberOfInvalidValues | Mean         | Error      | StdDev     | Ratio | RatioSD | Gen0   | Gen1   | Gen2   | Allocated | Alloc Ratio |
|--------------------- |--------------------- |--------------------- |---------------------- |-------------:|-----------:|-----------:|------:|--------:|-------:|-------:|-------:|----------:|------------:|
| **Valigator**            | **.NET 8.0**             | **.NET 8.0**             | **all**                   |    **395.22 ns** |  **29.521 ns** |  **32.812 ns** |  **0.67** |    **0.07** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator            | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   |    592.47 ns |  31.381 ns |  34.879 ns |  1.00 |    0.08 | 0.0105 | 0.0048 | 0.0010 |      63 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| DataAnnotation       | .NET 8.0             | .NET 8.0             | all                   |  1,656.12 ns | 102.219 ns | 113.616 ns |  0.25 |    0.02 | 0.3834 | 0.0019 |      - |    3208 B |        0.50 |
| DataAnnotation       | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   |  6,506.08 ns | 198.577 ns | 228.682 ns |  1.00 |    0.05 | 1.0071 | 0.0076 |      - |    6371 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| &#39;Validot (IsValid)&#39;  | .NET 8.0             | .NET 8.0             | all                   |     56.72 ns |   1.104 ns |   1.033 ns |  0.45 |    0.01 | 0.0057 |      - |      - |      48 B |        1.00 |
| &#39;Validot (IsValid)&#39;  | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   |    124.87 ns |   2.459 ns |   2.415 ns |  1.00 |    0.03 | 0.0076 |      - |      - |      48 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| &#39;Validot (Validate)&#39; | .NET 8.0             | .NET 8.0             | all                   |  1,146.42 ns |  88.287 ns | 101.671 ns |  0.62 |    0.06 | 0.3948 | 0.0038 |      - |    3304 B |        0.98 |
| &#39;Validot (Validate)&#39; | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   |  1,841.58 ns |  80.004 ns |  92.132 ns |  1.00 |    0.07 | 0.5379 | 0.0038 |      - |    3386 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| FluentValidation     | .NET 8.0             | .NET 8.0             | all                   |  7,049.77 ns | 173.199 ns | 185.321 ns |  0.56 |    0.03 | 1.6632 | 0.0229 |      - |   13928 B |        0.77 |
| FluentValidation     | .NET Framework 4.7.2 | .NET Framework 4.7.2 | all                   | 12,613.47 ns | 507.596 ns | 543.122 ns |  1.00 |    0.06 | 2.8381 | 0.0305 |      - |   17997 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| **Valigator**            | **.NET 8.0**             | **.NET 8.0**             | **none**                  |    **411.79 ns** |   **6.209 ns** |   **5.504 ns** |  **0.70** |    **0.04** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator            | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  |    586.99 ns |  33.335 ns |  37.051 ns |  1.00 |    0.09 | 0.0105 | 0.0048 | 0.0010 |      63 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| DataAnnotation       | .NET 8.0             | .NET 8.0             | none                  |  1,847.46 ns |  43.855 ns |  45.036 ns |  0.31 |    0.01 | 0.3834 | 0.0019 |      - |    3208 B |        0.50 |
| DataAnnotation       | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  |  5,941.03 ns |  73.036 ns |  60.988 ns |  1.00 |    0.01 | 1.0071 | 0.0076 |      - |    6371 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| &#39;Validot (IsValid)&#39;  | .NET 8.0             | .NET 8.0             | none                  |    287.03 ns |   5.329 ns |   5.472 ns |  0.43 |    0.03 | 0.0057 |      - |      - |      48 B |        1.00 |
| &#39;Validot (IsValid)&#39;  | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  |    675.05 ns |  44.224 ns |  50.929 ns |  1.01 |    0.11 | 0.0076 |      - |      - |      48 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| &#39;Validot (Validate)&#39; | .NET 8.0             | .NET 8.0             | none                  |    819.89 ns |  47.457 ns |  54.652 ns |  0.55 |    0.04 | 0.2842 | 0.0019 |      - |    2384 B |        0.99 |
| &#39;Validot (Validate)&#39; | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  |  1,484.18 ns |  36.943 ns |  42.543 ns |  1.00 |    0.04 | 0.3834 | 0.0019 |      - |    2415 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| FluentValidation     | .NET 8.0             | .NET 8.0             | none                  |    798.46 ns |  15.327 ns |  15.054 ns |  0.52 |    0.01 | 0.0906 |      - |      - |     760 B |        0.98 |
| FluentValidation     | .NET Framework 4.7.2 | .NET Framework 4.7.2 | none                  |  1,533.92 ns |  18.224 ns |  15.218 ns |  1.00 |    0.01 | 0.1221 |      - |      - |     778 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| **Valigator**            | **.NET 8.0**             | **.NET 8.0**             | **one**                   |    **403.05 ns** |   **8.139 ns** |   **9.046 ns** |  **0.69** |    **0.02** |      **-** |      **-** |      **-** |         **-** |        **0.00** |
| Valigator            | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   |    586.39 ns |  11.231 ns |  12.017 ns |  1.00 |    0.03 | 0.0105 | 0.0048 | 0.0010 |      63 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| DataAnnotation       | .NET 8.0             | .NET 8.0             | one                   |  1,560.35 ns |  83.501 ns |  96.160 ns |  0.31 |    0.02 | 0.3357 |      - |      - |    2808 B |        0.52 |
| DataAnnotation       | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   |  5,004.08 ns | 111.559 ns | 123.998 ns |  1.00 |    0.03 | 0.8621 | 0.0076 |      - |    5440 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| &#39;Validot (IsValid)&#39;  | .NET 8.0             | .NET 8.0             | one                   |     63.82 ns |   1.222 ns |   1.143 ns |  0.53 |    0.01 | 0.0057 |      - |      - |      48 B |        1.00 |
| &#39;Validot (IsValid)&#39;  | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   |    120.73 ns |   2.168 ns |   2.409 ns |  1.00 |    0.03 | 0.0076 |      - |      - |      48 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| &#39;Validot (Validate)&#39; | .NET 8.0             | .NET 8.0             | one                   |    877.64 ns |  54.443 ns |  55.909 ns |  0.60 |    0.04 | 0.3271 | 0.0029 |      - |    2736 B |        0.99 |
| &#39;Validot (Validate)&#39; | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   |  1,451.14 ns |  28.349 ns |  25.131 ns |  1.00 |    0.02 | 0.4406 | 0.0038 |      - |    2776 B |        1.00 |
|                      |                      |                      |                       |              |            |            |       |         |        |        |        |           |             |
| FluentValidation     | .NET 8.0             | .NET 8.0             | one                   |  2,885.13 ns |  56.387 ns |  49.986 ns |  0.57 |    0.01 | 0.6523 | 0.0038 |      - |    5456 B |        0.80 |
| FluentValidation     | .NET Framework 4.7.2 | .NET Framework 4.7.2 | one                   |  5,087.52 ns |  76.130 ns |  71.212 ns |  1.00 |    0.02 | 1.0834 | 0.0076 |      - |    6836 B |        1.00 |
