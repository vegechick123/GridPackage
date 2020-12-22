using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 当前的2Dint坐标，未到达前按为上次坐标
    /// </summary>
    public Vector2Int localtion;
    /*---------------敌人的基础属性面板-----------------*/
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed;
    public int health;
    /// <summary>
    /// 预设的目的地
    /// </summary>
    [SerializeField]
    private Vector2Int destination;
    /*---------------------------------------------*/
    private void Awake()
    {
        InitEnemy(new Vector2Int(2,2),1.0f,10);
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void InitEnemy(Vector2Int initPos, float speed,int health)
    {
        localtion = initPos;
        // todo: 坐标BUG
        transform.localPosition = GridManager.instance.GetChessPosition3D(initPos);
        //hack
        transform.localPosition = new Vector3(transform.localPosition.x,
            transform.localPosition.y+1,
            transform.localPosition.z);
        //
        this.speed = speed;
        this.health = health;
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="target">移动的终点</param>
    public void Move(Vector2Int target)
    {
        destination = target;
        //获得可以走动的格子
        GridManager.instance.GetEnemyPath(localtion);
        //选择正确的方向走动，移动一格，并且更新现在的location
        //迭代进入下一次移动
    }
}
