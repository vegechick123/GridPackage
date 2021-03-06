﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle:MonoBehaviour
{
    private void Start()
    {
    }
    private void FixedUpdate()
    {
        CheckWin();
        CheckLose();
    }
    void CheckWin()
    {
            foreach (var tran in GetAllChilds())
            {
                if (tran.childCount != 0 || tran.GetComponent<EnemyBorn>().isEnd == false)
                    return;
            }
        Win();
    }
    void CheckLose()
    {
        foreach (var tran in GetAllChilds())
        {
            GChess chess = GridManager.instance.GetChess(tran.GetComponent<EnemyBorn>().destination);
            if (chess != null
                && chess.chessType == ChessType.ememy)
                Lose();
        }
    }
    void Win()
    {
        LevelManager.instance.Win();
    }
    void Lose()
    {
        LevelManager.instance.Lose();
        //Time.timeScale = 0;
    }
    Transform[] GetAllChilds()
    {
        Queue<Transform> transforms = new Queue<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            transforms.Enqueue(transform.GetChild(i));
        }
        return transforms.ToArray();
    }
}

