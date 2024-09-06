using System;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.CircuitBees;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class CircuitBeesPatches
{
    public static void Init()
    {
        On.RedLocustBees.Start += RedLocustBees_Start;
    }

    private static void RedLocustBees_Start(On.RedLocustBees.orig_Start orig, RedLocustBees self)
    {
        orig(self);
        IEnemy enemy = new LocalCircuitBees(self);
        EnemyManager.Instance().RegisterEnemy(enemy);

    }

    private class LocalCircuitBees(RedLocustBees beesAI) : ICircuitBees
    {
        public ulong Id => beesAI.NetworkObjectId;
        public bool IsAlive => !beesAI.isEnemyDead;
        //TODO might mean "health enemies" needs to be interfaced out? 
        public int Health => throw new NotImplementedException("Bees do not have health!");
        
        public Vector3 Position => beesAI.transform.position;
        public IEnemyProperties EnemyProperties => new LocalCircuitBeesEnemyProperties(beesAI.enemyType);
        public bool IsSpawned => beesAI.IsSpawned;
        public bool IsHostile => true;
        public bool IsChasing => beesAI.movingTowardsTargetPlayer;
        public bool IsRoaming => beesAI.searchForHive.inProgress;
    }

    private class LocalCircuitBeesEnemyProperties(EnemyType type) :IEnemyProperties
    {
        public string Name
        {
            get => type.enemyName;
            set => type.enemyName = value;
        }
        public bool IsCustom => false;
        public Type EnemyClassType => typeof(ICircuitBees);
        public bool SpawningDisabled => type.spawningDisabled;
        //TODO got probability curve and spawn weight mutliplier... oopsie
        public AnimationCurve ProbabilityCurve => type.probabilityCurve;

        public GameObject EnemyPrefab
        {
            get => type.enemyPrefab;
            set => type.enemyPrefab = value;
        }

        public bool IsOutsideEnemy => type.isOutsideEnemy;
        public bool IsDaytimeEnemy => type.isDaytimeEnemy;
        public bool SpawnFromWeeds => type.spawnFromWeeds;

        public AnimationCurve SpawnWeightMultiplier
        {
            get => type.probabilityCurve;
            set => type.probabilityCurve = value;
            
        }

        public int MaxCount => type.MaxCount;
        public float PowerLevel => type.PowerLevel;
        public bool CanBeStunned => type.canBeStunned;
        public bool CanDie => false;
        public bool DestroyOnDeath => type.destroyOnDeath;
        public float StunTimeMultiplier => type.stunTimeMultiplier;
        public float DoorSpeedMultiplier => type.doorSpeedMultiplier;
        public float StunGameDifficultyMultiplier => type.stunGameDifficultyMultiplier;
        public bool CanSeeThroughFog => type.canSeeThroughFog;
        public float PushPlayerForce => type.pushPlayerForce;
        public float PushPlayerDistance => type.pushPlayerDistance;

        public AudioClip HitBodySFX
        {
            get => type.hitBodySFX;
            set => type.hitBodySFX = value;
        }

        public AudioClip HitEnemyVoiceSFX
        {
            get => type.hitEnemyVoiceSFX;
            set => type.hitEnemyVoiceSFX = value;
        }

        public AudioClip DeathSFX
        {
            get => type.deathSFX;
            set => type.deathSFX = value;
        }

        public AudioClip StunSFX
        {
            get => type.stunSFX;
            set => type.stunSFX = value;
        }

        public MiscAnimation[] MiscAnimations
        {
            get=> type.miscAnimations;
            set => type.miscAnimations = value;
        }

        public AudioClip[] AudioClips
        {
            get => type.audioClips;
            set => type.audioClips = value;
        }

        public float TimeToPlayAudio
        {
            get => type.timeToPlayAudio;
            set => type.timeToPlayAudio = value;
        }

        public float LoudnessMultiplier
        {
            get => type.loudnessMultiplier;
            set => type.loudnessMultiplier = value;
        }

        public AudioClip OverrideVentSFX
        {
            get => type.overrideVentSFX;
            set => type.overrideVentSFX = value;
        }
    }
}