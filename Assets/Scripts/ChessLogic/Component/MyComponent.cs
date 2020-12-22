using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyComponent : MonoBehaviour
{
    [HideInInspector]
    public GChess chess;
    public Vector2Int location { get { return chess.location; }set { chess.location = value; } }
    virtual protected void Awake()
    {
        chess = GetComponent<GChess>();
    }
}
