using System;
using System.Collections.Generic;
using ContentLib.Core;
using ContentLib.Core.Exceptions;
using ContentLib.Core.Tags;
using UnityEngine;

namespace ContentLib.EnemyAPI;

/// <summary>
/// Base class for custom content that can be registered.
/// </summary>
[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "ContentLib/EnemyAPI/EnemyDefinition", order = 0)]
public class EnemyDefinition : ContentDefinition
{
    /// <summary>
    /// The Vanilla EnemyType ScriptableObject.
    /// </summary>
    [field: SerializeField] public EnemyType EnemyType { get; set; } = null!;

    /// <summary>
    /// Tags for matching and getting a weight for injecting this enemy to inside enemies spawn pool.
    /// </summary>
    /// <remarks>
    /// If no tags match, the enemy won't be injected into this spawn pool.
    /// </remarks>
    [field: SerializeField] public List<LevelMatchingTags> InsideLevelMatchingTags = [];

    /// <summary>
    /// Tags for matching and getting a weight for injecting this enemy to outside enemies spawn pool.
    /// </summary>
    /// <inheritdoc cref="InsideLevelMatchingTags"/>
    [field: SerializeField] public List<LevelMatchingTags> OutsideLevelMatchingTags = [];

    /// <summary>
    /// Tags for matching and getting a weight for injecting this enemy to daytime enemies spawn pool.
    /// </summary>
    /// <inheritdoc cref="InsideLevelMatchingTags"/>
    [field: SerializeField] public List<LevelMatchingTags> DaytimeLevelMatchingTags = [];

    internal static List<EnemyDefinition> s_registeredEnemies = [];
    internal static bool s_lateForRegister = false;

    /// <inheritdoc/>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="ContentRegisteredTooLateException"></exception>
    /// <exception cref="ContentAlreadyRegisteredException"></exception>
    public override void Register(ModDefinition mod)
    {
        base.Register(mod);

        if (EnemyType == null)
            throw new NullReferenceException($"{nameof(EnemyType)} is null!");

        if (EnemyType.enemyPrefab == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)} is null!");

        if (s_lateForRegister)
            throw new ContentRegisteredTooLateException($"EnemyDefinition '{name}' was registered too late!");

        if (IsRegistered)
            throw new ContentAlreadyRegisteredException($"EnemyDefinition '{name}' has already been registered!");

        s_registeredEnemies.Add(this);
        IsRegistered = true;
    }
}
