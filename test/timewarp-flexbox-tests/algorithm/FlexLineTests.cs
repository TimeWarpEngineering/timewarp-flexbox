/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for FlexLine and FlexLineRunningLayout
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

/// <summary>
/// Tests for FlexLine and FlexLineRunningLayout.
/// </summary>
public class FlexLineTests
{
  #region FlexLineRunningLayout - Equality

  public void FlexLineRunningLayoutShouldBeEqualWhenAllValuesMatch()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new()
    {
      TotalFlexGrowFactors = 2.0f,
      TotalFlexShrinkScaledFactors = 1.5f,
      RemainingFreeSpace = 100f,
      MainDim = 200f,
      CrossDim = 50f
    };

    FlexLineRunningLayout layout2 = new()
    {
      TotalFlexGrowFactors = 2.0f,
      TotalFlexShrinkScaledFactors = 1.5f,
      RemainingFreeSpace = 100f,
      MainDim = 200f,
      CrossDim = 50f
    };

    // Act & Assert
    (layout1 == layout2).ShouldBeTrue();
    layout1.Equals(layout2).ShouldBeTrue();
  }

  public void FlexLineRunningLayoutShouldNotBeEqualWhenTotalFlexGrowFactorsDiffers()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new() { TotalFlexGrowFactors = 1.0f };
    FlexLineRunningLayout layout2 = new() { TotalFlexGrowFactors = 2.0f };

    // Act & Assert
    (layout1 != layout2).ShouldBeTrue();
  }

  public void FlexLineRunningLayoutShouldNotBeEqualWhenTotalFlexShrinkScaledFactorsDiffers()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new() { TotalFlexShrinkScaledFactors = 1.0f };
    FlexLineRunningLayout layout2 = new() { TotalFlexShrinkScaledFactors = 2.0f };

    // Act & Assert
    (layout1 != layout2).ShouldBeTrue();
  }

  public void FlexLineRunningLayoutShouldNotBeEqualWhenRemainingFreeSpaceDiffers()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new() { RemainingFreeSpace = 50f };
    FlexLineRunningLayout layout2 = new() { RemainingFreeSpace = 100f };

    // Act & Assert
    (layout1 != layout2).ShouldBeTrue();
  }

  public void FlexLineRunningLayoutShouldNotBeEqualWhenMainDimDiffers()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new() { MainDim = 100f };
    FlexLineRunningLayout layout2 = new() { MainDim = 200f };

    // Act & Assert
    (layout1 != layout2).ShouldBeTrue();
  }

  public void FlexLineRunningLayoutShouldNotBeEqualWhenCrossDimDiffers()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new() { CrossDim = 30f };
    FlexLineRunningLayout layout2 = new() { CrossDim = 60f };

    // Act & Assert
    (layout1 != layout2).ShouldBeTrue();
  }

  #endregion

  #region FlexLineRunningLayout - GetHashCode

  public void FlexLineRunningLayoutShouldHaveSameHashCodeWhenEqual()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new()
    {
      TotalFlexGrowFactors = 2.0f,
      TotalFlexShrinkScaledFactors = 1.5f,
      RemainingFreeSpace = 100f,
      MainDim = 200f,
      CrossDim = 50f
    };

    FlexLineRunningLayout layout2 = new()
    {
      TotalFlexGrowFactors = 2.0f,
      TotalFlexShrinkScaledFactors = 1.5f,
      RemainingFreeSpace = 100f,
      MainDim = 200f,
      CrossDim = 50f
    };

    // Act & Assert
    layout1.GetHashCode().ShouldBe(layout2.GetHashCode());
  }

  public void FlexLineRunningLayoutShouldHaveDifferentHashCodeWhenDifferent()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new() { TotalFlexGrowFactors = 1.0f };
    FlexLineRunningLayout layout2 = new() { TotalFlexGrowFactors = 2.0f };

    // Act & Assert - Hash codes may collide but usually differ
    // This test documents the behavior, not a strict requirement
    (layout1.GetHashCode() != layout2.GetHashCode()).ShouldBeTrue();
  }

  #endregion

  #region FlexLineRunningLayout - Default Values

  public void FlexLineRunningLayoutShouldHaveZeroDefaultValues()
  {
    // Arrange & Act
    FlexLineRunningLayout layout = new();

    // Assert
    layout.TotalFlexGrowFactors.ShouldBe(0f);
    layout.TotalFlexShrinkScaledFactors.ShouldBe(0f);
    layout.RemainingFreeSpace.ShouldBe(0f);
    layout.MainDim.ShouldBe(0f);
    layout.CrossDim.ShouldBe(0f);
  }

  #endregion

  #region FlexLine - Default State

  public void FlexLineShouldHaveEmptyItemsInFlowByDefault()
  {
    // Arrange & Act
    FlexLine flexLine = new();

    // Assert
    flexLine.ItemsInFlow.Count.ShouldBe(0);
  }

  public void FlexLineShouldHaveZeroSizeConsumedByDefault()
  {
    // Arrange & Act
    FlexLine flexLine = new();

    // Assert
    flexLine.SizeConsumed.ShouldBe(0f);
  }

  public void FlexLineShouldHaveZeroAutoMarginsByDefault()
  {
    // Arrange & Act
    FlexLine flexLine = new();

    // Assert
    flexLine.NumberOfAutoMargins.ShouldBe(0);
  }

  public void FlexLineShouldHaveDefaultRunningLayout()
  {
    // Arrange & Act
    FlexLine flexLine = new();

    // Assert
    flexLine.Layout.TotalFlexGrowFactors.ShouldBe(0f);
    flexLine.Layout.TotalFlexShrinkScaledFactors.ShouldBe(0f);
    flexLine.Layout.RemainingFreeSpace.ShouldBe(0f);
    flexLine.Layout.MainDim.ShouldBe(0f);
    flexLine.Layout.CrossDim.ShouldBe(0f);
  }

  #endregion

  #region FlexLine - Layout Can Be Modified

  public void FlexLineLayoutCanBeModified()
  {
    // Arrange
    FlexLine flexLine = new();

    // Act
    flexLine.Layout = new FlexLineRunningLayout
    {
      TotalFlexGrowFactors = 3.0f,
      RemainingFreeSpace = 50f,
      MainDim = 150f,
      CrossDim = 75f
    };

    // Assert
    flexLine.Layout.TotalFlexGrowFactors.ShouldBe(3.0f);
    flexLine.Layout.RemainingFreeSpace.ShouldBe(50f);
    flexLine.Layout.MainDim.ShouldBe(150f);
    flexLine.Layout.CrossDim.ShouldBe(75f);
  }

  #endregion

  #region FlexLineRunningLayout - Object Equals

  public void FlexLineRunningLayoutObjectEqualsShouldReturnTrueForEqualLayouts()
  {
    // Arrange
    FlexLineRunningLayout layout1 = new() { MainDim = 100f };
    object layout2 = new FlexLineRunningLayout { MainDim = 100f };

    // Act & Assert
    layout1.Equals(layout2).ShouldBeTrue();
  }

  public void FlexLineRunningLayoutObjectEqualsShouldReturnFalseForNull()
  {
    // Arrange
    FlexLineRunningLayout layout = new() { MainDim = 100f };

    // Act & Assert
    layout.Equals(null).ShouldBeFalse();
  }

  public void FlexLineRunningLayoutObjectEqualsShouldReturnFalseForDifferentType()
  {
    // Arrange
    FlexLineRunningLayout layout = new() { MainDim = 100f };
    object other = "not a layout";

    // Act & Assert
    layout.Equals(other).ShouldBeFalse();
  }

  #endregion
}
