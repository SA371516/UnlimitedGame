using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public  class SceneLoadManager : MonoBehaviour
{
    public enum Scenes
    {
        Game=0,
        Title=1,
        Result = 2,
        Exit = 3,
    }
    public static SceneLoadManager _loadManager;
    public SaveData saveData;               //Jsonに書かれているものをすべて入れる
    public PlayerStatus _playerStatus;      //ゲームに必要な情報のみ保存する
    public int _getPoint;

    string savePath;                        //エディタとアプリケーションで分けるため
    const string saveFileName = "savedata.json";
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

    private void Start()
    {
        //エディタ状態だと
#if UNITY_EDITOR
        savePath = "";
#else
        savePath = Application.persistentDataPath + "/";
#endif
        LoadDate();
    }
    //===================シーン移動===================
    public void SceneLoadFunction(int i)
    {
        Scenes scenes = (Scenes)i;
        if (scenes == Scenes.Exit)
        {
            if (!SaveDate(saveData))
            {
                Debug.Log("セーブ失敗");
                return;
            }
            Application.Quit();
        }
        else 
            SceneManager.LoadScene(scenes.ToString());
    }
    //===================保存関数===================
    bool SaveDate(SaveData s)
    {
        string jsonstr = JsonUtility.ToJson(s);
        Debug.Log("SEVE;" + jsonstr);
        bool _chack = true;
        try
        {
            using (StreamWriter streamWriter = new StreamWriter(savePath + saveFileName))
            {
                streamWriter.Write(jsonstr);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            _chack = false;
        }
        return _chack;
    }
    //==============読み込み関数====================
    void LoadDate()
    {
        FileInfo fileInfo = new FileInfo(savePath + saveFileName);
        if (!fileInfo.Exists)
        {
            Debug.Log("ファイルが存在しておりません");
            CreateData();
        }

        string dataStr = "";
        SaveData script = new SaveData();
        try
        {
            using (StreamReader streamReader = new StreamReader(savePath + saveFileName))
            {
                dataStr = streamReader.ReadToEnd();
                streamReader.Close();
                script = JsonUtility.FromJson<SaveData>(dataStr);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("ERROR:" + e.ToString());
            return;
        }

        saveData = script;//ここでデータを挿入
    }
    //===============データがないとき================
    void CreateData()
    {
        PlayerStatus s = new PlayerStatus();
        string jsonstr = JsonUtility.ToJson(s);
        try
        {
            using (StreamWriter streamWriter = new StreamWriter(savePath + saveFileName))
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
    //==============データを更新するため===============
    public void DataUpdate()
    {
        int i = 0;
        foreach(var v in saveData.status)
        {
            if (v.UserName == _playerStatus.UserName)
            {
                saveData.status[i] = _playerStatus;
            }
            i++;
        }

        //データを保存する
        if (!SaveDate(saveData))
        {
            Debug.Log("セーブ失敗");
        }
    }
    //=============ゲーム終了時に呼ばれる=============
    private void OnApplicationQuit()
    {
        if (!SaveDate(saveData))
        {
            Debug.Log("セーブ失敗");
        }
    }


}

//保存する情報
[System.Serializable]   //<--メモリに書き込むことが出来る
public class PlayerStatus
{
    public string UserName;
    public string PassWord;
    public int Point;
    public int SRLevel;
    public int ARLevel;
}
//実際に保存するクラス
public class SaveData
{
    public List<PlayerStatus> status = new List<PlayerStatus>();//ユーザー情報の保存用
}

