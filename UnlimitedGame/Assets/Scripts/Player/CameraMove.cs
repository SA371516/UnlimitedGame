using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Confug _confug;
    Transform verRot;
    Transform horRot;
    Vector3 _gole;
    float _XAngle;
    float _Yangle;
    float _move;//ゲームオーバー時
    public float _cameraMove;
    public bool _stop;
    void Start()
    {
        _confug = Confug._confug;
        verRot = transform.parent.GetComponent<Transform>();
        horRot = GetComponent<Transform>();
        _cameraMove = _confug.GetConfugStatus<float>(_cameraMove);
        _gole = transform.position + new Vector3(0, 10, 10);
        _move = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stop) return;
        _cameraMove = (int)_confug.GetConfugStatus<float>(_cameraMove);

        //横の回転
        float Y_Rotation = Input.GetAxis("Mouse X") * _cameraMove;
        _Yangle += Y_Rotation;
        var Q = Quaternion.Euler(0, _Yangle, 0);
        verRot.transform.rotation = Q;

        //縦の角度制限
        float X_Rotation = Input.GetAxis("Mouse Y") * _cameraMove;
        _XAngle += X_Rotation;
        var x = Mathf.Clamp(_XAngle, -75f, 75f);
        var q = Quaternion.Euler(-x, 0, 0);
        horRot.localRotation = q;

        if (_XAngle > 360) _XAngle -= 360;
        else if (_XAngle < -360) _XAngle += 360;
    }
    
    //カメラを一定時間見下ろした形にするため//何度もこの関数に来させる//プレイヤーの位置にカメラを向けるため
    public IEnumerator GameOver(Vector3 vec,float _jumpTime,int S,bool once)
    {
        _stop = true;
        //if (once)//一度しか来ないように
        //{
        //    PlayerData._Data._getPoint = S;
        //}
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
            transform.LookAt(vec);
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        SceneLoadManager._loadManager.SceneLoadFunction((int)SceneLoadManager.Scenes.Result);
    }
}
