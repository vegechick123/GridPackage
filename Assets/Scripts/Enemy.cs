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
        transform.localPosition = GetChessPosition3D(initPos);

        this.speed = speed;
        this.health = health;
    }
    /// <summary>
    /// Hack
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    Vector3 GetChessPosition3D(Vector2Int vector2)
    {
        Vector3 output = GridManager.instance.GetChessPosition3D(vector2);
        //hack
        return new Vector3(output.x,
            output.y + 1,
            output.z);
    }
    #region 移动
    /*-------------------------------移动-----------------------------------*/
    [ContextMenu("MoveTest")]
    private void MoveTest()
    {
        StartCoroutine(MoveActor(destination));
    }
    public void Move(Vector2Int target)
    {
        StartCoroutine(MoveActor(target));
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="target">移动的终点</param>
    IEnumerator MoveActor(Vector2Int target)
    {
        destination = target;
        while (true)
        {
            //获得可以走动的格子
            Vector2Int[] targets = GridManager.instance.GetEnemyPath(localtion);
            //选择正确的方向走动，移动一格
            Vector2Int next = GoDir(targets);
            Vector3 nextPos = GetChessPosition3D(next);
            Vector3 localtionPos = GetChessPosition3D(localtion);
            for (float time = 0; time <= 1; time += speed * Time.deltaTime)
            {
                transform.position = Vector3.Lerp(localtionPos, nextPos, time);
                yield return null;
            }
            //更新现在的location,迭代进入下一次移动
            localtion = next;
            if (localtion == destination)
                yield break;
        }
    }
    /// <summary>
    /// 获得下一次移动的格子坐标
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
    Vector2Int GoDir(Vector2Int[] targets)
    {
        foreach (Vector2Int vector2 in targets)
        {
            if (((vector2 - localtion) * (destination - localtion)).x > 0)
            {
                return vector2;
            }
            else if (((vector2 - localtion) * (destination - localtion)).y > 0)
            {
                return vector2;
            }
        }
        return localtion;
    }
    /*--------------------------------------------------------------------*/
    #endregion
}
