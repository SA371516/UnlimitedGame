using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public  class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager _loadManager;
    public PlayerStatus saveScript;
    public int _playerDateID;
    public int _getPoint;
    private void Awake()
    {
        if (_loadManager == null)
        {
            _loadManager = this;
            //LoadDate();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadDate();
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
            if(!SaveDate(saveScript))
            {
                Debug.Log("セーブ失敗");
                return;
            }
            Application.Quit();
        }
        else 
            SceneManager.LoadScene(scenes.ToString());
    }

    public bool SaveDate(PlayerStatus s)
    {
        string jsonstr = JsonUtility.ToJson(s);
        bool _chack = true;
        try
        {
            using (StreamWriter streamWriter = new StreamWriter(/*Application.dataPath*/Application.persistentDataPath + "/savedata.json"))
            {
                streamWriter.Write(jsonstr);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch (System.Exception e)
        {
            _chack = false;
        }
        return _chack;
    }
    //データがないとき
    void CreateData()
    {
        PlayerStatus s = new PlayerStatus();
        string jsonstr = JsonUtility.ToJson(s);
        try
        {
            using (StreamWriter streamWriter = new StreamWriter(/*Application.dataPath*/Application.persistentDataPath + "/savedata.json"))
            {
                streamWriter.Write(jsonstr);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void LoadDate()
    {
        FileInfo fileInfo = new FileInfo(/*Application.dataPath*/Application.persistentDataPath + "/savedata.json");
        if (!fileInfo.Exists)
        {
            Debug.Log("ファイルが存在しておりません");
            CreateData();
        }

        string dataStr = "";
        PlayerStatus script = new PlayerStatus();
        try
        {
            using (StreamReader streamReader = new StreamReader(/*Application.dataPath*/Application.persistentDataPath + "/savedata.json"))
            {
                dataStr = streamReader.ReadToEnd();
                streamReader.Close();
                script = JsonUtility.FromJson<PlayerStatus>(dataStr);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("ERROR:" + e.ToString());
            return;
        }

        saveScript = script;//ここでデータを挿入
    }
}

public class PlayerStatus
{
    public List<PlayerStatus> status = new List<PlayerStatus>();//保存用

    public string UserName;
    public string PassWord;
    public int Point;
    public int SRLevel;
    public int ARLevel;
}

