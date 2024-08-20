using System;
using System.Collections.Generic;
using ContentLib.Core;
using ContentLib.Core.Exceptions;
using ContentLib.Core.Tags;
using UnityEngine;
using UnityEngine.AI;

namespace ContentLib.EnemyAPI;

/// <summary>
/// Base class for custom content that can be registered.
/// </summary>
[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "ContentLib/EnemyAPI/EnemyDefinition", order = 0)]
public class EnemyDefinition : ContentDefinition
{
    /// <inheritdoc cref="RegisterCallbacks{T}"/>
    public static RegisterCallbacks<EnemyDefinition> Callbacks { get; } = new(ref s_registerCallbackInvoker!);
    internal static List<EnemyDefinition> s_registeredEnemies = [];
    internal static bool s_lateForRegister = false;

    /// <inheritdoc cref="RegisterCallbacks{T}.CallbackInvoker"/>
    private static RegisterCallbacks<EnemyDefinition>.CallbackInvoker s_registerCallbackInvoker;

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

    /// <inheritdoc/>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="ContentRegisteredTooLateException"></exception>
    /// <exception cref="ContentAlreadyRegisteredException"></exception>
    public override void Register(ModDefinition modDefinition)
    {
        if (modDefinition == null)
            throw new ArgumentNullException(nameof(modDefinition));

        ModDefinition realMod = modDefinition.GetRealInstance();
        base.Register(realMod);

        s_registerCallbackInvoker.Invoke(realMod, name, isBefore: true, this);

        ValidateEnemyType();
        ValidateEnemyPrefab();

        if (s_lateForRegister)
            throw new ContentRegisteredTooLateException($"EnemyDefinition '{name}' was registered too late!");

        if (IsRegistered)
            throw new ContentAlreadyRegisteredException($"EnemyDefinition '{name}' has already been registered!");

        IsRegistered = true;
        s_registeredEnemies.Add(this);
        s_registerCallbackInvoker.Invoke(realMod, name, isBefore: false, this);
    }

    private void ValidateEnemyType() 
    {
        if (EnemyType == null)
            throw new NullReferenceException($"{nameof(EnemyType)} is null!");

        if (EnemyType.enemyPrefab == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)} is null!");

        ValidateLayerAndTag(EnemyType.enemyPrefab, "Enemies", "Enemy", nameof(EnemyType.enemyPrefab));
        
