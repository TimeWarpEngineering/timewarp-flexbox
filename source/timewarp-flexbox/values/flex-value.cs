namespace TimeWarp.Flexbox;

/// <summary>
/// Represents a CSS-style value with a numeric component and unit type.
/// This is an immutable value type optimized for performance.
/// </summary>
public readonly struct FlexValue : IEquatable<FlexValue>
{
  /// <summary>
  /// Gets the numeric value component.
  /// </summary>
  public float Value { get; }

  /// <summary>
  /// Gets the unit type for this value.
  /// </summary>
  public Unit Unit { get; }

  /// <summary>
  /// A value representing an undefined/unset state.
  /// </summary>
  public static readonly FlexValue Undefined = new(float.NaN, Unit.Undefined);

  /// <summary>
  /// A value representing automatic sizing.
  /// </summary>
  public static readonly FlexValue Auto = new(float.NaN, Unit.Auto);

  /// <summary>
  /// A value representing max-content intrinsic sizing.
  /// </summary>
  public static readonly FlexValue MaxContent = new(float.NaN, Unit.MaxContent);

  /// <summary>
  /// A value representing stretch intrinsic sizing.
  /// </summary>
  public static readonly FlexValue Stretch = new(float.NaN, Unit.Stretch);

  private FlexValue(float value, Unit unit)
  {
    Value = value;
    Unit = unit;
  }

  /// <summary>
  /// Creates a FlexValue with an absolute point measurement.
  /// </summary>
  /// <param name="value">The point value.</param>
  /// <returns>A FlexValue with Unit.Point.</returns>
  public static FlexValue Point(float value) => new(value, Unit.Point);

  /// <summary>
  /// Creates a FlexValue with a percentage measurement.
  /// </summary>
  /// <param name="value">The percentage value (0-100).</param>
  /// <returns>A FlexValue with Unit.Percent.</returns>
  public static FlexValue Percent(float value) => new(value, Unit.Percent);

  /// <summary>
  /// Creates a FlexValue with fit-content sizing and a maximum value.
  /// </summary>
  /// <param name="value">The maximum value to clamp fit-content to.</param>
  /// <returns>A FlexValue with Unit.FitContent.</returns>
  public static FlexValue FitContent(float value) => new(value, Unit.FitContent);

  /// <summary>
  /// Determines whether this value represents an undefined state.
  /// </summary>
  public bool IsUndefined => Unit == Unit.Undefined;

  /// <summary>
  /// Determines whether this value represents automatic sizing.
  /// </summary>
  public bool IsAuto => Unit == Unit.Auto;

  /// <summary>
  /// Determines whether this value represents max-content intrinsic sizing.
  /// </summary>
  public bool IsMaxContent => Unit == Unit.MaxContent;

  /// <summary>
  /// Determines whether this value represents fit-content intrinsic sizing.
  /// </summary>
  public bool IsFitContent => Unit == Unit.FitContent;

  /// <summary>
  /// Determines whether this value represents stretch intrinsic sizing.
  /// </summary>
  public bool IsStretch => Unit == Unit.Stretch;

  /// <summary>
  /// Determines whether this value has a defined numeric value (Point, Percent, or FitContent).
  /// </summary>
  public bool IsDefined => Unit == Unit.Point || Unit == Unit.Percent || Unit == Unit.FitContent;

  public bool Equals(FlexValue other)
  {
    if (Unit != other.Unit)
      return false;

    // For Undefined, Auto, MaxContent, and Stretch, we don't compare the float values
    if (Unit == Unit.Undefined || Unit == Unit.Auto || Unit == Unit.MaxContent || Unit == Unit.Stretch)
      return true;

    // For Point, Percent, and FitContent, compare the actual values
    return Value.Equals(other.Value);
  }

  public override bool Equals(object? obj) => obj is FlexValue other && Equals(other);

  public override int GetHashCode()
  {
    // For Undefined, Auto, MaxContent, and Stretch, only hash the unit
    if (Unit == Unit.Undefined || Unit == Unit.Auto || Unit == Unit.MaxContent || Unit == Unit.Stretch)
      return Unit.GetHashCode();

    return HashCode.Combine(Value, Unit);
  }

  public static bool operator ==(FlexValue left, FlexValue right) => left.Equals(right);

  public static bool operator !=(FlexValue left, FlexValue right) => !left.Equals(right);

  public override string ToString()
  {
    return Unit switch
    {
      Unit.Undefined => "undefined",
      Unit.Auto => "auto",
      Unit.Point => $"{Value}pt",
      Unit.Percent => $"{Value}%",
      Unit.MaxContent => "max-content",
      Unit.FitContent => $"fit-content({Value})",
      Unit.Stretch => "stretch",
      _ => $"{Value} ({Unit})"
    };
  }
}
