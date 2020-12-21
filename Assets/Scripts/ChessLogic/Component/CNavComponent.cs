using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CNavComponent : Component
{
    [HideInInspector]
    public NavInfo navInfo;
    [HideInInspector]
    public CMoveComponent moveComponent;

    protected override void Awake()
    {
        base.Awake();
        moveComponent = GetComponent<CMoveComponent>();
    }
    public void GenNavInfo()
    {
        navInfo = GridManager.instance.GetNavInfo(location);
    }
    /// <summary>
    /// 为不包括队友所占格子的移动范围
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetMoveRange()
    {
        GenNavInfo();
        return navInfo.range;
    }
    /// <summary>
    /// 包括队友所占格子的移动范围
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetMoveRangeWithoutOccupy()
    {
        GenNavInfo();
        return navInfo.GetRangeWithoutOccupy();
    }
    public int MoveToWtihNavInfo(Vector2Int destination)
    {
        Vector2Int[] path= navInfo.GetPath(destination);
        moveComponent.RequestMove(path);
        return path.Length;
    }
}
