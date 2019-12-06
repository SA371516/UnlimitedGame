using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWeapon 
{
    Camera Mycamera;
    protected Ray ray;
    protected RaycastHit hit;       
    protected GameManager _manager;
    protected GameObject _particle; //血しぶき
    protected ParticleScr particleScr;
    protected float _distance;      //射程距離
    protected float _recustTime;    //次弾発射
    protected Slider _recustSlider;
    protected int _Damage;          //攻撃力
    protected int _bulletNum;       //弾数
    protected int _accuracy;        //精度
    protected string _weaponName;
    protected Vector3 _bulletPos;
    protected WeaponStatus _status;

    public string GetSetWeaponName
    {
        get { return _weaponName; }
        set { _weaponName=value; }
    }
    public int GetSetBulletNum
    {
        get { return _bulletNum; }
        set { _bulletNum = value; }
    }
    

    // Start is called before the first frame update
    protected BaseWeapon(GameObject p)
    {
        _particle = Resources.Load("Prefabs/Damage") as GameObject;
        particleScr = p.GetComponent<ParticleScr>();
        _recustSlider = GameObject.Find("Slider").GetComponent<Slider>();
        _manager = GameObject.Find("Manager").GetComponent<GameManager>();
        Mycamera = GameObject.Find("FirstPersonCamera").GetComponent<Camera>();
    }
    protected Ray GetBulletItem(int a)
    {
        Vector2 vec = Random.insideUnitCircle * a;
        _bulletPos = new Vector3(Screen.width / 2 + vec.x, Screen.height / 2 + vec.y, 10);
        return Mycamera.ScreenPointToRay(_bulletPos);
    }

    // Update is called once per frame
    public abstract void Update();

    protected abstract void Attack();
}
