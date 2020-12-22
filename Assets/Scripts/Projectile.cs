using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    RawMaterial,//原材料
    NormalBullet//普通炮弹
}
public class Projectile : MonoBehaviour
{
    public ProjectileType type;
    public IReceiveable target;
    protected virtual void Shoot(IReceiveable target)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            if (target != null)
            {
                target.Receive(this);
            }
        }
    }
    /// <summary>
    /// 炮弹经过拥有资源的发射塔时，会根据对应地的资源发生反应
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public ProjectileType Reaction(ResourceType resource)
    {
        switch (type)
        {
            case ProjectileType.RawMaterial:
                switch (resource)
                {
                    case ResourceType.Iron:
                        return ProjectileType.NormalBullet;
                    default:
                        return type;
                }
            case ProjectileType.NormalBullet:
                switch (resource)
                {
                    default:
                        return type;
                }
            default:return type;
        }
    }
}
