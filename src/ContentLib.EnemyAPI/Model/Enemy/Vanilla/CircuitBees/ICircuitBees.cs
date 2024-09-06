using System;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy.Vanilla.CircuitBees;

/// <summary>
/// Interface that represents the general functionality of the Circuit Bees enemy.
/// </summary>
public interface ICircuitBees : IEnemy
{
    bool IsRoaming { get; }
}