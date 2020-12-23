using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
[ExecuteInEditMode]
public class GridManager : Manager<GridManager>
{
    [Tooltip("Grid的最大大小，Chess与Floor的位置不允许超过这个范围")]
    public Vector2Int size;
    [Tooltip("所管理的Grid")]
    public Grid grid;
    [Tooltip("被实例化的Chess的父物体")]
    public Transform chessContainer;
    [Tooltip("Chess在y轴上的偏移")]
    public float chessOffset;
    [Tooltip("Floor在y轴上的偏移")]
    public float floorOffset;
    //用二维数组存储Floor
    protected GFloor[,] floors;
    //用链表存储Chess
    protected List<GChess> chesses;
    protected override void Awake()
    {
        base.Awake();
        floors = new GFloor[size.x, size.y];
        chesses = new List<GChess>();
    }
    /// <summary>
    /// 判断location是否在size的范围内
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    private bool InRange(Vector2Int location)
    {
        if (0 <= location.x &&
            location.x < size.x &&
            0 <= location.y &&
            location.y < size.y)
            return true;
        else
            return false;
    }
    #region Floor与Chess的增删改查
    /// <summary>
    /// 使用GetTower和GetResources代替
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    [Obsolete]
    public GChess GetChess(Vector2Int location)
    {
        return chesses.Find(x => location == x.location);
    }
    public GChess[] GetChesses()
    {
        return chesses.ToArray();
    }
    public GTower GetTower(Vector2Int location)
    {
        return (GTower)chesses.Find(x => (location == x.location&&(x is GTower)));
    }
    public GResource GetResources(Vector2Int location)
    {
        return (GResource)chesses.Find(x => (location == x.location && (x is GResource)));
    }
    public GChess[] GetChessesInRange(Vector2Int[] range)
    {
        return chesses.FindAll(x=>range.Contains(x.location)).ToArray();
    }
    public void AddChess(GChess chess)
    {
        chesses.Add(chess);
    }
    public void RemoveChess(GChess chess)
    {
        chesses.Remove(chess);
    }
    /// <summary>
    /// 如果查询位置超出size大小则会数组越界
    ///如果该位置没有GFloor则返回null
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public GFloor GetFloor(Vector2Int location)
    {
        if (!InRange(location))
        {
            return null;
        }
        return floors[location.x, location.y];
    }
    public void AddFloor(GFloor floor)
    {
        if (floors[floor.location.x, floor.location.y] != null)
        {
            Debug.LogError("同一位置多个Floor");
        }
        floors[floor.location.x, floor.location.y] = floor;
    }
    public void RemoveFloor(GFloor floor)
    {
        if (floors[floor.location.x, floor.location.y] != floor)
        {
            Debug.LogError("移除错误的Floor");
        }
        floors[floor.location.x, floor.location.y] = null;
    }
    #endregion
    #region 2D与3D的映射
    /// <summary>
    /// 将对应的Chess的2D位置，映射到3D位置
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public Vector3 GetChessPosition3D(Vector2Int location)
    {
        return grid.GetCellCenterWorld(new Vector3Int(location.x, location.y, 0)) + chessOffset * grid.GetUpVector();
    }
    /// <summary>
    /// 将对应的Floor的2D位置，映射到3D位置
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public Vector3 GetFloorPosition3D(Vector2Int location)
    {
        return grid.GetCellCenterWorld(new Vector3Int(location.x, location.y, 0)) + floorOffset * grid.GetUpVector();
    }
    /// <summary>
    /// 将对应的将对应的2D方向映射到3D方向
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 GetDirection3D(Vector2Int direction)
    {
        Vector3 delta= grid.GetCellCenterWorld(new Vector3Int(direction.x, direction.y, 0))- grid.GetCellCenterWorld(new Vector3Int(0,0, 0));
        return delta.normalized;
    }
    #endregion
    #region 寻路相关
    /// <summary>
    /// 检查目标位置是否可以通行
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public bool CheckTransitability(Vector2Int location)
    {
        if (!InRange(location))
            return false;
        if(GetChess(location)||!GetFloor(location).transitable)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 获取寻路信息
    /// </summary>
    /// <param name="location"></param>
    /// <param name="movement"></param>
    /// <param name="teamID"></param>
    /// <returns></returns>
    public NavInfo GetNavInfo(Vector2Int location, int movement=999999, int teamID = -1)
    {
        Queue<ValueTuple<UnityEngine.Vector2Int, int, int>> queue = new Queue<ValueTuple<UnityEngine.Vector2Int, int, int>>();
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Queue<int> prev = new Queue<int>();
        Queue<bool> occupy = new Queue<bool>();
        Vector2Int[] dir = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };

        
        queue.Enqueue((location, movement, 0));
        res.Enqueue(location);
        prev.Enqueue(-1);
        occupy.Enqueue(true);

        HashSet<Vector2Int> vis = new HashSet<Vector2Int>();
        vis.Add(location);
        while (queue.Count != 0)
        {
            var node = queue.Dequeue();
            if (node.Item2 <= 0)
                continue;
            foreach (Vector2Int curDir in dir)
            {
                Vector2Int loc = node.Item1 + curDir;
                if (vis.Contains(loc))
                    continue;
                else
                    vis.Add(loc);

                GFloor floor = GetFloor(loc);
               // GChess chess = GetChess(loc);
                if (!floor || !CheckTransitability(loc))
                {
                    continue;
                }
                else
                {
                    queue.Enqueue((loc, node.Item2 - 1, res.Count));
                    res.Enqueue(loc);
                    prev.Enqueue(node.Item3);
                    occupy.Enqueue(false);
                    //if (!chess)
                    //{
                    //    occupy.Enqueue(false);
                    //}
                    //else
                    //{
                    //    occupy.Enqueue(true);
                    //}
                }
            }
        }
        return new NavInfo(res.ToArray(), prev.ToArray(), occupy.ToArray());
    }
    #endregion
    #region 范围查询
    /// <summary>
    /// 获得方格形式的圆内的位置坐标
    /// </summary>
    /// <param name="origin">原点</param>
    /// <param name="radius">半径</param>
    /// <returns></returns>
    public Vector2Int[] GetCircleRange(Vector2Int origin, int radius)
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Vector2Int[] dir = new Vector2Int[4];
        dir[0] = new Vector2Int(1, 1);
        dir[1] = new Vector2Int(1, -1);
        dir[2] = new Vector2Int(-1, 1);
        dir[3] = new Vector2Int(-1, -1);
        for (int x = 0; x <= radius; x++)
            for (int y = 0; x + y <= radius; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (x == 0 && y == 0 && i >= 1)
                    {
                        break;
                    }
                    if (x == 0 && i >= 2)
                    {
                        break;
                    }
                    if (y == 0 && (i == 1 || i == 3))
                    {
                        continue;
                    }

                    Vector2Int loc = new Vector2Int(x, y) * dir[i] + origin;
                    if (InRange(loc))
                        res.Enqueue(loc);
                }
            }
        return res.ToArray();
    }
    /// <summary>
    /// 获得上下左右四条射线范围距离内的坐标
    /// </summary>
    /// <param name="origin">原点</param>
    /// <param name="maxLength">射线最大距离</param>
    /// <param name="beginDistance"></param>
    /// <returns></returns>
    public Vector2Int[] GetFourRayRange(Vector2Int origin,int maxLength,int beginDistance=1)
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Vector2Int[] dir = new Vector2Int[4];
        dir[0] = new Vector2Int(1, 0);
        dir[1] = new Vector2Int(-1, 0);
        dir[2] = new Vector2Int(0, 1);
        dir[3] = new Vector2Int(0, -1);
        for (int i = 0; i < 4; i++)
        {
            Vector2Int[] temp = GetOneRayRange(origin, dir[i],maxLength,beginDistance);
            foreach (Vector2Int t in temp)
            {
                res.Enqueue(t);
            }
        }
        return res.ToArray();
    }
    /// <summary>
    /// 获得指定方向射线范围距离内的坐标
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="dir"></param>
    /// <param name="maxLength"></param>
    /// <param name="beginDistance"></param>
    /// <returns></returns>
    public Vector2Int[] GetOneRayRange(Vector2Int origin, Vector2Int dir, int maxLength, int beginDistance = 1)
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        for (int d = 1; d<=maxLength; d++)
        {
            Vector2Int nowpos = d * dir + origin;
            GChess t = GetChess(nowpos);
            if (!InRange(nowpos) ||t!=null)
            {
                if(t!=null&&d>=beginDistance)
                {
                    res.Enqueue(nowpos);
                }
                break;
            }
            else
                res.Enqueue(nowpos);
        }
        return res.ToArray();
    }
    /// <summary>
    /// 获得周围敌人可以走动的范围
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetEnemyPath(Vector2Int currentLocation)
    { 
        List<Vector2Int> output=new List<Vector2Int>();
        foreach(var location in GetCircleRange(currentLocation, 1))
        {
            if (GetFloor(location) != null)
            {
                if (GetFloor(location).floorType == GFloor.FloorType.the_enemy_path)
                {
                    output.Add(location);
                } 
            }
        }
        return output.ToArray();
    }
    #endregion 
    public GChess InstansiateChessAt(GameObject prefab, Vector2Int location)
    {
        var res = Instantiate(prefab, chessContainer);
        res.transform.position = GridManager.instance.GetChessPosition3D(location);
        GChess chess = res.GetComponent<GChess>();
        chess.location = location;
        return chess;
    }

}
