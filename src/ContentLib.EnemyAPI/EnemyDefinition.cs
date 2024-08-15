using System;
using System.Collections.Generic;
using ContentLib.Core;
using ContentLib.Core.Exceptions;
using UnityEngine;

namespace ContentLib.EnemyAPI;

/// <summary>
/// Base class for custom content that can be registered.
/// </summary>
public abstract class EnemyDefinition : ContentDefinition
{
    /// <summary>
    /// The Vanilla EnemyType ScriptableObject.
    /// </summary>
    [field: SerializeField] public EnemyType EnemyType { get; set; } = null!;

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
