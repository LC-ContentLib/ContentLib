using ContentLib.Core.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;
using UnityEngine;

namespace Lethal_Promotions.Model.Events.Listeners;

public class TestListener 
{
    public TestListener()
    {
        GameEventManager.Instance.Subscribe<MonsterKillsPlayerEvent>(GameEventType.MonsterKillsPlayerEvent,
            OnMonsterKill);
        
        GameEventManager.Instance.Subscribe<MonsterCollideWithPlayerEvent>(GameEventType.MonsterPlayerCollisionEvent,OnMonsterCollision);
        Debug.Log("TestListener has been registered");
    }


    private void OnMonsterCollision(MonsterCollideWithPlayerEvent colideEvent) => Debug.Log("Collision Occured");

    private void OnMonsterKill(MonsterKillsPlayerEvent killsPlayerEvent)
    {
        IEnemy enemy = killsPlayerEvent.Enemy;
        if (enemy is IBracken bracken)
        {
            Debug.Log($"[$LC-ContentLib] The player has been killed by a Braken with id: {bracken.Id}");
        }
    }
}