using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理各种Projectile的prefab
/// </summary>
/// 
public class BulletManager : Manager<BulletManager>
{
    [Serializable]
    struct ProjectileInfo
    {
        public ProjectileType type;
        public GameObject prefab;
    }
    [SerializeField]
    private ProjectileInfo[] test;


}
