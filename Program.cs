
using System.Security.Cryptography;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ListHashesPerformanceTest>();

[MemoryDiagnoser]
public class ListHashesPerformanceTest
{
    private static readonly string TargetDirPath = AppContext.BaseDirectory;
    private static readonly HashAlgorithm HashAlgorithm = SHA1.Create();

    [Benchmark]
    public List<(string, string)> WithLocksTest()
    {
        return Test(ComputeHashWithLocks);
    }

    [Benchmark]
    public List<(string, string)> WithoutLocksTest()
    {
        return Test(ComputeHashWithoutLocks);
    }

    private static List<(string, string)> Test(Func<string, byte[]> computeHash)
    {
        return Directory.EnumerateFiles(TargetDirPath, "*", SearchOption.AllDirectories).AsParallel()
            .Select(filePath => (filePath, ToHexString(computeHash(filePath)))).OrderBy(t => t.filePath).ToList();
    }

    private static byte[] ComputeHashWithLocks(string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            lock (HashAlgorithm)
            {
                return HashAlgorithm.ComputeHash(stream);
            }
        }
    }

    private static byte[] ComputeHashWithoutLocks(string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var hashAlgorithm = SHA1.Create())
        {
            return hashAlgorithm.ComputeHash(stream);
        }
    }

    private static string ToHexString(byte[] bytes)
    {
        //return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
        return Convert.ToHexString(bytes).ToLower();
    }
}
