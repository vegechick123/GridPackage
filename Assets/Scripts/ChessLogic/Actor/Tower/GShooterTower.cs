using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 发射塔
/// </summary>
public class GShooterTower : GTower,IReceiveable
{
     //发射塔的设计方向
    public Vector2Int direction;
    //所占有的资源
    public ResourceType ownResourse;
    /// <summary>
    /// 朝着对应方向发射反应后的炮弹后销毁原炮弹
    /// </summary>
    /// <param name="projectile"></param>
    public void Receive(Projectile projectile)
    {
        Shoot(projectile.Reaction(ownResourse));
        Destroy(projectile.gameObject);
    }
    /// <summary>
    /// 朝着对应方向发射炮弹
    /// </summary>
    /// <param name="projectileType"></param>
    protected void Shoot(ProjectileType projectileType)
    {

    }
}
