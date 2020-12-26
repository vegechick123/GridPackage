using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 public class BigShiLaiMu:Enemy
 {
    protected override void Awake()
    {
        base.Awake();

            Vector2Int origin = this.GetComponentInParent<EnemyBorn>().origin;
            Vector2Int des = this.GetComponentInParent<EnemyBorn>().destination;
            InitEnemy(origin, des, 0.2f, 50);
            Move();

    }
    //protected override void OnDestroy()
    //{
    //    base.OnDestroy();
    //    // this.GetComponentInParent<EnemyBorn>().RefreshEnemyById(1, this.location);
    //    GameObject prefab = PrefabManager.instance.GetEnemyPrefabById(1);
    //    if (prefab != null)
    //    {
    //        Instantiate(prefab, this.transform.parent);
    //        Enemy enemy = prefab.GetComponent<Enemy>();
    //        Vector2Int origin = this.location;
    //        Vector2Int des = this.GetComponentInParent<EnemyBorn>().destination;
    //        enemy.ReadyToMove = false;
    //        enemy.InitEnemy(origin, des, 0.4f, 10);
    //        enemy.Move();
    //    }

    //}
    //IEnumerator Delay(float time,GameObject prefab)
    //{
    //    yield return new WaitForSeconds(time);
    //    Enemy enemy = prefab.GetComponent<Enemy>();
    //    enemy.Rebirth(this.location);
    //}
}

