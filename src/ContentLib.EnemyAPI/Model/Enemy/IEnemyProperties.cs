using System;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy;
/// <summary>
/// Interface that represents the general properties of an IEnemy.
/// </summary>
public interface IEnemyProperties
{
    /// <summary>
    /// The type of the enemy instance these properties relate to.
    /// </summary>
    Type EnemyClassType { get; }
    /// <summary>
    /// The Unity Object that this Enemy relates to.
    /// </summary>
    GameObject EnemyPrefab { get; set; }
    
    /// <summary>
    /// The multiplier to determine spawn chance of the entity.
    /// </summary>
    AnimationCurve SpawnWeightMultiplier { get; set; }
    
    /// <summary>
    /// The maximum number of this enemy that can spawn on a moon.
    /// </summary>
    int MaxCount { get; set; }
    
    /// <summary>
    /// The power level of the enemy. 
    /// </summary>
    float PowerLevel { get; set; }
}