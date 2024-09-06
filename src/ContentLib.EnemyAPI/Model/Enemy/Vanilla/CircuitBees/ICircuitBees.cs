using System;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy.Vanilla.CircuitBees;

/// <summary>
/// Interface that represents the general functionality of the Circuit Bees enemy.
/// </summary>
public interface ICircuitBees : IEnemy
{
    /// <summary>
    /// Checkable state for if the bees are currently roaming (i.e. not at their nest due to any reason other than
    /// chasing a player). 
    /// </summary>
    bool IsRoaming { get; }
}