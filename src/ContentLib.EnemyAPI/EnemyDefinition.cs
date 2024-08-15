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

        CheckEnemyTypeForErrors();
        CheckEnemyPrefabForErrors();

        if (s_lateForRegister)
            throw new ContentRegisteredTooLateException($"EnemyDefinition '{name}' was registered too late!");

        if (IsRegistered)
            throw new ContentAlreadyRegisteredException($"EnemyDefinition '{name}' has already been registered!");

        s_registeredEnemies.Add(this);
        IsRegistered = true;
    }

    private void CheckEnemyTypeForErrors() 
    {
        if (EnemyType == null)
            throw new NullReferenceException($"{nameof(EnemyType)} is null!");

        if (EnemyType.enemyPrefab == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)} is null!");

        ValidateLayerAndTag(EnemyType.enemyPrefab, "Enemies", "Enemy", nameof(EnemyType.enemyPrefab));
        
        if (String.IsNullOrEmpty(EnemyType.enemyName))
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyName)} is null or empty!");
    }

    private void CheckEnemyPrefabForErrors() // potentially doesn't always throw but important to atleast warn/error?
    {
        EnemyAI enemyAI = EnemyType.enemyPrefab.GetComponent<EnemyAI>();
        if (enemyAI == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAI)} is null!");

        if (enemyAI.enemyType == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAI)}{nameof(enemyAI.enemyType)} is null!"); // Needed for the enemy to reference important base values.
        
        if (enemyAI.agent == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAI)}{nameof(enemyAI.agent)} is null!"); // Needed for the enemy to move around the world.

        if (enemyAI.creatureAnimator == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAI)}{nameof(enemyAI.creatureAnimator)} is null!"); // Needed to animate the enemy.

        if (enemyAI.enemyBehaviourStates == null || enemyAI.enemyBehaviourStates.Length == 0)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAI)}{nameof(enemyAI.enemyBehaviourStates)} is null!"); // Needed so that the game knows what states you're switching when using SwitchBehaviour.

        ScanNodeProperties scanNodeProperties = EnemyType.enemyPrefab.GetComponentInChildren<ScanNodeProperties>();
        if (scanNodeProperties == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(scanNodeProperties)} is null!"); // Needed so enemies can be scanned.

        ValidateLayerAndTag(scanNodeProperties.gameObject, "ScanNode", "Untagged", $"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(scanNodeProperties)}"); // Needed so it can be scanned and not scanned when holding (needs tag to be untagged).

        Transform mapDot = EnemyType.enemyPrefab.transform.Find("MapDot");
        if (mapDot == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(EnemyType.enemyPrefab.transform)} MapDot is null!"); // Needed to display on the radars ingame.

        ValidateLayerAndTag(mapDot.gameObject, "MapRadar", "DoNotSet", $"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(EnemyType.enemyPrefab.transform)} MapDot"); // Needed if the MapDot does exist.
        
        EnemyAICollisionDetect enemyAICollisionDetect = EnemyType.enemyPrefab.GetComponentInChildren<EnemyAICollisionDetect>();
        if (enemyAICollisionDetect == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAICollisionDetect)} is null!"); // Needed for collision with enemies, players and doors.
        
        ValidateLayerAndTag(enemyAICollisionDetect.gameObject, "Enemies", "Enemy", $"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAICollisionDetect)}"); // Not 100% sure on if this is needed but likely is.

        if (enemyAICollisionDetect.gameObject.GetComponent<Rigidbody>() == null)
            throw new NullReferenceException($"{nameof(EnemyType)}{nameof(EnemyType.enemyPrefab)}{nameof(enemyAICollisionDetect)}{nameof(Rigidbody)} is null!"); // Needed for opening doors and stuff.
    
        // Considered adding a check for a box collider in the root gameobject/prefab, would be needed so that other enemies can see your enemy in the case of inheriting IVisibleThreat interface.
    }

    private void ValidateLayerAndTag(GameObject gameObject, string requiredLayer, string requiredTag, string errorMessagePrefix)
    {
        if (gameObject.layer != LayerMask.NameToLayer(requiredLayer) || gameObject.tag != requiredTag)
        {
            throw new NullReferenceException($"{errorMessagePrefix} is not set on the correct layer ('{requiredLayer}') or tag ('{requiredTag}')!");
        }
    }
}
