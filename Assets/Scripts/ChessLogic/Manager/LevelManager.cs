using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : Manager<LevelManager>
{
    public int currentLevelNumber { get { return SceneManager.GetActiveScene().buildIndex; } }
    public void SwitchToLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(currentLevelNumber+1);
    }
}
