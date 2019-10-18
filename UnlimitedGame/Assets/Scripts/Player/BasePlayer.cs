﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class BasePlayer : MonoBehaviour
{
    UIManager _iManager;
    GameManager _manager;
    Confug _confug;

    int _hp;
    public int GetSetHP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    protected RaycastHit hit;
    protected Rigidbody rig;
    float speed;
    [SerializeField]
    GameObject _Mycamera;
    [SerializeField]
    Text _GetLog;
    Camera _Camera;
    BaseWeapon _haveWeapon;

    public List<Weapons> _weaponName = new List<Weapons>();

    KeyCode Dush;
    KeyCode Up;
    KeyCode Down;
    KeyCode Left;
    KeyCode Right;


    private void Awake()
    {
        _hp = 10;
    }

    protected virtual void Start()
    {
        _confug = Confug._confug;
        Up = _confug.StatusInctance<KeyCode>("",0);
        Down = _confug.StatusInctance<KeyCode>("",1);
        Left = _confug.StatusInctance<KeyCode>("",2);
        Right = _confug.StatusInctance<KeyCode>("",3);
        Dush = _confug.StatusInctance<KeyCode>("",4);


        speed = 5f;
        _iManager = GameObject.Find("Manager").GetComponent<UIManager>();
        _Camera = _Mycamera.GetComponent<Camera>();
        rig = GetComponent<Rigidbody>();
        _haveWeapon = null;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //移動
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        float h=0;
        float v=0;
        if (Input.GetKey(Up)) h = 1f;
        else if (Input.GetKey(Down)) h = -1f;
        if (Input.GetKey(Left)) v = 1f;
        else if (Input.GetKey(Right)) v = -1f;

        if (Input.GetKey(Dush))
        {
            if (Input.GetKeyDown(Dush))
            {
                speed *= 2;
            }
        }
        else speed = 5f;
        rig.velocity = v * transform.forward * speed + h * transform.right * speed;
        //攻撃
        if (_haveWeapon != null)
        {
            _haveWeapon.Update();
        }
        ItemGet();

        if (_hp >= 0) return;
        Debug.Log("GameOver");
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
                        case "SR(Clone)":
                            _haveWeapon = new SRWeapon();
                            _iManager._weapon = _haveWeapon;

                            _weaponName.Add(Weapons.SR);
                            break;
                        case "AR(Clone)":
                            _haveWeapon = new ARWeapon();
                            _iManager._weapon = _haveWeapon;
                            _weaponName.Add(Weapons.AR);
                            break;
                    }
                    _GetLog.text = "";
                    Destroy(hit.collider.gameObject);
                }
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
