﻿using System.Collections;
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
public class GResource : GChess, IPickUpAble
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
    public int needMaterialCount = 5;
    public ProjectileType needMaterialType;
    [Space]
    public _Color _color;
    protected override void Awake()
    {
        base.Awake();
        if(type != ResourceType.Iron && _color != _Color.orgin)
        this.transform.GetChild(0).GetComponent<MeshRenderer>().materials[1].
            SetColor("_BaseColor",ColorMixing.instance.GetColor(_color));
    }
    public void BePickUp(Player player)
    {
        if (canGather)
        {
            player.PickUp(projectile,_color);
        }
        else
        {
            if (player.conveyObject)
            {
                player.PickUp(player.conveyObject.Reaction(type),_color);
            }
        }
    }
}
