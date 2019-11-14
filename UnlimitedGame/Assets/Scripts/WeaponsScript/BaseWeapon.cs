using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWeapon 
{
    Camera Mycamera;
    protected GameManager _manager;
    protected GameObject _particle;
    protected ParticleSystem _bulletParticle;
    protected particleScr particleScr;
    protected Ray ray;
    protected RaycastHit hit;
    protected float _distance;
    protected float _recustTime;
    protected Slider _recustSlider;
    protected int _Damage;
    protected int _bulletNum;
    protected int _accuracy;
    protected string _weaponName;
    protected Vector3 _bulletPos;

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
        _bulletParticle = p.GetComponent<ParticleSystem>();
        particleScr = p.GetComponent<particleScr>();
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
