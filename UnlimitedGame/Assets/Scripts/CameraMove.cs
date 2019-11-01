using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Confug _confug;
    Transform verRot;
    Transform horRot;
    Vector3 _gole;
    float _limit;//ゲームオーバー時
    float _move;//ゲームオーバー時
    public float _cameraMove;
    public bool _stop;
    void Start()
    {
        _confug = Confug._confug;
        verRot = transform.parent;
        horRot = GetComponent<Transform>();
        _cameraMove = _confug.StatusInctance<float>();
        _gole = transform.position + new Vector3(0, 10, 5);
        _move = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stop) return;
        _cameraMove = (int)_confug.StatusInctance<float>();
        float X_Rotation = Input.GetAxis("Mouse X")*_cameraMove;
        verRot.transform.Rotate(0, X_Rotation, 0);

        float Y_Rotation = Input.GetAxis("Mouse Y")*_cameraMove;
        _limit += Y_Rotation;
        ////角度制限
        if (_limit <= -75)
        {
            _limit = -75;
            return;
        }
        if (_limit >= 75)
        {
            _limit = 75;
            return;
        }
        verRot.transform.Rotate(0, X_Rotation, 0);
        horRot.transform.Rotate(-Y_Rotation, 0, 0);
    }
    
    public IEnumerator GameOver(Vector3 vec,float _jumpTime)
    {
        while (_move <= 1f)
        {
            transform.LookAt(vec);
            transform.position = Vector3.Lerp(transform.position, _gole, _move);
            _move += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        float time = 0f;
        while (time <= _jumpTime)
        {
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        SceneLoadManager._loadManager.SceneLoadFunction((int)SceneLoadManager.Scenes.Result);
    }
}
