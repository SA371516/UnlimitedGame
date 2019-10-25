using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Confug _confug;
    Transform verRot;
    Transform horRot;
    public float _cameraMove;
    public bool _stop;
    // Use this for initialization
    void Start()
    {
        _confug = Confug._confug;
        verRot = transform.parent;
        horRot = GetComponent<Transform>();
        _cameraMove = _confug.StatusInctance<float>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_stop) return;
        float X_Rotation = Input.GetAxis("Mouse X")*_cameraMove;
        float Y_Rotation = Input.GetAxis("Mouse Y")*_cameraMove;
        verRot.transform.Rotate(0, X_Rotation, 0);
        horRot.transform.Rotate(-Y_Rotation, 0, 0);
    }
}
