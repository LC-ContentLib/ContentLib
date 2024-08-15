using System.Collections.Generic;
using UnityEngine;

namespace ContentLib.Core;

/// <summary>
/// A list of properties for matching.
/// </summary>
public class LevelMatchingTags : MatchingTags
{
    /// <summary>
    /// Tags for matching level names.
    /// </summary>
    [field: SerializeField] public List<WeightedTag> LevelNameTags = [];

    /// <summary>
    /// Tags for matching kinds of levels.
    /// </summary>
    [field: SerializeField] public List<WeightedTag> LevelKindTags = [];

    /// <summary>
    /// Tags for matching weather names.
    /// </summary>
    [field: SerializeField] public List<WeightedTag> WeatherNameTags = [];

    /// <summary>
    /// RangeTags for matching level route price ranges.
    /// </summary>
    [field: SerializeField] public List<WeightedTag> RoutePriceRangeTags = [];

    /// <summary>
    /// Tags for matching dungeon names.
    /// </summary>
    [field: SerializeField] public List<WeightedTag> DungeonNameTags = [];

    /// <summary>
    /// Tags for matching kinds of dungeons.
    /// </summary>
    [field: SerializeField] public List<WeightedTag> DungeonKindTags = [];
}