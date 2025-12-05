# Task 027-implement-debug-and-print-utilities

## Summary
Implement debugging utilities for visualizing node trees and layout results.

## Todo List
- [ ] Create Debug/FlexNodePrinter.cs class
- [ ] Implement Print(FlexNode root) -> string with tree visualization
- [ ] Show node style properties in output
- [ ] Show layout results (position, size) in output
- [ ] Add indentation for child hierarchy
- [ ] Implement PrintLayout(FlexNode root) for layout-only output
- [ ] Add optional verbose mode with all properties
- [ ] Add JSON serialization for layout results
- [ ] Verify code follows csharp-coding.md standards

## Notes
Debug output helps with development and troubleshooting:

```csharp
public static class FlexNodePrinter
{
  public static string Print(FlexNode node, bool includeLayout = true)
  {
    StringBuilder sb = new();
    PrintNode(sb, node, 0, includeLayout);
    return sb.ToString();
  }
  
  private static void PrintNode(StringBuilder sb, FlexNode node, int depth, bool includeLayout)
  {
    string indent = new(' ', depth * 2);
    
    sb.AppendLine($"{indent}<FlexNode");
    sb.AppendLine($"{indent}  Direction={node.FlexDirection}");
    sb.AppendLine($"{indent}  Wrap={node.FlexWrap}");
    
    if (node.Width.Unit != Unit.Undefined)
      sb.AppendLine($"{indent}  Width={node.Width}");
    if (node.Height.Unit != Unit.Undefined)
      sb.AppendLine($"{indent}  Height={node.Height}");
    
    if (includeLayout)
    {
      sb.AppendLine($"{indent}  Layout={{");
      sb.AppendLine($"{indent}    Left={node.Layout.Left}");
      sb.AppendLine($"{indent}    Top={node.Layout.Top}");
      sb.AppendLine($"{indent}    Width={node.Layout.Width}");
      sb.AppendLine($"{indent}    Height={node.Layout.Height}");
      sb.AppendLine($"{indent}  }}");
    }
    
    sb.AppendLine($"{indent}>");
    
    foreach (FlexNode child in node.Children)
      PrintNode(sb, child, depth + 1, includeLayout);
    
    sb.AppendLine($"{indent}</FlexNode>");
  }
}
```

Usage:
```csharp
root.CalculateLayout(800, 600);
Console.WriteLine(FlexNodePrinter.Print(root));
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
