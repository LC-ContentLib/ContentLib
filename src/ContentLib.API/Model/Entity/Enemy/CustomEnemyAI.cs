using System;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.API.Model.Entity.Enemy;

/// <summary>
/// Implementation of the EnemyAI that 
/// </summary>
public class CustomEnemyAI : EnemyAI, IEnemy
{
    public ulong Id => this.NetworkObjectId;
    public bool IsAlive => this.isEnemyDead;
    public int Health => this.enemyHP;
    public Vector3 Position => this.transform.position;
    public IEnemyProperties EnemyProperties => new CustomEnemyAIProperties(this.enemyType);
    public bool IsHostile { get; }
    public bool IsChasing { get; }

    private class CustomEnemyAIProperties(EnemyType type) : IEnemyProperties
    {
        public string Name { get; set; }
        public bool IsCustom => false;
        public Type EnemyClassType => typeof(CustomEnemyAI);
        public bool SpawningDisabled { get; }
        public AnimationCurve ProbabilityCurve { get; }

        public GameObject EnemyPrefab
        {
            get => type.enemyPrefab;
            set => type.enemyPrefab = value;
        }

        public bool IsOutsideEnemy { get; set; }
        public bool IsDaytimeEnemy { get; set; }
        public bool SpawnFromWeeds { get; set; }

        public AnimationCurve SpawnWeightMultiplier
        {
            get => type.probabilityCurve;
            set => type.probabilityCurve = value;
        }

        public int MaxCount
        {
            get => type.MaxCount;
            set => type.MaxCount = value;
        }

        public float PowerLevel
        {
            get => type.PowerLevel;
            set => type.PowerLevel = value; 
        }

        public bool CanBeStunned
        {
            get => type.canBeStunned;
            set => type.canBeStunned = value;
        }

        public bool CanDie
        {
            get => type.canDie;
            set => type.canDie = value;
        }

        public bool DestroyOnDeath
        {
            get => type.destroyOnDeath;
            set => type.destroyOnDeath = value;
        }

        public float StunTimeMultiplier
        {
            get => type.stunTimeMultiplier;
            set => type.stunTimeMultiplier = value;
        }

        public float DoorSpeedMultiplier
        {
            get => type.doorSpeedMultiplier;
            set => type.doorSpeedMultiplier = value;
        }

        public float StunGameDifficultyMultiplier
        {
            get => type.stunGameDifficultyMultiplier;
            set => type.stunGameDifficultyMultiplier = value;
        }

        public bool CanSeeThroughFog
        {
            get => type.canSeeThroughFog;
            set => type.canSeeThroughFog = value;
        }

        public float PushPlayerForce
        {
            get => type.pushPlayerForce;
            set => type.pushPlayerForce = value;
        }

        public float PushPlayerDistance
        {
            get => type.pushPlayerDistance;
            set => type.pushPlayerDistance = value;
        }

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
            get => type.miscAnimations;
            set => type.miscAnimations = value;
        }
        public AudioClip[] AudioClips { 
            get => type.audioClips;
            set => type.audioClips = value;
        }
        public float TimeToPlayAudio {
            get => type.timeToPlayAudio;
            set => type.timeToPlayAudio = value;
            
        }
        public float LoudnessMultiplier { 
            get => type.loudnessMultiplier;
            set => type.loudnessMultiplier = value;
        }

        public AudioClip OverrideVentSFX
        {
            get => type.overrideVentSFX;
            set => type.overrideVentSFX = value;
        }
        public IEnemyHordeProperties? HordeProperties => null;
    }
    
}