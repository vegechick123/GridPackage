using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CController : MyComponent
{
    
    void Update()
    {
        if (chess.moveComponent.state == MoveState.Idle)
        {
            Vector2Int moveDir=Vector2Int.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveDir=Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                moveDir = Vector2Int.down;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                moveDir = Vector2Int.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                moveDir = Vector2Int.right;
            }
            if (moveDir != Vector2Int.zero)
            {
                Vector2Int destination = chess.location + moveDir;
                chess.MoveTo(destination);
            }
        }

    }
}
