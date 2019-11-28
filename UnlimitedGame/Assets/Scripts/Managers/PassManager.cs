using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassManager : MonoBehaviour
{
    [SerializeField]
    InputField _userNameField, _passField;
    [SerializeField]
    Text _message;

    SaveScript[] saves;
    // Start is called before the first frame update
    void Start()
    {
        saves = Resources.LoadAll<SaveScript>("PlayerSaveDate");
    }

    public void AlreadyUser()
    {
        bool _chack = false;
        if (_userNameField.text == "" && _passField.text == "")
        {
            _message.text = "入力に不具合があります";
            return;
        }
        foreach(var v in saves)
        {
            if (_userNameField.text == v.UserName)
            {
                _chack = true;
                if (_passField.text == v.PassWord)
                {
                    Debug.Log("一致しました");
                    //SceneLoadManager._loadManager.LoadDate(v);
                    //SceneLoadManager._loadManager.SceneLoadFunction((int)SceneLoadManager.Scenes.Title);
                }
            }
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

    }
}
