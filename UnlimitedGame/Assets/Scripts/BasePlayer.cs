﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class BasePlayer : MonoBehaviour
{
    protected RaycastHit hit;
    protected Rigidbody rig;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject _Mycamera;
    [SerializeField]
    Text _GetLog;
    Camera _Camera;
    BaseWeapon _haveWeapon;
    UIManager _iManager;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _iManager = GameObject.Find("Manager").GetComponent<UIManager>();
        _Camera = _Mycamera.GetComponent<Camera>();
        rig = GetComponent<Rigidbody>();
        _haveWeapon = null;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //移動
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rig.velocity = v * transform.forward * speed + h * transform.right * speed;
        //攻撃
        if (_haveWeapon != null)
        {
            _haveWeapon.Update();
        }
        ItemGet();
        
    }
    
    protected void ItemGet()
    {
        Ray ray = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        if (!Physics.Raycast(ray, out hit, 2f)) {
            _GetLog.text = "";
            return;
        }
        switch (hit.collider.gameObject.tag)
        {
            case "Weapons":
                _GetLog.text = "[E]で武器を取る";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switch (hit.collider.gameObject.name)
                    {
                        case "SR":
                            _haveWeapon = new SRWeapon();
                            _iManager._weapon = _haveWeapon;
                            break;
                        case "AR":
                            _haveWeapon = new ARWeapon();
                            _iManager._weapon = _haveWeapon;
                            break;
                    }
                    _GetLog.text = "";
                    Destroy(hit.collider.gameObject);
                }
                break;
        }

    }
}
