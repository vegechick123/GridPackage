using System;
using System.Collections.Generic;
using UnityEngine;


public class SmallShiLaiMu : Enemy
{

    protected override void Awake()
    {
        base.Awake();

        Vector2Int origin = this.GetComponentInParent<EnemyBorn>().origin;
        Vector2Int des = this.GetComponentInParent<EnemyBorn>().destination;
        InitEnemy(origin, des, 0.7f, 6);
        Move();

    }
}

