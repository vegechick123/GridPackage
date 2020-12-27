using System;
using System.Collections.Generic;
using UnityEngine;


public class Victory : MonoBehaviour, UIBase
{
    public void OnClik1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UIManager.instance.LevelIndex++);
    }

    public void OnClik2()
    {
        throw new NotImplementedException();
    }

    public void OnClik3()
    {
        throw new NotImplementedException();
    }
}

