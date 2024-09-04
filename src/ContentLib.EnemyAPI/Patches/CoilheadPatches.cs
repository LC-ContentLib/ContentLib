using System;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Coilhead;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class CoilheadPatches
{
    //TODO find the best way to get the SpringManAI (God damnit zeekers)
    public static void Init()
    {
        
    }

    private class LocalCoilhead(SpringManAI springManAI) : ICoilhead
    {
        public ulong Id { get; }
        public bool IsAlive { get; }
        public int Health { get; }
        public Vector3 Position { get; }
        public IEnemyProperties EnemyProperties { get; }
        public bool IsSpawned { get; }
        public bool IsHostile { get; }
        public bool IsChasing { get; }
        public void OnCollideWithPlayer(Collider other) => throw new System.NotImplementedException();

        public void SetAnimationStopServerRpc() => throw new System.NotImplementedException();

        public void SetAnimationStopClientRpc() => throw new System.NotImplementedException();

        public void SetAnimationGoServerRpc() => throw new System.NotImplementedException();

        public void SetAnimationGoClientRpc() => throw new System.NotImplementedException();

        public void SearchForPlayers() => throw new System.NotImplementedException();

        public CoilheadBehaviourState State { get; }
    }

    private class CoilheadProperties(EnemyType type) : IEnemyProperties
    {
        public Type EnemyClassType => typeof(ICoilhead);

        public GameObject EnemyPrefab
        {
            get => type.enemyPrefab;
            set => type.enemyPrefab = value;
        }
        public AnimationCurve SpawnWeightMultiplier { get; set; }
        public int MaxCount { get; set; }
        public float PowerLevel { get; set; }
    }
}