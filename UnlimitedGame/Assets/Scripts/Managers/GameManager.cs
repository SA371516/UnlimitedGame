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
    public struct AddEnemyStatus
    {
        public int AddHP;
        public int AddATK;
    }
    [SerializeField,Header("武器のリス位置")]
    Transform[] _Wp;
    [SerializeField, Header("敵オブジェクト")]
    List<GameObject> _enemys = new List<GameObject>();
    [SerializeField,Header("武器オブジェクト")]
    List<GameObject> _weapons = new List<GameObject>();
    [SerializeField]
    GameObject Goal;

    List<Transform> _lis = new List<Transform>();                               //敵のリス位置
    List<GameObject> _weaponControll = new List<GameObject>();  //武器の出現状況
    List<GameObject> _nowEnemy = new List<GameObject>();          //敵の出現状況
    float _time = 0;
    float _leveltime;                                                                                   //レベルの管理
    float _inctanceTime = 2f;                                                                   //リスタイム
    float _gameTime = 10f;                                                                      //次のレベルまでの
    bool _stop;
    bool once;
    int _score;
    int _tankCount;//敵のタンクを撃破した数

    public float _shotNum;//撃った数
    public float _hitNum;//当たった数
    public float _exitTime;//一定時間とどまる必要がある
    public bool _goalChack;
    public float GetTime { get { return _gameTime; } }
    public int GetSetScore  {  get { return _score; } set { _score = value; }  }
    public int GetSetTankCount  {  get { return _tankCount; } set { _tankCount = value; }  }

    UIManager _uiManager;
    BasePlayer _player;
    CameraMove _camera;

    public AddEnemyStatus status = new AddEnemyStatus();

    void Start()      
    {
        if (PlayerData._Data._debug)
        {
            var v = PlayerData._Data.CreateUserData("Debug", "FF");
            PlayerData._Data._playerStatus = v;
        }
        _stop = false;
        _score = 0;
        //=========マウス処理========
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //=========================
        _player = GameObject.Find("Player").GetComponent<BasePlayer>();
        _camera = _player.transform.GetChild(0).GetComponent<CameraMove>();
        _uiManager = GetComponent<UIManager>();
        foreach(Transform v in transform.GetChild(0).transform)                                 //敵のリス位置初期化
        {
            _lis.Add(v);
        }
        //武器出現
        for(int i=0;i<_weapons.Count*2;++i)                                                                 //武器の出現（二つずつ）
        {
            Vector3 vec =
                new Vector3(
                    Random.Range(_Wp[0].position.x, _Wp[1].position.x),
                    1,
                    Random.Range(_Wp[0].position.z, _Wp[1].position.z)
                        );
            int _weaponsID = i % _weapons.Count;                                                    //二つ出すため
            _weaponControll.Add(ObjectInctance(_weapons[_weaponsID], vec));
        }
        //ゴール位置初期化
        Vector3 _goalpos = new Vector3(0, 0, 0);
        if ( Random.Range(0, 2) == 0)
        {
            _goalpos.z = Random.Range(_Wp[0].position.z, _Wp[1].position.z);
            if (Random.Range(0, 2) == 0)
                _goalpos.x = _Wp[0].position.x;
            else
                _goalpos.x = _Wp[1].position.x;
        }
        else
        {
            _goalpos.x = Random.Range(_Wp[0].position.x, _Wp[1].position.x);
            if (Random.Range(0, 2) == 0)
                _goalpos.z = _Wp[0].position.z;
            else
                _goalpos.z = _Wp[1].position.z;
        }
        ObjectInctance(Goal, _goalpos);
        SoundManager._soundManager.GameSetInit();
        once = true;
    }
    void Update()
    {
        //コンフィグ画面表示時に、敵を止める処理
        _stop = Confug._confug.GetConfugStatus<bool>(_stop);
        Stop(_stop);
        foreach (var v in _nowEnemy)
        {
            if (v != null)
            {
                BaseEnemy _e = v.GetComponent<BaseEnemy>();
                v.GetComponent<BaseEnemy>()._stop = _stop;
            }
        }
        if (_stop)                                                                                                  //操作中はゲームを止める
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else
            Cursor.lockState = CursorLockMode.Locked;

        _leveltime += Time.deltaTime;
        if (_leveltime > _gameTime)                                 //レベル処理
        {
            _gameTime += 30;
            status.AddHP += 2;
            status.AddATK += 1;
        }
        //脱出処理
        if (_goalChack)
        {
            if (_exitTime >= 20f)
            {
                GameClear();
            }
            else { _exitTime += Time.deltaTime; }
        }

        _time += Time.deltaTime;
        if (_time > _inctanceTime) //オブジェクト生成
        {
            _weaponControll = _weaponControll.Where(j => j != null).ToList();    //Null以外を挿入
            if(_player._weaponName.Count>0)
            {
                foreach(var v in _player._weaponName)
                {
                    Vector3 vec = new Vector3(Random.Range(_Wp[0].position.x, _Wp[1].position.x), 1, Random.Range(_Wp[0].position.z, _Wp[1].position.z));
                    switch (v)
                    {
                        case Weapons.AR:
                            _weaponControll.Add(ObjectInctance( _weapons.Find(item=>item.name=="AR"), vec));//武器を探して出現させる
                            break;
                        case Weapons.SR:
                            _weaponControll.Add(ObjectInctance(_weapons.Find(item => item.name == "SR"), vec));//武器を探して出現させる
                            break;
                        default:
                            Debug.Log("異なるオブジェクトが挿入されています");
                            break;
                    }
                }
                _player._weaponName.Clear();
            }      
            //敵管理
            _nowEnemy = _nowEnemy.Where(j => j != null).ToList();                  //Null以外を挿入
            int _Enemycount = _nowEnemy.Count;
            if (_Enemycount > 10) return;
            int _id = Random.Range(0,_enemys.Count);
            //=============戦車は一度に一体しか出現しない================//
            if (_nowEnemy.Find(Item=>Item.name=="Tank"))
            {
                int _tankid = _enemys.IndexOf(_enemys.Find(item => item.name == "Tank"));
                if (_id != _tankid)
                {
                    GameObject obj = ObjectInctance(_enemys[_id], _lis[Random.Range(0, 4)].position);                        //敵生成
                    _time = 0f;
                    _nowEnemy.Add(obj);
                }
            }
            else//======普通に出現処理をする========//
            {
                GameObject obj = ObjectInctance(_enemys[_id], _lis[Random.Range(0, 4)].position);                        //敵生成
                _time = 0f;
                _nowEnemy.Add(obj);
            }
        }
    }
    void Stop(bool B)
    {
        Cursor.visible = B;
        Cursor.lockState = CursorLockMode.None;
        _stop = B;
        _player._stop = B;
        _camera._stop = B;
        _uiManager._stop = B;
    }
    //脱出した時に呼ばれる
    void GameClear()
    {
        float _headHitProbability = 0f;
        if (_shotNum > 30)
        {
            _headHitProbability = _hitNum / _shotNum;
        }
        _stop = true;
        Stop(_stop);
        Cursor.lockState = CursorLockMode.None;
        PlayerData._Data._getPoint = _score;
        PlayerData._Data._tankCount = this._tankCount;
        PlayerData._Data._probability = _headHitProbability;
        if (once)
        {
            once = false;
            StartCoroutine(_uiManager.BlackOut());
        }
    }
    //================オブジェクト生成関数(マネージャがすべてのインスタンスを行うため)=================
    public GameObject ObjectInctance(GameObject o,Vector3 pos,GameObject parent=null)
    {
        GameObject _obj = Instantiate(o, pos, Quaternion.identity);
        if (parent != null)//このままだと武器以外の対応が出来ない
        {
            _obj.transform.parent = parent.transform;
            _obj.transform.rotation = Quaternion.identity;
        }
        return _obj;
    }
    // プレイヤーのHPが0になった時呼ばれる
    //死亡したらスコアが入らない
    public void GameOver(Vector3 vec)
    {
        _stop = true;
        Stop(_stop);
        Cursor.lockState = CursorLockMode.None;
        PlayerData._Data._getPoint = 0;
        PlayerData._Data._tankCount = 0;
        PlayerData._Data._probability = 0;
        StartCoroutine(_camera.GameOver(vec, 3f));
    }

}
