using System.Collections.Generic;
using UnityEngine;

namespace ContentLib.Core;

/// <summary>
/// A list of properties for matching.
/// </summary>
public class MatchingTags : ScriptableObject
{
    /// <summary>
    /// Tags for matching mod names.
    /// </summary>
    [field: SerializeField] public List<WeightedTag> ModNameTags = [];

    /// <summary>
    /// Tags for matching author names.
    /// </summary>
    [field: SerializeField] public List<WeightedTag> AuthorNameTags = [];
}