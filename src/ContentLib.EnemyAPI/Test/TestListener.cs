using ContentLib.Core.Model.Event;
using ContentLib.EnemyAPI.Model.Enemy;
using ContentLib.EnemyAPI.Model.Enemy.Vanilla.Bracken;
using UnityEngine;

namespace Lethal_Promotions.Model.Events.Listeners;

public class TestListener
{
    public TestListener()
    {
        GameEventManager.Instance.Subscribe<MonsterCollideWithPlayerEvent>(GameEventType.MonsterPlayerCollisionEvent,
            OnMonsterColision);
    }
    
    private void OnMonsterColision(MonsterCollideWithPlayerEvent collideEvent)
    {
        IEnemy enemy = collideEvent.Enemy;
        if(enemy is IBracken)
            Debug.Log("It worked and is works");
        if (EnemyManager.Instance().IsEnemyTypeOf(enemy,typeof(IBracken)))
        {
            Debug.Log("[LC-ContentLib] The player has been killed by a Braken!");
        }

        
    }
}