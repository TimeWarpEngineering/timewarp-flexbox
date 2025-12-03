namespace TimeWarp.Flexbox.Tests.Debug;

using TimeWarp.Flexbox.Debug;

/// <summary>
/// Tests for FlexNodePrinter.Print method.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodePrinterPrintTests
{
  public void ShouldThrowWhenNodeIsNull()
  {
    Should.Throw<ArgumentNullException>(() => FlexNodePrinter.Print(null!));
  }

  public void ShouldIncludeFlexNodeTags()
  {
    FlexNode node = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(50) };

    string result = FlexNodePrinter.Print(node);

    result.ShouldContain("<FlexNode");
    result.ShouldContain("</FlexNode>");
  }

  public void ShouldIncludeLayoutByDefault()
  {
    FlexNode node = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(50) };
    FlexLayoutEngine engine = new();
    engine.CalculateLayout(node, 100, 50);

    string result = FlexNodePrinter.Print(node);

    result.ShouldContain("Layout={");
    result.ShouldContain("Width=100.00");
    result.ShouldContain("Height=50.00");
  }

  public void ShouldExcludeLayoutWhenRequested()
  {
    FlexNode node = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(50) };

    string result = FlexNodePrinter.Print(node, includeLayout: false);

    result.ShouldNotContain("Layout={");
  }

  public void ShouldIncludeNonDefaultPropertiesOnly()
  {
    FlexNode node = new()
    {
      FlexDirection = FlexDirection.Column,
      JustifyContent = JustifyContent.Center
    };

    string result = FlexNodePrinter.Print(node, includeLayout: false);

    result.ShouldContain("FlexDirection=Column");
    result.ShouldContain("JustifyContent=Center");
    result.ShouldNotContain("FlexWrap="); // Default is NoWrap, should be excluded
  }

  public void ShouldIncludeAllPropertiesWhenVerbose()
  {
    FlexNode node = new();

    string result = FlexNodePrinter.Print(node, includeLayout: false, verbose: true);

    result.ShouldContain("FlexDirection=Row");
    result.ShouldContain("FlexWrap=NoWrap");
    result.ShouldContain("JustifyContent=FlexStart");
    result.ShouldContain("AlignItems=Stretch");
  }

  public void ShouldPrintChildNodes()
  {
    FlexNode root = new() { Width = FlexValue.Point(200), Height = FlexValue.Point(100) };
    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    string result = FlexNodePrinter.Print(root, includeLayout: false);

    // Should have two FlexNode tags (root and child)
    int openTagCount = result.Split("<FlexNode").Length - 1;
    int closeTagCount = result.Split("</FlexNode>").Length - 1;

    openTagCount.ShouldBe(2);
    closeTagCount.ShouldBe(2);
  }

  public void ShouldIndentNestedNodes()
  {
    FlexNode root = new();
    FlexNode child = new();
    FlexNode grandchild = new();

    root.AddChild(child);
    child.AddChild(grandchild);

    string result = FlexNodePrinter.Print(root, includeLayout: false);

    // Check that indentation increases for nested nodes
    string[] lines = result.Split('\n');

    // Find the grandchild line - it should have more leading spaces
    bool foundDeeplyIndented = false;
    foreach (string line in lines)
    {
      if (line.StartsWith("    <FlexNode", StringComparison.Ordinal)) // 4 spaces = depth 2
      {
        foundDeeplyIndented = true;
        break;
      }
    }

    foundDeeplyIndented.ShouldBeTrue();
  }
}

/// <summary>
/// Tests for FlexNodePrinter.PrintLayout method.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodePrinterPrintLayoutTests
{
  public void ShouldThrowWhenNodeIsNull()
  {
    Should.Throw<ArgumentNullException>(() => FlexNodePrinter.PrintLayout(null!));
  }

  public void ShouldPrintLayoutInBracketFormat()
  {
    FlexNode node = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(50) };
    FlexLayoutEngine engine = new();
    engine.CalculateLayout(node, 100, 50);

    string result = FlexNodePrinter.PrintLayout(node);

    result.ShouldContain("[0.00, 0.00, 100.00, 50.00]");
  }

  public void ShouldPrintChildLayoutsIndented()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode child2 = new() { Width = FlexValue.Point(100) };

    root.AddChild(child1);
    root.AddChild(child2);

    FlexLayoutEngine engine = new();
    engine.CalculateLayout(root, 200, 100);

    string result = FlexNodePrinter.PrintLayout(root);
    string[] lines = result.Trim().Split('\n');

    lines.Length.ShouldBe(3);
    lines[0].ShouldStartWith("["); // Root at no indent
    lines[1].ShouldStartWith("  ["); // Child1 at 2 spaces
    lines[2].ShouldStartWith("  ["); // Child2 at 2 spaces
  }
}

/// <summary>
/// Tests for FlexNodePrinter.ToLayoutJson method.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexNodePrinterToLayoutJsonTests
{
  public void ShouldThrowWhenNodeIsNull()
  {
    Should.Throw<ArgumentNullException>(() => FlexNodePrinter.ToLayoutJson(null!));
  }

  public void ShouldReturnValidJson()
  {
    FlexNode node = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(50) };
    FlexLayoutEngine engine = new();
    engine.CalculateLayout(node, 100, 50);

    string result = FlexNodePrinter.ToLayoutJson(node);

    result.ShouldContain("\"left\"");
    result.ShouldContain("\"top\"");
    result.ShouldContain("\"width\"");
    result.ShouldContain("\"height\"");
  }

  public void ShouldUseCamelCasePropertyNames()
  {
    FlexNode node = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(50) };
    FlexLayoutEngine engine = new();
    engine.CalculateLayout(node, 100, 50);

    string result = FlexNodePrinter.ToLayoutJson(node);

    // Should be camelCase, not PascalCase
    result.ShouldContain("\"left\"");
    // Verify no PascalCase "Left" (case-sensitive check)
    result.ShouldNotContain("\"Left\"", Case.Sensitive);
  }

  public void ShouldIncludeChildrenArray()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    FlexLayoutEngine engine = new();
    engine.CalculateLayout(root, 200, 100);

    string result = FlexNodePrinter.ToLayoutJson(root);

    result.ShouldContain("\"children\"");
  }

  public void ShouldReturnCompactJsonWhenNotIndented()
  {
    FlexNode node = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(50) };
    FlexLayoutEngine engine = new();
    engine.CalculateLayout(node, 100, 50);

    string indented = FlexNodePrinter.ToLayoutJson(node, indented: true);
    string compact = FlexNodePrinter.ToLayoutJson(node, indented: false);

    // Compact should have no newlines within the JSON
    compact.ShouldNotContain("\n");
    // Indented should be longer due to formatting
    indented.Length.ShouldBeGreaterThan(compact.Length);
  }

  public void ShouldExcludeChildrenWhenNull()
  {
    FlexNode node = new() { Width = FlexValue.Point(100), Height = FlexValue.Point(50) };
    FlexLayoutEngine engine = new();
    engine.CalculateLayout(node, 100, 50);

    string result = FlexNodePrinter.ToLayoutJson(node);

    // Node has no children, so children should be null (excluded in JSON)
    result.ShouldContain("\"children\": null");
  }
}
