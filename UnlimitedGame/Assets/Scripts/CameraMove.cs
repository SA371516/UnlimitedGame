using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Confug _confug;
    Transform verRot;
    Transform horRot;
    Vector3 Gole;
    float _limit;//ゲームオーバー時
    float _move;//ゲームオーバー時
    public float _cameraMove;
    public bool _stop;
    public bool _gameOver;
    void Start()
    {
        _confug = Confug._confug;
        verRot = transform.parent;
        horRot = GetComponent<Transform>();
        _cameraMove = _confug.StatusInctance<float>();
        Gole = transform.position + new Vector3(0, 15, 5);
        _move = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stop) return;
        else if (_gameOver) return;
        _cameraMove = (int)_confug.StatusInctance<float>();
        float X_Rotation = Input.GetAxis("Mouse X")*_cameraMove;
        float Y_Rotation = Input.GetAxis("Mouse Y")*_cameraMove;
        _limit += Y_Rotation;
        ////角度制限
        //if (_limit <= -75) {
        //    _limit = -75;
        //    return;
        //}
        //if (_limit >= 75) {
        //    _limit = 75;
        //    return;
        //}
        verRot.transform.Rotate(0, X_Rotation, 0);
        horRot.transform.Rotate(-Y_Rotation, 0, 0);
    }
    public void GameOverMove(Vector3 vec)
    {
        transform.LookAt(vec);
        transform.position = Vector3.Lerp(transform.position, Gole,_move);
        _move += 0.5f;
    }
}
