﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class BasePlayer : MonoBehaviour
{
    UIManager _uiManager;
    GameManager _manager;
    Confug _confug;
    KeyCode[] _keyCodes;

    protected int _hp;
    protected float _dushValume = 5f;
    public float GetSetDush
    {
        get { return _dushValume; }
        set { _dushValume = value; }
    }
    public int GetSetHP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    protected RaycastHit hit;
    protected Rigidbody rig;
    protected float speed;
    protected bool _once;
    protected bool _dushChack;

    [SerializeField]//プレイヤーに知らせるため
    GameObject _getLog;
    [SerializeField]
    Vector3 _playerGravity;
    [SerializeField]//プレイヤーの近くに銃があるようにしたい
    List<GameObject> _Weapon = new List<GameObject>();

    Camera _Camera;
    BaseWeapon _haveWeapon;
    bool _Invincible;
    float _InvincibleTime;
    Text _keyText;
    Text _keyText2;
    GameObject _keyImg;

    [HideInInspector]
    public List<Weapons> _weaponName = new List<Weapons>();//取得したものを入れる
    [HideInInspector]
    public bool _stop;



    private void Awake()
    {
        _hp = 10;
    }

    protected virtual void Start()
    {
        if (_stop) return;//デバッグも兼ねている
        _confug = Confug._confug;
        _keyCodes = _confug.GetConfugStatus<KeyCode[]>(new KeyCode[4]);
        speed = 5f;
        _dushValume = 5f;
        _manager = GameObject.Find("Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Manager").GetComponent<UIManager>();
        _Camera = transform.GetChild(0).GetComponent<Camera>();
        _keyImg = _getLog.transform.GetChild(0).gameObject;
        _keyText = _getLog.transform.GetChild(1).GetComponent<Text>();
        _keyText2 = _getLog.transform.GetChild(2).GetComponent<Text>();
        rig = GetComponent<Rigidbody>();
        _haveWeapon = null;
        _stop = false;
        _Invincible = false;
        _once = true;
    }

    protected virtual void Update()
    {
        if (_stop) return;//ゲームを止める
        else if (_hp<=0) {
            _manager.GameOver(transform.position);//プレイヤーの位置を向かせるため
            if (_once)
            {
                _once = false;
            }
            return;
        }
        _keyCodes = _confug.GetConfugStatus<KeyCode[]>(new KeyCode[4]);
        float h =0;
        float v=0;
        if (Input.GetKey(_keyCodes[0])) v = 1f;//上
        else if (Input.GetKey(_keyCodes[1])) v = -1f;//下
        if (Input.GetKey(_keyCodes[2])) h = -1f;//左
        else if (Input.GetKey(_keyCodes[3])) h = 1f;//右
        if (Input.GetKey(_keyCodes[4]) && GetSetDush > 0f&&_dushChack)//ダッシュ
        {
            GetSetDush -= Time.deltaTime;
            if (Input.GetKeyDown(_keyCodes[4]))
            {
                speed = 10;
            }
            if (GetSetDush <= 0f && _dushChack) { speed = 5f; _dushChack = false; }//走る限界に達した時
        }
        else if (!_dushChack)
        {
            speed = 5f;
            _dushValume += Time.deltaTime;
            if (_dushValume >= 5f)
            {
                _dushValume = 5f;
                _dushChack = true;
            }
        }
        else
        {//何も入力されていない
            speed = 5f;
            _dushValume += Time.deltaTime;
            if (_dushValume >= 5f) _dushValume = 5f;
        }
        //SE
        if (speed == 10f) SoundManager._soundManager.PlaySESound(SoundManager.SE.Dush, 2);
        else if (h != 0 || v != 0) SoundManager._soundManager.PlaySESound(SoundManager.SE.Walk, 2);
        else SoundManager._soundManager.StopSESound(2);
        rig.velocity = v * transform.forward * speed + h * transform.right * speed;
        //攻撃
        if (_haveWeapon != null)
        {
            _haveWeapon.Update();
        }
        ItemGet();
    }

    protected virtual void FixedUpdate()
    {
        if (_stop) return;
        rig.AddForce(_playerGravity);
        if (_InvincibleTime + 3f < Time.time)//無敵時間
        {
            _Invincible = false;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetSetHP = 0;
        }
    }

    protected void ItemGet()
    {
        Ray ray = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        if (!Physics.Raycast(ray, out hit, 2f)) {
            _keyText.text = "";
            _keyText2.text = "";
            _keyImg.SetActive(false);
            return;
        }
        switch (hit.collider.gameObject.tag)
        {
            case "Weapons":
                string str = _keyCodes[5].ToString();
                string str2="で武器を取る";
                _keyImg.SetActive(true);
                _keyText.text = str;
                _keyText2.text = str2;
                if (Input.GetKeyDown(_keyCodes[5]))
                {
                    GameObject v = null;
                    switch (hit.collider.gameObject.name)
                    {
                        case "SR(Clone)":
                            v = _Weapon.Find(item => item.gameObject.name == "P_SR");
                            _haveWeapon = new SRWeapon(transform.GetChild(0).GetChild(0).gameObject);//(Muzzle particle)
                            _uiManager._weapon = _haveWeapon;
                            _weaponName.Add(Weapons.SR);
                            break;
                        case "AR(Clone)":
                            v = _Weapon.Find(item => item.gameObject.name == "P_AR");
                            _haveWeapon = new ARWeapon(transform.GetChild(0).GetChild(0).gameObject);//(Muzzle particle)
                            _uiManager._weapon = _haveWeapon;
                            _weaponName.Add(Weapons.AR);
                            break;
                    }
                    _keyText.text = "";
                    _keyText2.text = "";
                    Destroy(hit.collider.gameObject);//取得した武器を消去
                }
                break;
            default:
                //Debug.Log(hit.collider.gameObject.name);
                break;
        }
    }

    public void DamageMove(Vector3 vec,int ATK)
    {
        if (_stop) return;//デバッグ用
        if (!_Invincible)//無敵
        {
            transform.position = transform.position + (transform.position - vec);
            GetSetHP -= ATK;
            _Invincible = true;
            _uiManager.DamageFunction();
            _InvincibleTime = Time.time;
        }
    }
}
