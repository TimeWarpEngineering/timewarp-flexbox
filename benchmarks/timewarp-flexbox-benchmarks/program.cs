namespace TimeWarp.Flexbox.Benchmarks;

using BenchmarkDotNet.Running;

/// <summary>
/// Benchmark entry point.
/// </summary>
public static class Program
{
  /// <summary>
  /// Runs the layout benchmarks.
  /// </summary>
  public static void Main() => BenchmarkRunner.Run<LayoutBenchmarks>();
}
