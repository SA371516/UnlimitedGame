using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData : MonoBehaviour
{
    public static PlayerData _Data;

    public SaveData saveData;               //Jsonに書かれているものをすべて入れる
    public PlayerStatus _playerStatus;      //ゲームに必要な情報のみ保存する
    public int _getPoint;

    string savePath;                        //エディタとアプリケーションで分けるため
    const string saveFileName = "savedata.json";

    private void Awake()
    {
        if (_Data == null)
        {
            _Data = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //エディタ状態だと//保存場所を指定している
#if UNITY_EDITOR
        savePath = "";
#else
        savePath = Application.persistentDataPath + "/";
#endif
        LoadDate();
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
        foreach (var v in saveData.status)
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
    public List<WeaponStatus> weaponStatuses = new List<WeaponStatus>();
}
[System.Serializable]   //<--メモリに書き込むことが出来る
public class WeaponStatus
{
    public string WeaponName;
    public int BulletNum;
    public int WeaponATK;
    public int WeaponAccuracy;
    public int Levelcount;          //レベル10ごとに限界突破しなくてはならない
    public bool Exceeding_limit;    
    public bool OpenWeapon;
}
//実際に保存するクラス
public class SaveData
{
    public List<PlayerStatus> status = new List<PlayerStatus>();//ユーザー情報の保存用
}

