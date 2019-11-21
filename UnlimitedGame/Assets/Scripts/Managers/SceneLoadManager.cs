using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public  class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager _loadManager;
    public int _score;
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
    public enum Scenes
    {
        Game=0,
        Title=1,
        Result = 2,
        Exit = 3,
    }
    public void SceneLoadFunction(int i,bool re=false)
    {
        Scenes scenes = (Scenes)i;
        SceneManager.LoadScene(scenes.ToString());
    }
}
