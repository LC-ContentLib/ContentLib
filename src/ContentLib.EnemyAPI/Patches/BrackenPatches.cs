using System;
using ContentLib.Core.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;
/// <summary>
/// Patches responsible for the registration / unregistration and triggering of events to do with the Bracken.
/// </summary>
public class BrackenPatches
{
    public static void Init()
    {
        On.FlowermanAI.Start += FlowermanAI_Start;
        On.FlowermanAI.OnCollideWithPlayer += FlowerManAI_OnCollideWithPlayer;
    }
    private static void FlowermanAI_Start(On.FlowermanAI.orig_Start orig,FlowermanAI self)
    {
        orig(self);
        IEnemy vanillaBrackenEnemy = new LocalBracken(self);
        EnemyManager.Instance().RegisterEnemy(vanillaBrackenEnemy);
    }
    /// <summary>
    /// Patched method that will create either a MonsterCollideWithPlayerEvent or a MonsterKillPlayerEvent depending
    /// on if the colliding player is dead or not.
    /// </summary>
    /// <param name="orig"></param>
    /// <param name="self">The Bracken AI utilising the patched method.</param>
    /// <param name="other">The collider causing the method to be called.</param>
    private static void FlowerManAI_OnCollideWithPlayer(On.FlowermanAI.orig_OnCollideWithPlayer orig, FlowermanAI self, Collider other)
    {
        orig(self, other);
        IEnemy enemy = EnemyManager.Instance().GetEnemy(self.NetworkObjectId);
        if (self.targetPlayer.isPlayerDead)
        {
            GameEventManager.Instance.Trigger(new LocalBrackenMonsterKillPlayerEvent(enemy));
            return;
        }
        GameEventManager.Instance.Trigger(new LocalMonsterCollideWithPlayerEvent(enemy));
    }
    
    /// <summary>
    /// Local instance of the IBracken class, designed to be the sole thing in need of edit if a new version of LC is
    /// released.
    /// </summary>
    /// <param name="brackenAi">The vanilla game's BrackenAI instance.</param>
    private class LocalBracken(FlowermanAI brackenAi) : IBracken
    {
        //TODO General IEnemy methods could be moved to their own abstract initialisation perhaps?
        //TODO E.g: I dont imagine the way to get the NetworkObjectId is changing anytime soon?
        // IEnemy / IEntity METHODS
        public ulong Id => brackenAi.NetworkObjectId;
        public bool IsAlive => !brackenAi.isEnemyDead;
        public int Health => brackenAi.enemyHP;
        public Vector3 Position => brackenAi.transform.position;
        public IEnemyProperties EnemyProperties => new BrackenProperties(brackenAi);
        public bool IsSpawned => brackenAi.IsSpawned;
        public bool IsHostile => true;
        public bool IsChasing => brackenAi.movingTowardsTargetPlayer & brackenAi.isInAngerMode;
        
        //IBracken METHODS:
        //-----------------------------------------------------------------------------
        
        public void AddToAngerMeter(float amountToAdd) => brackenAi.AddToAngerMeter(amountToAdd);
        public float Anger => brackenAi.angerMeter;
        public bool IsAngry => brackenAi.isInAngerMode;
        public void EnterAngerModeServerRpc(float angerTime) => brackenAi.EnterAngerModeServerRpc(angerTime);

        public void EnterAngerModeClientRpc(float angerTime) => brackenAi.EnterAngerModeClientRpc(angerTime);

        public void AvoidClosestPlayer() => brackenAi.AvoidClosestPlayer();

        public void LookAtTrigger(int playerId) => brackenAi.LookAtFlowermanTrigger(playerId);

        public void ResetStealthTimerServerRpc(int playerObj) => brackenAi.ResetFlowermanStealthTimerServerRpc(playerObj);

        public void ResetStealthTimerClientRpc(int playerObj) => brackenAi.ResetFlowermanStealthClientRpc(playerObj);
    }

    /// <summary>
    /// Local instance of the IBracken's IEnemyProperties. Designed to be the sole thing in need of edit if a new
    /// version of LC is released. 
    /// </summary>
    /// <param name="flowermanAI">The vanilla version of the Bracken's AI.</param>
    private class BrackenProperties(FlowermanAI flowermanAI) : IEnemyProperties
    {
        private EnemyType _type = flowermanAI.enemyType;
        public string Name { get; set; }
        public bool IsCustom => false;
        public Type EnemyClassType => typeof(IBracken);
        public bool SpawningDisabled { get; }
        public AnimationCurve ProbabilityCurve { get; }

        public GameObject EnemyPrefab
        {
            get => _type.enemyPrefab;
            set => _type.enemyPrefab = value;
        }

        public bool IsOutsideEnemy { get;}
        public bool IsDaytimeEnemy { get;}
        public bool SpawnFromWeeds { get;}

        public AnimationCurve SpawnWeightMultiplier
        {
            get => _type.probabilityCurve;
            set => _type.probabilityCurve = value;
        }

        public int MaxCount
        {
            get => _type.MaxCount;
            set => _type.MaxCount = value;
        }

        public float PowerLevel
        {
            get => _type.PowerLevel;
            set => _type.PowerLevel = value; 
        }

        public bool CanBeStunned { get; set; }
        public bool CanDie { get; set; }
        public bool DestroyOnDeath { get; set; }
        public float StunTimeMultiplier { get; set; }
        public float DoorSpeedMultiplier { get; set; }
        public float StunGameDifficultyMultiplier { get; set; }
        public bool CanSeeThroughFog { get; set; }
        public float PushPlayerForce { get; set; }
        public float PushPlayerDistance { get; set; }
        public AudioClip HitBodySFX { get; set; }
        public AudioClip HitEnemyVoiceSFX { get; set; }
        public AudioClip DeathSFX { get; set; }
        public AudioClip StunSFX { get; set; }
        public MiscAnimation[] MiscAnimations { get; set; }
        public AudioClip[] AudioClips { get; set; }
        public float TimeToPlayAudio { get; set; }
        public float LoudnessMultiplier { get; set; }
        public AudioClip OverrideVentSFX { get; set; }
    }

    private class LocalMonsterCollideWithPlayerEvent(IEnemy enemy) : MonsterCollideWithPlayerEvent
    {
        public override IEnemy Enemy => enemy;
    }

    private class LocalBrackenMonsterKillPlayerEvent(IEnemy enemy) : MonsterKillsPlayerEvent
    {
        public override IEnemy Enemy => enemy;
    }
}

