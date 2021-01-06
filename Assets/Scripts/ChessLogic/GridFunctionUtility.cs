using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public enum Direction
{
    up, down, left, right
}
static class GridFunctionUtility
{/// <summary>
/// 判断location是否在range范围内
/// </summary>
/// <param name="range"></param>
/// <param name="location"></param>
/// <returns></returns>
    public static bool InRange(this Vector2Int[] range, Vector2Int location)
    {
        foreach (var p in range)
        {
            if (p == location)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="range"></param>
    /// <param name="location"></param>
    /// <param name="bias">range整体偏移量</param>
    /// <returns></returns>
    public static bool InRange(this Vector2Int[] range, Vector2Int location, Vector2Int bias)
    {
        location -= bias;
        foreach (var p in range)
        {
            if (p == location)
                return true;
        }
        return false;
    }
    public static Vector2Int Normalized(this Vector2Int dir)
    {
        if (dir == Vector2Int.zero)
        {
            Debug.LogError("传入Zero Vector");
            return dir;
        }
        int x = 0, y = 0;
        if (dir.x == 0)
        {
            if (dir.y > 0)
                y = 1;
            else
                y = -1;
        }
        else if (dir.y == 0)
        {
            if (dir.x > 0)
                x = 1;
            else
                x = -1;
        }
        else
        {
            Debug.LogError("传入不可标准化的Vector" + dir);
            return Vector2Int.zero;
        }
        return new Vector2Int(x, y);
    }
    public static int ManhattonDistance(this Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    public static Vector3 Divide(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
    /// <summary>
    /// 添加只会被调用一次的函数    
    /// </summary>
    /// <param name="a"></param>
    /// <param name="t"></param>
    public static void AddListenerForOnce(this UnityEvent a, UnityAction t)
    {
        UnityAction p = null;
        p = () =>
        {
            t();
            a.RemoveListener(p);
        };
        a.AddListener(p);
    }
    public static void DestoryAll(this GameObject[] gameObjects)
    {
        foreach (var t in gameObjects)
        {
            GameObject.Destroy(t);
        }
    }
    public static void InvokeAfter(this MonoBehaviour target,Action task, float time)
    {
        target.StartCoroutine(InvokeAfter(task, time));
    }
    public static IEnumerator InvokeAfter(Action task, float time)
    {
        yield return new WaitForSeconds(time);
        task();

    }
    public static void InvokeNextFrame(this MonoBehaviour target, Action task)
    {
        target.StartCoroutine(InvokeNextFrame(task));
    }
    public static IEnumerator InvokeNextFrame(Action task)
    {
        yield return null;
        task();

    }
    public static GameObject CreateParticleAt(GameObject prefab, GActor actor)
    {
        return GameObject.Instantiate(prefab, actor.transform.position + prefab.transform.position, prefab.transform.rotation, null);
    }
    public static GameObject CreateParticleAt(GameObject prefab, Vector2Int location)
    {
        return GameObject.Instantiate(prefab, GridManager.instance.GetChessPosition3D(location) + prefab.transform.position, prefab.transform.rotation, null);
    }
    public static Vector2Int GetRandomLocation(Vector2Int yRange)
    {
        return GetRandomLocation(new Vector2Int(0,yRange.x), new Vector2Int(GridManager.instance.size.x-1, yRange.y));
    }
    public static Vector2Int GetRandomLocation(Vector2Int leftButtom, Vector2Int rightTop)
    {
        System.Random random = new System.Random();
        Vector2Int t = new Vector2Int();
        t.y = random.Next(leftButtom.y, rightTop.y);
        t.x = random.Next(leftButtom.x, rightTop.x);
        return t;
    }
    public static Vector3 GetUpVector(this Grid grid)
    {
        switch (grid.cellSwizzle)
        {
            case GridLayout.CellSwizzle.XYZ:
                return new Vector3(0, 0, -1);
            case GridLayout.CellSwizzle.XZY:
                return new Vector3(0, 1, 0);

            default:
                Debug.LogError("Error Swizzle");
                return Vector3.zero;
        }
    }
    public static Vector2Int ToVector2(this Direction direction)
    {
        switch (direction)
        {
            case Direction.up:
                return Vector2Int.up;
            case Direction.down:
                return Vector2Int.down;
            case Direction.left:
                return Vector2Int.left;
            case Direction.right:
                return Vector2Int.right;
            default:
                throw new Exception("Error Direction");
        }
    }
    public static Direction ClockwiseNext(this Direction direction)
    {
        switch (direction)
        {
            case Direction.up:
                return Direction.right;
            case Direction.right:
                return Direction.down;
            case Direction.down:
                return Direction.left;
            case Direction.left:
                return Direction.up;
            default:
                throw new Exception("Error Direction");
        }
    }
    public static Direction ToDirection(this Vector2Int direction)
    {
        if (direction.x == 0)
        {
            if (direction.y == -1)
            {
                return Direction.down;
            }
            else if (direction.y == 1)
            {
                return Direction.up;
            }
            else
                throw new Exception("Error VectorInt");
        }
        else if (direction.y == 0)
        {
            if (direction.x == -1)
            {
                return Direction.left;
            }
            else if (direction.x == 1)
            {
                return Direction.right;
            }
            else
                throw new Exception("Error VectorInt");
        }
        else
            throw new Exception("Error VectorInt");
    }
}