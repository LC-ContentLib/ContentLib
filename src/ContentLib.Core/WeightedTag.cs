using System;
using UnityEngine;

namespace ContentLib.Core;

/// <summary>
/// A string tag with a weight.
/// </summary>
/// <param name="text">The text of this tag.</param>
/// <param name="weight">The weight of this tag.</param>
[Serializable]
public class WeightedTag(string text, float weight)
{
    /// <summary>
    /// The text of this tag used for matching against tags.
    /// </summary>
    [field: SerializeField] public string Text { get; set; } = text;

    /// <summary>
    /// The weight of this tag when applied from being matched.
    /// </summary>
    [field: SerializeField] public float Weight { get; set; } = weight;
}