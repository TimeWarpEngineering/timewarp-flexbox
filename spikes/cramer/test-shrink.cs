#!/usr/bin/dotnet run
#:project ../../source/timewarp-flexbox/timewarp-flexbox.csproj

using TimeWarp.Flexbox;

FlexLayoutEngine engine = new();

FlexNode root = new()
{
  Width = FlexValue.Point(200),
  Height = FlexValue.Point(100),
  FlexDirection = FlexDirection.Row
};

FlexNode child1 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };
FlexNode child2 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };

root.AddChild(child1);
root.AddChild(child2);

Console.WriteLine($"Before layout:");
Console.WriteLine($"  child1 Width style: {child1.Width}");
Console.WriteLine($"  child2 Width style: {child2.Width}");
Console.WriteLine($"  child1 FlexShrink: {child1.FlexShrink}");
Console.WriteLine($"  child2 FlexShrink: {child2.FlexShrink}");

engine.CalculateLayout(root, 200, 100);

Console.WriteLine($"After layout:");
Console.WriteLine($"  child1 Layout.Width: {child1.Layout.Width}");
Console.WriteLine($"  child2 Layout.Width: {child2.Layout.Width}");
Console.WriteLine($"  root Layout.Width: {root.Layout.Width}");
Console.WriteLine($"  Expected: each child should be 100 (shrink from 150)");
