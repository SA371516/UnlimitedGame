using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public  class SceneLoadManager : MonoBehaviour
{
    public enum Scenes
    {
        Game=0,
        Title=1,
        Result = 2,
        Teaming=3,
        Exit = 4,
    }
    public static SceneLoadManager _loadManager;

    private void Awake()
    {
        if (_loadManager == null)
        {
            _loadManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //===================シーン移動===================
    public void SceneLoadFunction(int i)
    {
        Scenes scenes = (Scenes)i;
        if (scenes == Scenes.Exit)
        {
            Application.Quit();
        }
        else 
            SceneManager.LoadScene(scenes.ToString());
    }
}

