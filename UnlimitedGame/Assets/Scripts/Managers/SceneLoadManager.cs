using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public  class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager _loadManager;
    public SaveScript saveScript;
    public int _getPoint;
    private void Awake()
    {
        if (_loadManager == null)
        {
            _loadManager = this;
            DontDestroyOnLoad(gameObject);
            LoadDate(out saveScript);
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
    public void SceneLoadFunction(int i)
    {
        Scenes scenes = (Scenes)i;
        if (scenes == Scenes.Exit)
        {
            SaveDate(saveScript);
            Application.Quit();
        }
        else 
            SceneManager.LoadScene(scenes.ToString());
    }

    void SaveDate(SaveScript s)
    {
        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson(s);
        writer = new StreamWriter(Application.dataPath+ "/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    void LoadDate(out SaveScript s)
    {
        s = Resources.Load("Save") as SaveScript;

        string dataStr = "";
        StreamReader reader;

        reader = new StreamReader(Application.dataPath + "/savedata.json");
        dataStr = reader.ReadToEnd();
        reader.Close();

        SaveScript script = new SaveScript();
        script = JsonUtility.FromJson<SaveScript>(dataStr);

        s.Point = script.Point;
        s.SRLevel = script.SRLevel;
        s.ARLevel = script.ARLevel;
    }
}
