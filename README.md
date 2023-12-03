# List Hashes Performance Test

## Result

```
BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3636/22H2/2022Update)
Intel Core i7-10700 CPU 2.90GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET 7.0.13 (7.0.1323.51816), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.13 (7.0.1323.51816), X64 RyuJIT AVX2
```

| Method           | Mean      | Error    | StdDev   | Allocated |
|----------------- |----------:|---------:|---------:|----------:|
| WithLocksTest    | 159.02 ms | 0.581 ms | 0.515 ms | 264.01 KB |
| WithoutLocksTest |  33.92 ms | 0.674 ms | 0.988 ms | 274.52 KB |
