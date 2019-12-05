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

    void Start()
    {
        _createPanel.SetActive(false);
        _passPanel.SetActive(true);
    }
    //===========ログインボタン入力時============
    public void AlreadyUser()
    {
        bool _chack = false;
        if (!_inputchack[0]|| !_inputchack[1])
        {
            _message.text = "入力に不具合があります";
            return;
        }
        int id = 0;
        foreach(var v in SceneLoadManager._loadManager.saveData.status)
        {
            if (_userNameField.text == v.UserName)
            {
                _chack = true;
                if (_passField.text == v.PassWord)//一致したら
                {
                    SceneLoadManager._loadManager._playerStatus = v;
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
    //=========ボタン入力時に====================
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

        SceneLoadManager._loadManager._playerStatus = instance;
        SceneLoadManager._loadManager.saveData.status.Add(instance);
        SceneLoadManager._loadManager.SceneLoadFunction((int)SceneLoadManager.Scenes.Title);
    }
    //=============削除ボタン====================
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
    //=======ログインと新規作成を切り替える======
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
    //=======Fieldに書かれたときに呼ばれる=======
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
    //=======入力文字を判定させる処理============
    void InputChack(string str,int i)
    {
        if (!IsStringByte(str)||str=="")
        {
            if (str == "")
            {
                _message.text = "";
            }
            _message.text = "半角文字を入力してください";
            _inputchack[i] = false;
        }
        else
        {
            _message.text = "";
            _inputchack[i] = true;
        }

    }
    //==============バイト判定===================
    bool IsStringByte(string str)
    {
        int c = str.Length;                                     //文字数
        Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        int num = sjisEnc.GetByteCount(str);                    //バイト数

        int byte2 = num - c;
        return (c == 0);
    }
}