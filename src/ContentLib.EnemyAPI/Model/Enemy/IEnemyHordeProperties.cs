using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy;

public interface IEnemyHordeProperties
{

    /// <summary>
    /// The falloff curve that decreases spawn probability based on the number of this enemy spawned.
    /// </summary>
    AnimationCurve? NumberSpawnedFalloff { get; set; }

    /// <summary>
    /// Whether to use the falloff curve based on the number of enemies spawned.
    /// </summary>
    bool UseNumberSpawnedFalloff { get; set; }

    /// <summary>
    /// How many of these enemies spawn in groups.
    /// </summary>
    int SpawnInGroupsOf { get; set; }

    /// <summary>
    /// Whether the enemy requires nest objects to spawn.
    /// </summary>
    bool RequireNestObjectsToSpawn { get; set; }

    /// <summary>
    /// Normalized time of day when this enemy leaves.
    /// </summary>
    float NormalizedTimeInDayToLeave { get; set; }

    /// <summary>
    /// The navigation size limit for where the enemy can spawn and navigate.
    /// </summary>
    NavSizeLimit SizeLimit { get; set; }

    /// <summary>
    /// Properties related to nest spawn objects.
    /// </summary>
    GameObject NestSpawnPrefab { get; set; }
        
    /// <summary>
    /// Width of the nest prefab?
    /// </summary>
    float NestSpawnPrefabWidth { get; set; }
    
    //TODO unsure, ask later.
    /// <summary>
    /// 
    /// </summary>
    bool UseMinEnemyThresholdForNest { get; set; }
    
    //TODO needs to be confirmed as it could be something else. 
    /// <summary>
    /// Minimum enemies to spawn at a nest?
    /// </summary>
    int MinEnemiesToSpawnNest { get; set; }
}