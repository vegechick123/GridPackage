using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[DisallowMultipleComponent]
public class EnemyBorn : MonoBehaviour
{
    //敌人起点
    public Vector2Int[] origin;
    //敌人终点
    public Vector2Int[] destination;
    [HideInInspector]
    public List<EnemyGroup> groups = new List<EnemyGroup>();
    [Serializable]
    public class EnemyGroup 
    {
        public int id;
        public int num;
        public float refreshTime;

    }
    public void Add()
    {
        groups.Add(new EnemyGroup());
    }
}

