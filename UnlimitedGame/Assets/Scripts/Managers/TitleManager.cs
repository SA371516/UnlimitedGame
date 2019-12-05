using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    Text UserNameDis;

    string name;

    private void Start()
    {
        name = SceneLoadManager._loadManager._playerStatus.UserName;
        UserNameDis.text = name + "さん、お疲れ様です！！";
    }
    public void OnClick(int i)
    {
       SceneLoadManager._loadManager.SceneLoadFunction(i);
    }
}
