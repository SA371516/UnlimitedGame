using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform verRot;
    public Transform horRot;

    // Use this for initialization
    void Start()
    {
        verRot = transform.parent;
        horRot = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float X_Rotation = Input.GetAxis("Mouse X")*3;
        float Y_Rotation = Input.GetAxis("Mouse Y")*3;
        verRot.transform.Rotate(0, X_Rotation, 0);
        horRot.transform.Rotate(-Y_Rotation, 0, 0);
    }
}
