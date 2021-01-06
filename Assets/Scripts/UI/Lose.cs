using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour,UIBase
{
    public void OnClik1()
    {
        Time.timeScale = 1;
        LevelManager.instance.ReloadLevel();
    }

    public void OnClik2()
    {
        throw new System.NotImplementedException();
    }

    public void OnClik3()
    {
        throw new System.NotImplementedException();
    }
}
