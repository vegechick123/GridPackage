using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : Manager<LevelManager>
{
    public Animator transitionMask;
    public Animator winAnimator;
    public Animator loseAnimator;
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
    public void Win()
    {
        winAnimator.SetTrigger("In");
        this.InvokeAfter(NextLevel, 2f);
    }
    public void Lose()
    {
        loseAnimator.SetTrigger("In");
        this.InvokeAfter(ReloadLevel, 2f);
    }

}
