using System.Collections;
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

    List<Transform> _lis = new List<Transform>();                               //敵のリス位置
    List<GameObject> _weaponControll = new List<GameObject>();  //武器の出現状況
    List<GameObject> _nowEnemy = new List<GameObject>();          //敵の出現状況
    float _time = 0;
    float _leveltime;                                                                                   //レベルの管理
    float _inctanceTime = 2f;                                                                   //リスタイム
    float _gameTime = 10f;                                                                      //次のレベルまでの
    public float GetTime  {    get { return _gameTime; }   }
    bool _stop;
    int _score;
    int _tankCount;//敵のタンクを撃破した数
    public int _shotNum;//撃った数
    public int _hitNum;//当たった数
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
    }
    void Update()
    {
        _stop = Confug._confug.GetConfugStatus<bool>(_stop);
        if (_stop)                                                                                                  //操作中はゲームを止める
        {
            Stop(_stop);
            Cursor.lockState = CursorLockMode.None;
            foreach (var v in _nowEnemy)
            {
                if (v != null)
                {
                    BaseEnemy _e = v.GetComponent<BaseEnemy>();
                    v.GetComponent<BaseEnemy>()._stop = _stop;
                }
            }
            return;
        }
        else
        {
            Stop(_stop);
            Cursor.lockState = CursorLockMode.Locked;
            foreach (var v in _nowEnemy)
            {
                if (v != null)
                {
                    BaseEnemy _e = v.GetComponent<BaseEnemy>();
                    v.GetComponent<BaseEnemy>()._stop = false;
                }
            }
        }

        _leveltime += Time.deltaTime;
        if (_leveltime > _gameTime)                                 //レベル処理
        {
            _gameTime += 30;
            status.AddHP += 2;
            status.AddATK += 1;
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
            }                                                 //敵管理
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
    //================オブジェクト生成関数(マネージャがすべてのインスタンスを行うため)=================
    public GameObject ObjectInctance(GameObject o,Vector3 pos,GameObject parent=null)
    {
        GameObject _obj = Instantiate(o, pos, Quaternion.identity);
        if (parent != null)//このままだと武器以外の対応が出来ない
        {
            _obj.transform.parent = parent.transform;
            _obj.transform.rotation = Quaternion.identity;
            //Vector3 vec = o.transform.position;
            //_obj.transform.position += vec;
            //_obj.transform.rotation = parent.transform.rotation;
            //_obj.transform.rotation = Quaternion.Euler(0, -90f,0);
        }
        return _obj;
    }
    // プレイヤーのHPが0になった時呼ばれる
    public void GameOver(Vector3 vec,bool once)
    {
        float _headHitProbability = 0;//敵に当たった確率
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
        StartCoroutine(_camera.GameOver(vec,3f, _score,once));
    }
}
