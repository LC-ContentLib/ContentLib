using ContentLib.API.Model.Entity.Enemy;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.EnemyAPI.Patches;

public class EnemyAIPatches
{
    public static void Init()
    {
        Debug.Log("Initialising EnemyAI Patches");
        On.EnemyAI.Start += EnemyAIOnStart;
        On.EnemyAI.KillEnemy += EnemyAIOnKillEnemy;
        On.StartOfRound.ShipLeave += StartOfRoundOnShipLeave;
    }

    private static void StartOfRoundOnShipLeave(On.StartOfRound.orig_ShipLeave orig, StartOfRound self) => EnemyManager.Instance.UnRegisterAllEnemies();

    private static void EnemyAIOnStart(On.EnemyAI.orig_Start orig, EnemyAI self)
    {
        orig(self);
        if(self is CustomEnemyAI customAI)
            EnemyManager.Instance.RegisterEnemy(customAI);
    }

    private static void EnemyAIOnKillEnemy(On.EnemyAI.orig_KillEnemy orig, EnemyAI self, bool destroy)
    {
        orig(self, destroy);
        EnemyManager.Instance.UnRegisterEnemy(self.NetworkObjectId);
        
    }
    

    
}