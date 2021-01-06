using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : Manager<LevelManager>
{
    public Animator transitionMask;
    public int currentLevelNumber { get { return SceneManager.GetActiveScene().buildIndex; } }
    public void SwitchToLevel(int levelNumber)
    {
        transitionMask.SetTrigger("Out");
        this.InvokeAfter(()=>SceneManager.LoadScene(levelNumber),1.5f);
    }
    public void NextLevel()
    {
        SwitchToLevel(currentLevelNumber+1);
    }
    public void ReloadLevel()
    {
        SwitchToLevel(currentLevelNumber);
    }
}
