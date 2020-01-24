using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData : MonoBehaviour
{
    public static PlayerData _Data;

    public SaveData saveData;               //Jsonに書かれているものをすべて入れる
    public PlayerStatus _playerStatus;      //遊んでいるプレイヤーの情報のみ保存する
    public int _getPoint;//リザルトに表示するポイント
    public int _tankCount;//戦車撃破数
    public float _probability;//Hitした確率
    public bool _debug;

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
    void Start()
    {
        
        //↓エディタ状態だと//保存場所を指定している
#if UNITY_EDITOR
        savePath = "";
#else
        savePath = Application.persistentDataPath + "/";
#endif
        LoadDate();
    }

    //========ユーザーを作成する関数================
    //デバッグにも使用するためここに書き、publicにする
    public PlayerStatus CreateUserData(string n=null,string p=null)
    {
        var instance = new PlayerStatus();
        instance.UserName = n;
        instance.PassWord = p;

        foreach (var v in Enum.GetValues(typeof(Weapons)))        //存在する武器の数、回す
        {
            var s = new WeaponStatus();
            //=====================武器の初期化=====================
            s.WeaponName = v.ToString();
            switch (v)
            {
                case Weapons.SR:
                    s.WeaponAccuracy = 5f;
                    s.WeaponATK = 5;
                    s.BulletNum = 10;
                    break;
                case Weapons.AR:
                    s.BulletNum = 50;
                    s.WeaponATK = 1f;
                    s.WeaponAccuracy = 70f;
                    break;
            }
            s.ExceedingLevel = 1;
            s.Levelcount = 1;
            s.OpenWeapon = false;
            //==================選択画面時に必要=====================
            if (s.WeaponName == "SR")
            {
                s.OpenWeapon = true;
            }
            else
                s.OpenWeapon = false;
            instance.weaponStatuses.Add(s);
        }
        return instance;
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
        if (script == null)//ファイルが存在するのにデータがなくなるバグのため
        {
            CreateData();
            LoadDate();
            return;
        }

        saveData = script;//ここでデータを挿入
    }
    //===============データがないとき================
    void CreateData()
    {
        SaveData data = new SaveData();
        PlayerStatus s = new PlayerStatus();
        data.status.Add(s);
        string jsonstr = JsonUtility.ToJson(data);
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
    public void DataUpdate<T>(T item)
    {
        //===================渡された値を反映==============
        switch (item)
        {
            case int j://ポイントの変動
                _playerStatus.Point += j;
                break;
                
        }
        //====================データ更新==================
        for (int i = 0;i<saveData.status.Count;++i)
        {
            if (saveData.status[i].UserName == _playerStatus.UserName)
            {
                saveData.status[i] = _playerStatus;
                break;
            }
        }
        //===================データを保存する===============
        if (!SaveDate(saveData))
        {
            Debug.Log("セーブ失敗");
        }
    }
    //=============ゲーム終了時に呼ばれる=============
    private void OnApplicationQuit()
    {
        if (!_debug)
        {
            if (!SaveDate(saveData))
            {
                Debug.Log("セーブ失敗");
            }
        }
    }
}

//保存する情報
[Serializable]   //<--メモリに書き込むことが出来る
public class PlayerStatus
{
    public string UserName;
    public string PassWord;
    public int Point;
    public List<WeaponStatus> weaponStatuses = new List<WeaponStatus>();
}
[Serializable]   //<--メモリに書き込むことが出来る
public class WeaponStatus
{
    public string WeaponName;
    public int BulletNum;
    public float WeaponATK;
    public float WeaponAccuracy;
    public int ExceedingLevel;      //現在のレベル
    public int Levelcount;          //レベル10ごとに限界突破しなくてはならない
    public bool OpenWeapon;         //武器が解放されているか
    //コンストラクタ
    public WeaponStatus(WeaponStatus copy=null)
    {
        if (copy != null)
        {
            WeaponName = copy.WeaponName;
            BulletNum = copy.BulletNum;
            WeaponATK = copy.WeaponATK;
            WeaponAccuracy = copy.WeaponAccuracy;
            ExceedingLevel = copy.ExceedingLevel;
            Levelcount = copy.Levelcount;
            OpenWeapon = copy.OpenWeapon;
        }
    }
}
//実際に保存するクラス
public class SaveData
{
    public List<PlayerStatus> status = new List<PlayerStatus>();//ユーザー情報の保存用
}

