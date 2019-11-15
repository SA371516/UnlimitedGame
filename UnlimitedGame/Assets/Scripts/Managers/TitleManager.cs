using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    SceneLoadManager _loadManager;
    // Start is called before the first frame update
    void Start()
    {
        _loadManager = GameObject.Find("DontDes").GetComponent<SceneLoadManager>();
    }

    public void OnClick(int i)
    {
        _loadManager.SceneLoadFunction(i);
    }
}
