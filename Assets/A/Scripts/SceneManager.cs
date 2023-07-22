using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    Title,
    InGame
}
public class SceneManager : Singleton<SceneManager>
{
    protected override bool IsDontDestroying => true;
    public void SceneLoad(SceneType sceneType)
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneType);
    }
}
