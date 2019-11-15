using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int GetSetHP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    protected RaycastHit hit;
    protected Rigidbody rig;
    protected float speed;

    [SerializeField]//プレイヤーに知らせるため
    Text _GetLog;
    [SerializeField]
    Vector3 _playerGravity;
    [SerializeField]
    List<GameObject> _Weapon = new List<GameObject>();

    Camera _Camera;
    BaseWeapon _haveWeapon;
    bool _Invincible;
    float _InvincibleTime;
    GameObject obj;

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
        _keyCodes = _confug.StatusInctance<KeyCode[]>();
        speed = 5f;
        _manager = GameObject.Find("Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Manager").GetComponent<UIManager>();
        _Camera = transform.GetChild(0).GetComponent<Camera>();
        rig = GetComponent<Rigidbody>();
        _haveWeapon = null;
        _stop = false;
        _Invincible = false;
    }

    protected virtual void Update()
    {
        if (_stop) return;//ゲームを止める
        _keyCodes = _confug.StatusInctance<KeyCode[]>();
        float h =0;
        float v=0;
        if (Input.GetKey(_keyCodes[0])) v = 1f;//上
        else if (Input.GetKey(_keyCodes[1])) v = -1f;//下
        if (Input.GetKey(_keyCodes[2])) h = -1f;//左
        else if (Input.GetKey(_keyCodes[3])) h = 1f;//右
        if (Input.GetKey(_keyCodes[4]))
        {
            if (Input.GetKeyDown(_keyCodes[4]))
            {
                speed *= 2;
            }
        }//ダッシュ
        else speed = 5f;
        rig.velocity = v * transform.forward * speed + h * transform.right * speed;
        //攻撃
        if (_haveWeapon != null)
        {
            _haveWeapon.Update();
        }
        ItemGet();

        if (_hp > 0) return;
        Debug.Log("GameOver");
        _manager.GameOver(transform.position);
    }

    protected virtual void FixedUpdate()
    {
        if (_stop) return;
        rig.AddForce(_playerGravity);
        if (_InvincibleTime + 3f < Time.time)//無敵時間
        {
            _Invincible = false;
        }
        _uiManager.DamageFunction(_Invincible);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetSetHP = 0;
        }
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
                string str= "[" + _keyCodes[5].ToString() + "]で武器を取る";
                _GetLog.text = str;
                if (Input.GetKeyDown(_keyCodes[5]))
                {
                    GameObject v = null;
                    Destroy(obj);
                    obj = null;
                    switch (hit.collider.gameObject.name)
                    {
                        case "SR(Clone)":
                            v = _Weapon.Find(item => item.gameObject.name == "P_SR");
                            //obj = _manager.ObjectInctance(v, transform.position,gameObject);//(武器,場所)
                            _haveWeapon = new SRWeapon(transform.GetChild(1).gameObject);//(particle)
                            _uiManager._weapon = _haveWeapon;
                            _weaponName.Add(Weapons.SR);
                            break;
                        case "AR":
                            v = _Weapon.Find(item => item.gameObject.name == "P_AR");
                            //obj = _manager.ObjectInctance(v, transform.position, gameObject);
                            _haveWeapon = new ARWeapon(transform.GetChild(1).gameObject);
                            _uiManager._weapon = _haveWeapon;
                            _weaponName.Add(Weapons.AR);
                            break;
                    }
                    _GetLog.text = "";
                    Destroy(hit.collider.gameObject);
                }
                break;
            default:
                Debug.Log(hit.collider.gameObject.name);
                break;
        }
    }

    public void DamageMove(Vector3 vec)
    {
        if (_stop) return;//デバッグ用
        if (!_Invincible)//無敵
        {
            transform.position = transform.position + (transform.position - vec);
            GetSetHP--;
            _Invincible = true;
            _InvincibleTime = Time.time;
        }
    }
}
