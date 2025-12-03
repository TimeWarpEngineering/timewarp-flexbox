# Task 030-implement-benchmarks

## Summary
Create BenchmarkDotNet benchmarks to measure layout performance and compare with Yoga.

## Todo List
- [ ] Create benchmarks/TimeWarp.Flexbox.Benchmarks project
- [ ] Add BenchmarkDotNet package reference
- [ ] Create benchmark for simple layout (few nodes)
- [ ] Create benchmark for deep nested layout
- [ ] Create benchmark for wide layout (many siblings)
- [ ] Create benchmark for flex-grow/shrink calculations
- [ ] Create benchmark for wrapping layouts
- [ ] Create benchmark with measurement callbacks
- [ ] Create benchmark for layout caching effectiveness
- [ ] Add memory allocation benchmarks
- [ ] Document baseline performance numbers
- [ ] Add benchmarks to CI (optional)
- [ ] Verify code follows csharp-coding.md standards

## Notes
Benchmark project setup:

```csharp
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net100)]
public class LayoutBenchmarks
{
  private FlexNode simpleRoot;
  private FlexNode deepRoot;
  private FlexNode wideRoot;
  
  [GlobalSetup]
  public void Setup()
  {
    // Simple: root with 3 children
    simpleRoot = CreateSimpleLayout();
    
    // Deep: 10 levels of nesting
    deepRoot = CreateDeepLayout(10);
    
    // Wide: root with 100 children
    wideRoot = CreateWideLayout(100);
  }
  
  [Benchmark(Baseline = true)]
  public void SimpleLayout()
  {
    simpleRoot.CalculateLayout(800, 600);
  }
  
  [Benchmark]
  public void DeepLayout()
  {
    deepRoot.CalculateLayout(800, 600);
  }
  
  [Benchmark]
  public void WideLayout()
  {
    wideRoot.CalculateLayout(800, 600);
  }
  
  [Benchmark]
  public void CachedLayout()
  {
    // Layout already calculated, should hit cache
    simpleRoot.CalculateLayout(800, 600);
  }
}
```

Target metrics:
- Simple layout: < 1 microsecond
- Deep layout (10 levels): < 10 microseconds
- Wide layout (100 children): < 50 microseconds

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
