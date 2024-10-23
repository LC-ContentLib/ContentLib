using System;
using ContentLib.API.Model.Mods;
using UnityEngine;

namespace ContentLib.EnemyAPI.Model.Enemy
{
    /// <summary>
    /// Interface that represents the general properties of an IEnemy.
    /// </summary>
    public interface IEnemyProperties
    {
        /// <summary>
        /// Name of the entity.
        /// </summary>
        string Name { get; set; }
        
        bool IsCustom { get;}
        
        /// <summary>
        /// The type of the enemy instance these properties relate to.
        /// </summary>
        Type EnemyClassType { get; }
        
        bool SpawningDisabled { get; }
        
        AnimationCurve ProbabilityCurve { get; } 
        
        /// <summary>
        /// The Unity Object that this Enemy relates to.
        /// </summary>
        GameObject EnemyPrefab { get; set; }
        
        /// <summary>
        /// Whether this enemy is considered an outside enemy.
        /// </summary>
        bool IsOutsideEnemy { get;}

        /// <summary>
        /// Whether this enemy is active during the day.
        /// </summary>
        bool IsDaytimeEnemy { get;}

        /// <summary>
        /// Whether this enemy's spawn probability is affected by the presence of weeds or mold.
        /// </summary>
        bool SpawnFromWeeds { get; }

        /// <summary>
        /// The multiplier to determine spawn chance of the entity.
        /// </summary>
        AnimationCurve SpawnWeightMultiplier { get; set; }
        
        /// <summary>
        /// The maximum number of this enemy that can spawn on a moon.
        /// </summary>
        int MaxCount { get; }
        
        /// <summary>
        /// The power level of the enemy. 
        /// </summary>
        float PowerLevel { get;}
        
        /// <summary>
        /// Whether the enemy can be stunned.
        /// </summary>
        bool CanBeStunned { get;}
        
        /// <summary>
        /// Whether the enemy can be killed.
        /// </summary>
        bool CanDie { get;}
        
        /// <summary>
        /// Whether the enemy should be destroyed when it dies.
        /// </summary>
        bool DestroyOnDeath { get;}
        
        /// <summary>
        /// Multiplier for how long the enemy stays stunned.
        /// </summary>
        float StunTimeMultiplier { get;}
        
        /// <summary>
        /// Multiplier for the door interaction speed.
        /// </summary>
        float DoorSpeedMultiplier { get;}
        
        /// <summary>
        /// The overall game difficulty multiplier affected by the stun mechanics.
        /// </summary>
        float StunGameDifficultyMultiplier { get;}
        
        /// <summary>
        /// Whether the enemy can see through fog.
        /// </summary>
        bool CanSeeThroughFog { get;}
        
        /// <summary>
        /// Force with which the enemy pushes the player.
        /// </summary>
        float PushPlayerForce { get;}
        
        /// <summary>
        /// Distance at which the enemy pushes the player.
        /// </summary>
        float PushPlayerDistance { get;}
        /// <summary>
        /// Audio for the sound of an item makes when hitting the enemy's body.
        /// </summary>
        AudioClip HitBodySFX { get; set; }
        
        /// <summary>
        /// Audio for the vocalisation the enemy makes when hit.
        /// </summary>
        AudioClip HitEnemyVoiceSFX { get; set; }
        
        /// <summary>
        /// The sound the enemy makes when it dies.
        /// </summary>
        AudioClip DeathSFX { get; set; }
        
        /// <summary>
        /// The sound the enemy makes when stunned. 
        /// </summary>
        AudioClip StunSFX { get; set; }
        //TODO getting exampls would be good
        /// <summary>
        /// Miscellaneous animations for this enemy.
        /// </summary>
        MiscAnimation[] MiscAnimations { get; set; }

        /// <summary>
        /// General audio clips for this enemy.
        /// </summary>
        AudioClip[] AudioClips { get; set; }
        
        /// <summary>
        /// The audio properties of the enemy when in a vent.
        /// </summary>
        float TimeToPlayAudio { get; set; }
        
        //TODO needs verificaiton
        /// <summary>
        /// Loudness of a vent when the vent noise is made?
        /// </summary>
        float LoudnessMultiplier { get; set; }
        
        /// <summary>
        /// Overide the Vent Noise made by a spawning entity.
        /// </summary>
        AudioClip OverrideVentSFX { get; set; }
        IEnemyHordeProperties? HordeProperties { get; }
    }
}
