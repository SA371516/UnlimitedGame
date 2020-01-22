using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class PlayerData : MonoBehaviour
{
    public static PlayerData _Data;

    public SaveData saveData;               //Jsonに書かれているものをすべて入れる
    public PlayerStatus _playerStatus;      //遊んでいるプレイヤーの情報のみ保存する
    public int _getPoint;
    public bool _debug;

    string savePath;                        //エディタとアプリケーションで分けるため
    const string saveFileName = "savedata.json";
    const string EncryptKey = "c6eahbq9sjuawhvdr9kvhpsm5qv393ga";
    const string PasswordChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly int PasswordCharsLength = PasswordChars.Length;
    const int EncryptPasswordCount = 16;

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
        bool _chack = true;
        string iv;
        string base64;
        EncryptAesBase64(jsonstr, out iv, out base64);
        try
        {
            //using (StreamWriter streamWriter = new StreamWriter(savePath + saveFileName))
            //{
            //    streamWriter.Write(jsonstr);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}
            // 保存
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);
            using (FileStream fs = new FileStream(savePath + saveFileName, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(ivBytes.Length);
                bw.Write(ivBytes);

                bw.Write(base64Bytes.Length);
                bw.Write(base64Bytes);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            _chack = false;
        }
        Debug.Log("SEVE;" + jsonstr);
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
        byte[] ivBytes = null;
        byte[] base64Bytes = null;
        SaveData script = new SaveData();
        try
        {
            using (FileStream fs=new FileStream(savePath+saveFileName,FileMode.Open,FileAccess.Read))
            //using (StreamReader streamReader = new StreamReader(fs))//書かれているデータをそのまま読み込み
            using (BinaryReader br =new BinaryReader(fs))//暗号化に必要
            {
                int length = br.ReadInt32();
                ivBytes = br.ReadBytes(length);

                length = br.ReadInt32();
                base64Bytes = br.ReadBytes(length);

                //dataStr = streamReader.ReadToEnd();
                //streamReader.Close();
            }
            string iv = Encoding.UTF8.GetString(ivBytes);
            string base64 = Encoding.UTF8.GetString(base64Bytes);
            DecryptAesBase64(base64, iv, out dataStr);
            script = JsonUtility.FromJson<SaveData>(dataStr);
            Debug.Log("LORD："+dataStr);
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
        string iv;
        string base64;
        EncryptAesBase64(jsonstr, out iv, out base64);
        Debug.Log("[Encrypt]json:" + jsonstr);
        Debug.Log("[Encrypt]base64:" + base64);

        try
        {
            //using (StreamWriter streamWriter = new StreamWriter(savePath + saveFileName))
            //{
            //    streamWriter.Write(jsonstr);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}
            // 保存
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);
            using (FileStream fs = new FileStream(savePath+saveFileName, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(ivBytes.Length);
                bw.Write(ivBytes);

                bw.Write(base64Bytes.Length);
                bw.Write(base64Bytes);
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
    /// <summary>
    /// AES複合化(Base64形式)
    /// </summary>
    public static void DecryptAesBase64(string base64, string iv, out string json)
    {
        byte[] src = Convert.FromBase64String(base64);
        byte[] dst;
        DecryptAes(src, iv, out dst);
        json = Encoding.UTF8.GetString(dst).Trim('\0');
    }
    /// <summary>
    /// AES複合化
    /// </summary>
    public static void DecryptAes(byte[] src, string iv, out byte[] dst)
    {
        dst = new byte[src.Length];
        using (RijndaelManaged rijndael = new RijndaelManaged())
        {
            rijndael.Padding = PaddingMode.PKCS7;
            rijndael.Mode = CipherMode.CBC;
            rijndael.KeySize = 256;
            rijndael.BlockSize = 128;

            byte[] key = Encoding.UTF8.GetBytes(EncryptKey);
            byte[] vec = Encoding.UTF8.GetBytes(iv);

            using (ICryptoTransform decryptor = rijndael.CreateDecryptor(key, vec))
            using (MemoryStream ms = new MemoryStream(src))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {
                cs.Read(dst, 0, dst.Length);
            }
        }
    }
    /// <summary>
    /// AES暗号化(Base64形式)
    /// </summary>
    public static void EncryptAesBase64(string json, out string iv, out string base64)
    {
        byte[] src = Encoding.UTF8.GetBytes(json);
        byte[] dst;
        EncryptAes(src, out iv, out dst);
        base64 = Convert.ToBase64String(dst);
    }
    /// <summary>
    /// AES暗号化
    /// </summary>
    public static void EncryptAes(byte[] src, out string iv, out byte[] dst)
    {
        iv = CreatePassword(EncryptPasswordCount);
        dst = null;
        using (RijndaelManaged rijndael = new RijndaelManaged())
        {
            rijndael.Padding = PaddingMode.PKCS7;
            rijndael.Mode = CipherMode.CBC;
            rijndael.KeySize = 256;
            rijndael.BlockSize = 128;

            byte[] key = Encoding.UTF8.GetBytes(EncryptKey);
            byte[] vec = Encoding.UTF8.GetBytes(iv);

            using (ICryptoTransform encryptor = rijndael.CreateEncryptor(key, vec))
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(src, 0, src.Length);
                cs.FlushFinalBlock();
                dst = ms.ToArray();
            }
        }
    }
    /// <summary>
    /// パスワード生成
    /// </summary>
    /// <param name="count">文字列数</param>
    /// <returns>パスワード</returns>
    public static string CreatePassword(int count)
    {
        StringBuilder sb = new StringBuilder(count);
        for (int i = count - 1; i >= 0; i--)
        {
            char c = PasswordChars[UnityEngine.Random.Range(0, PasswordCharsLength)];
            sb.Append(c);
        }
        return sb.ToString();
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

