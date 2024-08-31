using System;
using ContentLib.Core.Utils;
//TODO here you go Xu <3 
namespace ContentLib.EnemyAPI.Model.Enemy.Custom;
/// <summary>
/// Factory responsible for creating custom EnemyType from IEnemyProperties instances, to then have their enemy 
/// </summary>
public class CustomEnemyFactory(IEnemyProperties properties) : IFactory<EnemyType>
{
    private IEnemyProperties Properties => Properties;
    
    
    public EnemyType Create()
    {
        throw new NotImplementedException();
    }
}