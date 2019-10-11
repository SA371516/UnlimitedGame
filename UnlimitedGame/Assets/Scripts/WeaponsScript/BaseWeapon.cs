using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWeapon 
{
    Camera Mycamera;
    protected Ray ray;
    protected RaycastHit hit;
    protected float _distance;
    protected float _recustTime;
    protected Slider _recustSlider;

    // Start is called before the first frame update
    protected BaseWeapon()
    {
        Mycamera = GameObject.Find("FirstPersonCamera").GetComponent<Camera>();
        _recustSlider = GameObject.Find("Slider").GetComponent<Slider>();
    }
    protected Ray GetRay()
    {
       return Mycamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 10));
    }

    // Update is called once per frame
    public abstract void Update();

    public abstract void Attack();
}
