using System.Collections;
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
    Text _GetLog;
    [SerializeField]
    Vector3 _playerGravity;
    Camera _Camera;
    BaseWeapon _haveWeapon;

    [HideInInspector]
    public List<Weapons> _weaponName = new List<Weapons>();//取得したものを入れる
    [HideInInspector]
    public bool _stop;

    KeyCode[] _keyCodes;


    private void Awake()
    {
        _hp = 10;
    }

    protected virtual void Start()
    {
        _confug = Confug._confug;
        _keyCodes = _confug.StatusInctance<KeyCode[]>();

        speed = 5f;
        _iManager = GameObject.Find("Manager").GetComponent<UIManager>();
        _Camera = transform.GetChild(0).GetComponent<Camera>();
        rig = GetComponent<Rigidbody>();
        _haveWeapon = null;
        _stop = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_stop) return;//ゲームを止める
        _keyCodes = _confug.StatusInctance<KeyCode[]>();
        float h =0;
        float v=0;
        if (Input.GetKey(_keyCodes[0])) v = 1f;
        else if (Input.GetKey(_keyCodes[1])) v = -1f;
        if (Input.GetKey(_keyCodes[2])) h = -1f;
        else if (Input.GetKey(_keyCodes[3])) h = 1f;

        if (Input.GetKey(_keyCodes[4]))
        {
            if (Input.GetKeyDown(_keyCodes[4]))
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

    protected virtual void FixedUpdate()
    {
        rig.AddForce(_playerGravity);
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

    public void DamageMove(Vector3 vec)
    {
        Vector3 _force = (vec - transform.position)*10;
        _force.y = -22;
        GetSetHP--;
        rig.AddForce(-_force, ForceMode.Impulse);
        //rig.AddForce(new Vector3(0, 22, 60),ForceMode.Impulse);
    }
}
