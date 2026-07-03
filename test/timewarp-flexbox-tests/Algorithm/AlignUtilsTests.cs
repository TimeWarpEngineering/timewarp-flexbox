/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for AlignUtils utilities
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for AlignUtils utilities.
/// </summary>
public class AlignUtilsTests
{
  #region ResolveChildAlignment - AlignSelf Takes Precedence

  public void AlignSelfFlexStartShouldOverrideParentAlignItems()
  {
    // Arrange
    FlexNode parent = new() { Style = { AlignItems = Align.Center } };
    FlexNode child = new() { Style = { AlignSelf = Align.FlexStart } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert
    result.ShouldBe(Align.FlexStart);
  }

  public void AlignSelfCenterShouldOverrideParentAlignItems()
  {
    // Arrange
    FlexNode parent = new() { Style = { AlignItems = Align.FlexEnd } };
    FlexNode child = new() { Style = { AlignSelf = Align.Center } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert
    result.ShouldBe(Align.Center);
  }

  #endregion

  #region ResolveChildAlignment - AlignSelf Auto Uses Parent AlignItems

  public void AlignSelfAutoShouldUseParentAlignItems()
  {
    // Arrange
    FlexNode parent = new() { Style = { AlignItems = Align.Stretch } };
    FlexNode child = new() { Style = { AlignSelf = Align.Auto } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert
    result.ShouldBe(Align.Stretch);
  }

  public void AlignSelfAutoWithParentBaselineShouldUseBaseline()
  {
    // Arrange - Row direction (baseline is valid)
    FlexNode parent = new() { Style = { FlexDirection = FlexDirection.Row, AlignItems = Align.Baseline } };
    FlexNode child = new() { Style = { AlignSelf = Align.Auto } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert
    result.ShouldBe(Align.Baseline);
  }

  #endregion

  #region ResolveChildAlignment - Baseline in Column Direction Falls Back to FlexStart

  public void BaselineInColumnDirectionShouldFallBackToFlexStart()
  {
    // Arrange - Column direction with baseline alignment
    FlexNode parent = new() { Style = { FlexDirection = FlexDirection.Column, AlignItems = Align.Baseline } };
    FlexNode child = new() { Style = { AlignSelf = Align.Auto } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert - Baseline doesn't make sense in column, falls back to FlexStart
    result.ShouldBe(Align.FlexStart);
  }

  public void AlignSelfBaselineInColumnDirectionShouldFallBackToFlexStart()
  {
    // Arrange - Column direction with child explicitly using baseline
    FlexNode parent = new() { Style = { FlexDirection = FlexDirection.Column, AlignItems = Align.Center } };
    FlexNode child = new() { Style = { AlignSelf = Align.Baseline } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert
    result.ShouldBe(Align.FlexStart);
  }

  public void BaselineInColumnReverseShouldFallBackToFlexStart()
  {
    // Arrange - ColumnReverse is also a column direction
    FlexNode parent = new() { Style = { FlexDirection = FlexDirection.ColumnReverse, AlignItems = Align.Baseline } };
    FlexNode child = new() { Style = { AlignSelf = Align.Auto } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert
    result.ShouldBe(Align.FlexStart);
  }

  #endregion

  #region ResolveChildAlignment - Baseline Valid in Row Direction

  public void BaselineInRowDirectionShouldRemainBaseline()
  {
    // Arrange
    FlexNode parent = new() { Style = { FlexDirection = FlexDirection.Row, AlignItems = Align.Baseline } };
    FlexNode child = new() { Style = { AlignSelf = Align.Auto } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert
    result.ShouldBe(Align.Baseline);
  }

  public void BaselineInRowReverseDirectionShouldRemainBaseline()
  {
    // Arrange
    FlexNode parent = new() { Style = { FlexDirection = FlexDirection.RowReverse, AlignItems = Align.Baseline } };
    FlexNode child = new() { Style = { AlignSelf = Align.Auto } };

    // Act
    Align result = AlignUtils.ResolveChildAlignment(parent, child);

    // Assert
    result.ShouldBe(Align.Baseline);
  }

  #endregion

  #region FallbackAlignment (Align) - Distribution Values Fall Back

  public void SpaceBetweenAlignShouldFallBackToFlexStart()
  {
    // Act
    Align result = AlignUtils.FallbackAlignment(Align.SpaceBetween);

    // Assert
    result.ShouldBe(Align.FlexStart);
  }

  public void StretchAlignShouldFallBackToFlexStart()
  {
    // Act
    Align result = AlignUtils.FallbackAlignment(Align.Stretch);

    // Assert
    result.ShouldBe(Align.FlexStart);
  }

  public void SpaceAroundAlignShouldFallBackToFlexStart()
  {
    // Act
    Align result = AlignUtils.FallbackAlignment(Align.SpaceAround);

    // Assert
    result.ShouldBe(Align.FlexStart);
  }

  public void SpaceEvenlyAlignShouldFallBackToFlexStart()
  {
    // Act
    Align result = AlignUtils.FallbackAlignment(Align.SpaceEvenly);

    // Assert
    result.ShouldBe(Align.FlexStart);
  }

  #endregion

  #region FallbackAlignment (Align) - Non-Distribution Values Unchanged

  public void FlexStartAlignShouldRemainFlexStart()
  {
    // Act
    Align result = AlignUtils.FallbackAlignment(Align.FlexStart);

    // Assert
    result.ShouldBe(Align.FlexStart);
  }

  public void FlexEndAlignShouldRemainFlexEnd()
  {
    // Act
    Align result = AlignUtils.FallbackAlignment(Align.FlexEnd);

    // Assert
    result.ShouldBe(Align.FlexEnd);
  }

  public void CenterAlignShouldRemainCenter()
  {
    // Act
    Align result = AlignUtils.FallbackAlignment(Align.Center);

    // Assert
    result.ShouldBe(Align.Center);
  }

  public void BaselineAlignShouldRemainBaseline()
  {
    // Act
    Align result = AlignUtils.FallbackAlignment(Align.Baseline);

    // Assert
    result.ShouldBe(Align.Baseline);
  }

  #endregion

  #region FallbackAlignment (Justify) - Distribution Values Fall Back

  public void SpaceBetweenJustifyShouldFallBackToFlexStart()
  {
    // Act
    Justify result = AlignUtils.FallbackAlignment(Justify.SpaceBetween);

    // Assert
    result.ShouldBe(Justify.FlexStart);
  }

  public void SpaceAroundJustifyShouldFallBackToFlexStart()
  {
    // Act
    Justify result = AlignUtils.FallbackAlignment(Justify.SpaceAround);

    // Assert
    result.ShouldBe(Justify.FlexStart);
  }

  public void SpaceEvenlyJustifyShouldFallBackToFlexStart()
  {
    // Act
    Justify result = AlignUtils.FallbackAlignment(Justify.SpaceEvenly);

    // Assert
    result.ShouldBe(Justify.FlexStart);
  }

  #endregion

  #region FallbackAlignment (Justify) - Non-Distribution Values Unchanged

  public void FlexStartJustifyShouldRemainFlexStart()
  {
    // Act
    Justify result = AlignUtils.FallbackAlignment(Justify.FlexStart);

    // Assert
    result.ShouldBe(Justify.FlexStart);
  }

  public void FlexEndJustifyShouldRemainFlexEnd()
  {
    // Act
    Justify result = AlignUtils.FallbackAlignment(Justify.FlexEnd);

    // Assert
    result.ShouldBe(Justify.FlexEnd);
  }

  public void CenterJustifyShouldRemainCenter()
  {
    // Act
    Justify result = AlignUtils.FallbackAlignment(Justify.Center);

    // Assert
    result.ShouldBe(Justify.Center);
  }

  #endregion
}
