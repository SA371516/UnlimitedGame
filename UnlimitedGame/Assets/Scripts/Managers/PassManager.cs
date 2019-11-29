using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PassManager : MonoBehaviour
{
    [SerializeField]
    GameObject _passPanel, _createPanel;
    [SerializeField]
    InputField _userNameField, _passField, _createUserField, _createPassField, _chackPassField;
    [SerializeField]
    Text _message;

    bool[] _inputchack = new bool[5]; //入力チェック

    static Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");//全角、半角チェック

    void Start()
    {
        _createPanel.SetActive(false);
        _passPanel.SetActive(true);
    }

    public void AlreadyUser()
    {
        bool _chack = false;
        if (!_inputchack[0]|| !_inputchack[1])
        {
            _message.text = "入力に不具合があります";
            return;
        }
        int id = 0;
        foreach(var v in SceneLoadManager._loadManager.saveScript.status)
        {
            if (_userNameField.text == v.UserName)
            {
                _chack = true;
                if (_passField.text == v.PassWord)//一致したら
                {
                    SceneLoadManager._loadManager._playerDateID = id;
                    SceneLoadManager._loadManager.SceneLoadFunction((int)SceneLoadManager.Scenes.Title);
                }
            }
            id++;
        }
        if (_chack)
        {
            _message.text = "パスワードが違います";
        }
        else
        {
            _message.text = "ユーザーがありません";
        }

    }

    public void CreateUser()
    {
        if (!_inputchack[2] || !_inputchack[3]||!_inputchack[4])
        {
            _message.text = "入力に不具合があります";
            return;
        }
        if (_createPassField.text != _chackPassField.text)
        {
            _message.text = "パスワードが一致していません";
            return;
        }

        var instance = new PlayerStatus();
        instance.UserName = _createUserField.text;
        instance.PassWord = _createPassField.text;

        //var directory = "Assets"+"/"+"Resources" + "/" + "PlayerSaveDate";           //オブジェクトの場所
        //var path = directory + "/" + "Save" + saves.Length + ".asset";  //ファイル名
        //var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);   //unityに必要なパスに変更する//「AssetDatabase」は拡張エディタなので別のを探す

        //AssetDatabase.CreateAsset(instance, uniquePath);//作成
        //AssetDatabase.SaveAssets();

        SceneLoadManager._loadManager.saveScript.status.Add(instance);
        SceneLoadManager._loadManager._playerDateID = SceneLoadManager._loadManager.saveScript.status.Count - 1;
        SceneLoadManager._loadManager.SceneLoadFunction((int)SceneLoadManager.Scenes.Title);
    }
    public void DeleteWord()
    {
        if (_passPanel.activeSelf)
        {
            _userNameField.text = "";
            _passField.text = "";
        }
        else if (_createPanel.activeSelf)
        {
            _createUserField.text = "";
            _createPassField.text = "";
            _chackPassField.text = "";
        }
    }
    public void PanelChange()
    {
        if (_passPanel.activeSelf)
        {
            _passPanel.SetActive(false);
            _createPanel.SetActive(true);
        }
        else if (_createPanel.activeSelf)
        {
            _passPanel.SetActive(true);
            _createPanel.SetActive(false);
        }
    }

    //Fieldに書かれたときに呼ばれる
    public void FieldChack(int i)
    {
        switch (i)
        {
            case 0:
                InputChack(_userNameField.text,i);
                break;
            case 1:
                InputChack(_passField.text,i);
                break;
            case 2:
                InputChack(_createUserField.text,i);
                break;
            case 3:
                InputChack(_createPassField.text,i);
                break;
            case 4:
                InputChack(_chackPassField.text, i);
                break;
        }
    }
    void InputChack(string str,int i)
    {
        int num = sjisEnc.GetByteCount(str);
        if (num == str.Length * 2 && str != "")//日本語は確定時に来る
        {
            _message.text = "半角文字を入力してください";
            return;
        }
        _message.text = "";
        //入力判定
        if (str == "")
        {
            _inputchack[i] = false;
        }
        else
            _inputchack[i] = true;

    }
}