        if (string.IsNullOrEmpty(EnemyType.enemyName))
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyName)} is null or empty!");
    }

    private void ValidateEnemyPrefab()
    {
        var enemyPrefabPath = $"{nameof(EnemyType)}.{nameof(EnemyType.enemyPrefab)}";

        // The enemy spawning explodes without an EnemyAI component.
        EnemyAI enemyAI = EnemyType.enemyPrefab.GetComponent<EnemyAI>();
        if (enemyAI == null)
            throw new NullReferenceException($"{enemyPrefabPath}.{nameof(enemyAI)} is null!");

        // Needed for the enemy to reference important base values.
        if (enemyAI.enemyType == null)
            throw new NullReferenceException($"{enemyPrefabPath}.{nameof(enemyAI)}.{nameof(enemyAI.enemyType)} is null!");

        // Needed for the enemy to move around the world.
        if (enemyAI.agent == null)
            throw new NullReferenceException($"{enemyPrefabPath}.{nameof(enemyAI)}.{nameof(enemyAI.agent)} is null!");

        // Needed to animate the enemy.
        if (enemyAI.creatureAnimator == null)
            throw new NullReferenceException($"{enemyPrefabPath}.{nameof(enemyAI)}.{nameof(enemyAI.creatureAnimator)} is null!");

        // Needed so that the game knows what states you're switching when using SwitchBehaviour.
        // Not an issue if not using the game's Behaviour system, so should this be checked?
        // if (enemyAI.enemyBehaviourStates == null || enemyAI.enemyBehaviourStates.Length == 0)
        //     WarnBySeverity($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAI)}{nameof(enemyAI.enemyBehaviourStates)} is null!",
        //     (message) => throw new NullReferenceException(message));

        // Needed so enemies can be scanned.
        ScanNodeProperties[] scanNodeProperties = EnemyType.enemyPrefab.GetComponentsInChildren<ScanNodeProperties>();
        if (scanNodeProperties.Length == 0)
        {
            WarnBySeverity($"{nameof(EnemyType.enemyPrefab)} '{EnemyType.enemyPrefab.name}' doesn't have any {nameof(ScanNodeProperties)} components! It can't be scanned.",
                (message) => throw new NullReferenceException(message));
        }
        else
        {
            // Needed so it can be scanned and not scanned when holding (needs tag to be untagged).
            foreach (ScanNodeProperties scanNode in scanNodeProperties)
            {
                ValidateLayerAndTag(scanNode.gameObject, "ScanNode", "Untagged",
                    $"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(scanNodeProperties)}");
            }
        }

        // Need a better way for checking for a map dot than an object with a specific name.
        // Transform mapDot = EnemyType.enemyPrefab.transform.Find("MapDot");
        // if (mapDot == null)
        //     throw new NullReferenceException($"{nameof(EnemyType)} '{EnemyType.enemyPrefab.name}' doesn't have a 'MapDot' GameObject!"); // Needed to display on the radars in-game.

        // ValidateLayerAndTag(mapDot.gameObject, "MapRadar", "DoNotSet", $"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(EnemyType.enemyPrefab.transform)} MapDot"); // Needed if the MapDot does exist.
        
        // Needed for collision with enemies, players and doors.
        EnemyAICollisionDetect[] collisionDetectComponents = EnemyType.enemyPrefab.GetComponentsInChildren<EnemyAICollisionDetect>();
        if (collisionDetectComponents.Length == 0)
        {
            WarnBySeverity(
                $"Enemy '{EnemyType.enemyName}' doesn't reference any EnemyAI Collision Detect Scripts!",
                (message) => throw new NullReferenceException(message));
        }
        foreach (EnemyAICollisionDetect collisionDetect in collisionDetectComponents)
        {
            if (collisionDetect.mainScript == null)
            {
                WarnBySeverity(
                    $"An Enemy AI Collision Detect Script on GameObject '{collisionDetect.gameObject.name}' of enemy '{EnemyType.enemyName}' does not reference a 'Main Script', and could cause Null Reference Exceptions.",
                    (message) => throw new NullReferenceException(message));
            }
            ValidateLayerAndTag(collisionDetect.gameObject, "Enemies", "Enemy",
                $"Enemy '{EnemyType.enemyName}' has invalid layer or tag on a GameObject with an {nameof(EnemyAICollisionDetect)} Script!");

            // Needed for opening doors and stuff.
            if (collisionDetect.gameObject.GetComponent<Rigidbody>() == null)
            {
                WarnBySeverity(
                    $"An Enemy AI Collision Detect Script on GameObject '{collisionDetect.gameObject.name}' of enemy '{EnemyType.enemyName}' does not have a {nameof(Rigidbody)} Component attached, which prevents the enemy from opening doors!",
                    (message) => throw new MissingComponentException(message));
            }
        }

    
        // Considered adding a check for a box collider in the root GameObject/prefab, would be needed so that other enemies can see your enemy in the case of inheriting IVisibleThreat interface.
    }

    private void ValidateLayerAndTag(
        GameObject gameObject,
        string requiredLayer,
        string requiredTag,
        string errorMessagePrefix
    )
    {
        if (gameObject.layer != LayerMask.NameToLayer(requiredLayer) || gameObject.tag != requiredTag)
        {
            WarnBySeverity(
                $"{errorMessagePrefix} is not set on the correct layer ('{requiredLayer}') or tag ('{requiredTag}')!",
                (message) => throw new Exception(message));
        }
    }
}
