using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFloor : GActor
{
    public enum FloorType:byte
    {
        Default,
        the_enemy_path
    }

    public bool transitable=true;
    public FloorType floorType;

    [HideInInspector]
    public GFloor north, east, south, west;
    protected override void Awake()
    {
        base.Awake();
        RegistToManager();
        InitializesFloorInAllDir();
    }
    /// <summary>
    /// 初始化各个方向的地板信息
    /// </summary>
    void InitializesFloorInAllDir()
    {
        int x = location.x;
        int y = location.y;

        north = SetDir(x + 1, y);
        east = SetDir(x, y + 1);
        south = SetDir(x - 1, y);
        west = SetDir(x, y - 1);

        GFloor SetDir(int X,int Y)
        {
            return GridManager.instance.GetFloor(new Vector2Int(X, Y));
        }
    }
    void RegistToManager()
    {
        GridManager.instance.AddFloor(this);
    }
    public virtual void OnChessEnter(GChess chess)
    {
        Debug.Log("Chess Enter" + location);
    }
}
