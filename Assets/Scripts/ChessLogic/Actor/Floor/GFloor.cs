using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFloor : GActor
{
    public bool transitable=true;
    protected override void Awake()
    {
        base.Awake();
        RegistToManager();
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
