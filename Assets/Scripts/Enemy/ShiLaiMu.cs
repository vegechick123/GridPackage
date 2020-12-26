using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
    public class ShiLaiMu:Enemy
    {

    protected override void Awake()
    {
        base.Awake();

            Vector2Int origin = this.GetComponentInParent<EnemyBorn>().origin;
            Vector2Int des = this.GetComponentInParent<EnemyBorn>().destination;
            InitEnemy(origin, des, 0.4f, 10);
            Move();

    }

}

