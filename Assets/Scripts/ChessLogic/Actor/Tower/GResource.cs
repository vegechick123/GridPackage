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
    [SerializeField]
    public ResourceType type;
    [Header("采集相关")]
    //是否可以被玩家直接采集
    [SerializeField]
    public bool canGather;
    //采集后出产的Projectile
    [SerializeField]
    public ProjectileType projectile;
    [Header("建筑相关")]
    //建筑相关，在这个资源上建造建筑需要的资源类型及其花费//
    public int needMaterialCount=5;
    public ProjectileType needMaterialType;
}
