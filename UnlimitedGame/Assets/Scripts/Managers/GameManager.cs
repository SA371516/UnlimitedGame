﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public  enum Weapons
{
    SR,AR
}
public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform[] _Wp;
    [SerializeField]
    GameObject[] _enemys;//敵の種類
    [SerializeField]
    List<GameObject> _weapons = new List<GameObject>();

    List<Transform> _lis = new List<Transform>();
    List<GameObject> _weaponControll = new List<GameObject>();
    List<GameObject> _nowEnemy = new List<GameObject>();
    float _time = 0;
    float _inctanceTime = 2f;
    float _gameTime = 10f;
    public float GetTime
    {
        get { return _gameTime; }
    }
    UIManager _uiManager;
    BasePlayer _player;
    CameraMove _camera;
    bool _stop;
    public int _enemyHPChange;
    [SerializeField]
    int _score;
    public int GetSetScore
    {
        get { return _score; }
        set { _score = value; }
    }
    
    void Start()      
    {
        _stop = false;
        //=========マウス処理========
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //=========================
        _player = GameObject.Find("Player").GetComponent<BasePlayer>();
        _camera = _player.transform.GetChild(0).GetComponent<CameraMove>();
        _uiManager = GetComponent<UIManager>();
        foreach(Transform v in transform.GetChild(0).transform)
        {
            _lis.Add(v);
        }
        for(int i=0;i<_weapons.Count*2;++i)
        {
            Vector3 vec = new Vector3(Random.Range(_Wp[0].position.x, _Wp[1].position.x), 1, Random.Range(_Wp[0].position.z, _Wp[1].position.z));
            int _weaponsLength = i % _weapons.Count;
            _weaponControll.Add(ObjectInctance(_weapons[_weaponsLength], vec));
        }
    }

    // Update is called once per frame
    void Update()
    {
        _stop = Confug._confug.StatusInctance<bool>();
        if (_stop)//操作中はゲームを止める
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _stop = true;
            _player._stop = true;
            _camera._stop = true;
            _uiManager._stop = true;
            foreach(var v in _nowEnemy)
            {
                BaseEnemy _e = v.GetComponent<BaseEnemy>();
                if (_e != null)
                {
                    v.GetComponent<BaseEnemy>()._stop = true;
                }
            }
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _stop = false;
            _player._stop = false;
            _camera._stop = false;
            _uiManager._stop = false;
            foreach (var v in _nowEnemy)
            {
                BaseEnemy _e = v.GetComponent<BaseEnemy>();
                if (_e != null)
                {
                    v.GetComponent<BaseEnemy>()._stop = false;
                }
            }
        }
        if (_stop) return;

        _time += Time.deltaTime;
        if (_time > _gameTime)
        {
            Debug.Log("レベルアップ");
            _gameTime *= 2;
            _enemyHPChange += 2;
        }

        //オブジェクト生成
        if (_time > _inctanceTime)
        {
            _weaponControll = _weaponControll.Where(j => j != null).ToList(); //Null以外を挿入
            //武器生成
            if(_player._weaponName.Count>0)
            {
                foreach(var v in _player._weaponName)
                {
                    Vector3 vec = new Vector3(Random.Range(_Wp[0].position.x, _Wp[1].position.x), 1, Random.Range(_Wp[0].position.z, _Wp[1].position.z));
                    switch (v)
                    {
                        case Weapons.AR:
                            _weaponControll.Add(ObjectInctance( _weapons.Find(item=>item.name=="AR"), vec));//武器を探して出現させる
                            //_weaponControll.Add(ObjectInctance( _weapons.Where(item => item.name == "AR"), _weponlis[Random.Range(0, 4)].position));//
                            break;
                        case Weapons.SR:
                            _weaponControll.Add(ObjectInctance(_weapons.Find(item => item.name == "SR"), vec));//武器を探して出現させる
                            break;
                        default:
                            Debug.Log("異なる値が挿入されています");
                            break;
                    }
                }
                _player._weaponName.Clear();
            }
            //敵管理
            _nowEnemy = _nowEnemy.Where(j => j != null).ToList(); //Null以外を挿入
            int _Enemycount = _nowEnemy.Count;
            if (_Enemycount > 10) return;

            //敵生成
            _time = 0f;
            var obj = ObjectInctance(_enemys[0], _lis[Random.Range(0, 4)].position);
            _nowEnemy.Add(obj);
            obj.AddComponent<Zonbi>();
        }
    }

    public GameObject ObjectInctance(GameObject obj,Vector3 pos)
    {
        return Instantiate(obj, pos, Quaternion.identity);
    }
    // プレイヤーのHPが0になった時呼ばれる
    public void GameOver(Vector3 vec)
    {
        _uiManager._stop = true;
        _player._stop = true;
        _stop = true;
        _camera._stop = true;
        StartCoroutine(_camera.GameOver(vec,3f));
    }
}
