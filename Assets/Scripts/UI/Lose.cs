using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour,UIBase
{
    public void OnClik1()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UIManager.instance.LevelIndex);
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
