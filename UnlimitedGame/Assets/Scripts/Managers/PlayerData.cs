﻿using System;
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

    //========ユーザーを作成する関数================
    //デバッグにも使用するためここに書く
    public PlayerStatus CreateUserData(string n=null,string p=null)
    {
        var instance = new PlayerStatus();
        instance.UserName = n;
        instance.PassWord = p;

        foreach (var v in Enum.GetValues(typeof(Weapons)))        //武器の数回す
        {
            var s = new WeaponStatus();
            s.WeaponName = v.ToString();             //現在の要素の名前を取得//武器を登録
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
    public int BulletNum = 0;
    public float WeaponATK = 1;
    public float WeaponAccuracy = 1;
    public int ExceedingLevel = 1;      //現在のレベル
    public int Levelcount;          //レベル10ごとに限界突破しなくてはならない
    public bool OpenWeapon;         //武器が解放されているか
}
//実際に保存するクラス
public class SaveData
{
    public List<PlayerStatus> status = new List<PlayerStatus>();//ユーザー情報の保存用
}

