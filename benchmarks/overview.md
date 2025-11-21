# TimeWarp.Flexbox Benchmarks

This directory contains performance benchmarking projects for TimeWarp.Flexbox using BenchmarkDotNet.

## Purpose

Benchmarks measure and track:
- Component rendering performance
- Virtual DOM diffing efficiency
- Memory allocation patterns
- Startup and initialization time
- Comparison with direct Terminal.Gui usage

## Why Benchmark?

Performance is critical for TUI frameworks:
- **Responsiveness** - Users expect instant feedback in terminal apps
- **Memory efficiency** - Terminal apps often run in resource-constrained environments
- **Rendering speed** - Smooth updates require fast diffing and rendering
- **Scalability** - Performance with deep component trees

## Benchmark Projects

### TimeWarp.Flexbox.Benchmarks

Main benchmark project comparing:
- Flexbox component rendering vs. direct Terminal.Gui
- Different rendering strategies (with/without diffing)
- Component lifecycle overhead
- State management performance

## Running Benchmarks

### Quick Run
```bash
cd benchmarks/TimeWarp.Flexbox.Benchmarks
dotnet run -c Release
```

### With HTML Report
```bash
cd benchmarks/TimeWarp.Flexbox.Benchmarks
dotnet run -c Release -- --exporters html
```

### Run Specific Benchmarks
```bash
cd benchmarks/TimeWarp.Flexbox.Benchmarks
dotnet run -c Release -- --filter "*Rendering*"
```

### Memory Diagnostics
```bash
cd benchmarks/TimeWarp.Flexbox.Benchmarks
dotnet run -c Release -- --memory
```

## Benchmark Scenarios

### Component Rendering
Measures time to:
1. Create component instance
2. Set parameters
3. Invoke lifecycle methods
4. Generate render tree
5. Apply to Terminal.Gui

### Virtual DOM Diffing
Compares:
- Initial render (no previous tree)
- Update with no changes
- Update with small changes
- Update with large changes
- Complete tree replacement

### Memory Allocation
Tracks allocations during:
- Component creation
- Parameter binding
- Render tree generation
- Diffing operations
- Terminal.Gui updates

## Understanding Results

### Mean (Execution Time)
Average time in microseconds (μs) or milliseconds (ms):
- **< 1 ms**: Excellent - imperceptible to users
- **< 10 ms**: Good - feels instant
- **< 50 ms**: Acceptable - still responsive
- **> 50 ms**: May feel sluggish

### Allocated (Memory)
Total heap allocations:
- **0 B**: Perfect - no allocations
- **< 1 KB**: Excellent - minimal GC pressure
- **< 10 KB**: Good for complex operations
- **> 100 KB**: May need optimization

### Ratio
Relative to baseline (typically direct Terminal.Gui):
- **1.00**: Same as baseline
- **< 2.00**: Acceptable overhead
- **> 5.00**: Significant overhead, investigate

## Baseline Comparisons

Benchmarks compare Flexbox against:
- **Direct Terminal.Gui** - Raw performance ceiling
- **Previous Flexbox versions** - Track regressions
- **Other TUI frameworks** - Industry context (if available)

## Continuous Monitoring

Run benchmarks:
- Before major releases
- After performance optimizations
- When adding new features
- Investigating performance issues

Store results in `results/` with timestamps for tracking trends.

## Optimization Workflow

1. **Identify** - Run benchmarks to find bottlenecks
2. **Hypothesize** - Form theory about cause
3. **Optimize** - Make targeted changes
4. **Measure** - Re-run benchmarks
5. **Verify** - Confirm improvement without regressions
6. **Document** - Record findings

## Best Practices

### Writing Benchmarks

```csharp
[MemoryDiagnoser]
public class ComponentRenderingBenchmarks
{
  private MyComponent Component;

  [GlobalSetup]
  public void Setup()
  {
    Component = new MyComponent();
  }

  [Benchmark(Baseline = true)]
  public void DirectTerminalGui()
  {
    // Direct Terminal.Gui rendering
  }

  [Benchmark]
  public void FlexboxComponent()
  {
    // Flexbox component rendering
  }
}
```

### Guidelines
- Use `[MemoryDiagnoser]` to track allocations
- Set baseline with `Baseline = true`
- Use `[GlobalSetup]` for expensive initialization
- Keep benchmarks focused on one aspect
- Run in Release mode only
- Multiple iterations for accuracy

## Performance Goals

### Target Metrics
- **Component render**: < 1ms for simple components
- **Diffing**: < 5ms for typical trees (100 nodes)
- **Memory**: < 5KB per component instance
- **Overhead vs. Terminal.Gui**: < 2x

### Acceptable Trade-offs
- Developer experience over raw performance
- Type safety over speed (within reason)
- Maintainability over micro-optimizations

## Results Storage

Store benchmark results with context:
```
results/
├── 2025-10-15-baseline.md
├── 2025-11-01-diffing-optimization.md
└── 2025-12-15-memory-improvements.md
```

Include:
- Date and version
- Hardware specs
- Key findings
- Comparisons to previous runs
- Action items

## Related Documentation

- [Design](../documentation/developer/design/) - Performance design goals
- [Standards](../documentation/developer/standards/) - Code quality standards
- [Roadmap](../documentation/developer/roadmap/) - Planned optimizations
