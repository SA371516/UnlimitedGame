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
            //LoadDate();
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

    public void LoadDate(SaveScript saves)
    {
        saveScript = saves;

        string dataStr = "";
        StreamReader reader;

        reader = new StreamReader(Application.dataPath + "/savedata.json");
        dataStr = reader.ReadToEnd();
        reader.Close();

        SaveScript script = new SaveScript();
        script = JsonUtility.FromJson<SaveScript>(dataStr);

        saveScript.Point = script.Point;
        saveScript.SRLevel = script.SRLevel;
        saveScript.ARLevel = script.ARLevel;
    }
}
