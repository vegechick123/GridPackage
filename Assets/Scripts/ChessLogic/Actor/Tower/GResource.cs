using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图上分布的资源类型
/// </summary>
public enum ResourceType
{
    None,
    RawMaterial,
    BuildingMaterial,
    Iron,
    RedFruit,
    GreenFruit,
    BlueFruit
}
public class GResource : GChess
{
    //资源类型
    public ResourceType type { get; protected set;}
    //是否可以被玩家直接采集
    public bool canGather { get; protected set; }
    //采集后出产的Projectile
    public ProjectileType projectile { get; protected set; }

    //建筑相关，在这个资源上建造建筑需要的资源类型及其花费//
    public int needMaterialCount;
    public ProjectileType needMaterialType;
}
