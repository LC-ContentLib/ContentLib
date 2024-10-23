using ContentLib.API.Model.Event;
using ContentLib.Core.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;
using UnityEngine;

namespace ContentLib.EnemyAPI.Test;

public class TestListener
{
    bool triggeredOnce = false;
    public TestListener()
    {
        GameEventManager.Instance.Subscribe<MonsterCollideWithPlayerEvent>(GameEventType.MonsterPlayerCollisionEvent,
            OnMonsterColision);
    }
    
    private void OnMonsterColision(MonsterCollideWithPlayerEvent collideEvent)
    {
        if (triggeredOnce)
            return;
        IEnemy enemy = collideEvent.Enemy;
        if(enemy is IBracken bracken)
            Debug.Log($"Bracken with network id: {bracken.Id}");
    }
}