using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ChessType:byte
{
    ememy,
    shooterTower,
    build,
    player
}
public class GChess : GActor
{
    public ChessType chessType;

    public UnityEvent eLocationChange = new UnityEvent();
    [HideInInspector]
    public CNavComponent navComponent;
    [HideInInspector]
    public CMoveComponent moveComponent;

    protected override void Awake()
    {
        base.Awake();

        navComponent = GetComponent<CNavComponent>();
        moveComponent = GetComponent<CMoveComponent>();
        GridManager.instance.AddChess(this);
    }
    
    protected virtual void OnDestroy()
    {
        if(GridManager.instance)
            GridManager.instance.RemoveChess(this);

    }
    #region 位移相关
    /// <summary>
    /// 不通过寻路，径直走向终点
    /// </summary>
    /// <param name="destination">终点</param>
    public void MoveToDirectly(Vector2Int destination)
    {
        Debug.Log("MoveToDirectly " + destination);
        moveComponent.RequestDirectMove(destination);
        location = destination;
        moveComponent.eFinishPath.AddListener(EnterLocation);
    }
    /// <summary>
    /// 移动到指定地面上
    /// </summary>
    /// <param name="floor"></param>
    public void MoveTo(GFloor floor)
    {
        MoveTo(floor.location);
    }
    /// <summary>
    /// 移动到指定位置
    /// </summary>
    /// <param name="destination"></param>
    public virtual void MoveTo(Vector2Int destination)
    {
        Debug.Log("MoveTo " + destination);
        navComponent.GenNavInfo();
        if (navComponent)
        {
            moveComponent.eFinishPath.AddListenerForOnce(EnterLocation);
            navComponent.MoveToWtihNavInfo(destination);
            location = destination;
        }
        else if (moveComponent)
        {
            Debug.LogError("Move To without navComponent");
        }
        else
        {
            Debug.LogError("Move To without moveComponent");
        }
    }
    /// <summary>
    /// 放弃当前移动
    /// </summary>
    public void AbortMove()
    {
        moveComponent.AbortMove();
    }
    /// <summary>
    /// 瞬间传送到指定地点
    /// </summary>
    /// <param name="destination"></param>
    public void Teleport(Vector2Int destination)
    {
        location = destination;
        transform.position = GridManager.instance.GetChessPosition3D(location);
        GridManager.instance.GetFloor(location).OnChessEnter(this);
    }
    /// <summary>
    /// 进如一个位置时的回调函数
    /// </summary>
    protected void EnterLocation()
    {
        GridManager.instance.GetFloor(location).OnChessEnter(this);
        eLocationChange.Invoke();
    }
    #endregion
    
    public void FaceToward(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir.normalized, GridManager.instance.grid.GetUpVector());
    }
    /// <summary>
    /// 朝向对应方向
    /// </summary>
    /// <param name="dir"></param>
    public void FaceToward(Vector2Int dir)
    {
        FaceToward(GridManager.instance.GetDirection3D(dir));
    }
}
