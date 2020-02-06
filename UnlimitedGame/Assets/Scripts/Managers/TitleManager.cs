using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    Text UserNameDis;

    string _name;

    private void Start()
    {
        if (PlayerData._Data._playerStatus != null)
        {
            _name = PlayerData._Data._playerStatus.UserName;
            UserNameDis.text = _name + "さん、お疲れ様です！！";
        }
        SoundManager._soundManager.PlayBGMSound(SoundManager.BGM.Title);
    }
    public void OnClick(int i)
    {
        SoundManager._soundManager.StopBGM();
        SceneLoadManager._loadManager.SceneLoadFunction(i);
    }
}
