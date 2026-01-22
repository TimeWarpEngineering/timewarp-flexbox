using TimeWarp.Flexbox;
using FlexNode = TimeWarp.Flexbox.Node;

// Simple test to trace what's happening
FlexNode root = new();
root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

FlexNode child = new();
child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
root.InsertChild(child, 0);

Console.WriteLine("Before layout:");
Console.WriteLine($"  Root IsDirty: {root.IsDirty}");
Console.WriteLine($"  Child IsDirty: {child.IsDirty}");
Console.WriteLine($"  Child Layout Width: {child.Layout.GetDimension(Dimension.Width)}");
Console.WriteLine($"  Child Layout Height: {child.Layout.GetDimension(Dimension.Height)}");
Console.WriteLine($"  Child Layout ComputedFlexBasis: {child.Layout.ComputedFlexBasis}");

Console.WriteLine("\nRunning layout...");
CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

Console.WriteLine("\nAfter layout:");
Console.WriteLine($"  Root IsDirty: {root.IsDirty}");
Console.WriteLine($"  Child IsDirty: {child.IsDirty}");
Console.WriteLine($"  Child Layout Width: {child.Layout.GetDimension(Dimension.Width)}");
Console.WriteLine($"  Child Layout Height: {child.Layout.GetDimension(Dimension.Height)}");
Console.WriteLine($"  Child Layout Left: {child.Layout.GetPosition(PhysicalEdge.Left)}");
Console.WriteLine($"  Child Layout Top: {child.Layout.GetPosition(PhysicalEdge.Top)}");
Console.WriteLine($"  Child Layout ComputedFlexBasis: {child.Layout.ComputedFlexBasis}");
Console.WriteLine($"  Child MeasuredWidth: {child.Layout.GetMeasuredDimension(Dimension.Width)}");
Console.WriteLine($"  Child MeasuredHeight: {child.Layout.GetMeasuredDimension(Dimension.Height)}");
Console.WriteLine($"  Child GenerationCount: {child.Layout.GenerationCount}");
