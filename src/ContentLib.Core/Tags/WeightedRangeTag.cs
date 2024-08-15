using System;
using UnityEngine;

namespace ContentLib.Core.Tags;

/// <summary>
/// A range tag with a weight.
/// </summary>
/// <param name="range">The range of this tag.</param>
/// <param name="weight">The weight of this tag.</param>
[Serializable]
public class WeightedRangeTag(Vector2 range, float weight)
{
    [SerializeField] private Vector2 _minMaxRange = range;

    /// <summary>
    /// The minimum range of this tag, used for matching against ranges.
    /// </summary>
    public float Min { get => _minMaxRange.x; set => _minMaxRange.x = value; }

    /// <summary>
    /// The maximum range of this tag, used for matching against ranges.
    /// </summary>
    public float Max { get => _minMaxRange.y; set => _minMaxRange.y = value; }

    /// <summary>
    /// The weight of this tag when applied from being matched.
    /// </summary>
    [field: SerializeField] public float Weight { get; set; } = weight;
}