using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour, UIBase
{
    public void OnClik1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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

