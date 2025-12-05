namespace TimeWarp.Flexbox;

/// <summary>
/// Specifies how a dimension constraint should be interpreted during measurement.
/// </summary>
public enum MeasureMode
{
  /// <summary>
  /// No constraint - content can be any size.
  /// </summary>
  Undefined,

  /// <summary>
  /// Fixed size constraint - must be exactly this size.
  /// </summary>
  Exactly,

  /// <summary>
  /// Maximum size constraint - can be at most this size, but may be smaller.
  /// </summary>
  AtMost
}
