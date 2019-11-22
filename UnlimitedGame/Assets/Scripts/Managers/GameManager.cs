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
    [SerializeField,Header("武器のリス位置")]
    Transform[] _Wp;
    [SerializeField, Header("敵オブジェクト")]
    List<GameObject> _enemys = new List<GameObject>();
    [SerializeField,Header("武器オブジェクト")]
    List<GameObject> _weapons = new List<GameObject>();

    List<Transform> _lis = new List<Transform>();
    List<GameObject> _weaponControll = new List<GameObject>();
    List<GameObject> _nowEnemy = new List<GameObject>();
    float _time = 0;
    float _leveltime;
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
    int _score;
    public int GetSetScore
    {
        get { return _score; }
        set { _score = value; }
    }
    public int _enemyStatusChange;

    void Start()      
    {
        _stop = false;
        _score = 0;
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
        if (_leveltime > _gameTime)
        {
            _gameTime += 30;
            _enemyStatusChange += 2;
        }
        _time += Time.deltaTime;
                                                                                                                        //オブジェクト生成
        if (_time > _inctanceTime)
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
            }                                                 //敵管理
            _nowEnemy = _nowEnemy.Where(j => j != null).ToList();                  //Null以外を挿入

            int _Enemycount = _nowEnemy.Count;
            if (_Enemycount > 10) return;

            int _id = Random.Range(0,_enemys.Count);

            if (_nowEnemy.Find(Item=>Item.name=="Tank"))                                                         //戦車は一度に一体しか出現しない
            {
                int _tankid = _enemys.IndexOf(_enemys.Find(item => item.name == "Tank"));
                if (_id != _tankid)
                {
                    GameObject obj = ObjectInctance(_enemys[_id], _lis[Random.Range(0, 4)].position);                        //敵生成
                    _time = 0f;
                    _nowEnemy.Add(obj);
                    switch (_id)
                    {
                        case 0:
                            obj.AddComponent<Zonbi>();
                            break;
                    }
                }
            }
            else　                                                                                   //普通に出現処理をする
            {
                GameObject obj = ObjectInctance(_enemys[_id], _lis[Random.Range(0, 4)].position);                        //敵生成
                _time = 0f;
                _nowEnemy.Add(obj);
                switch (_id)
                {
                    case 0:
                        obj.AddComponent<Zonbi>();
                        break;
                    case 1:
                        obj.AddComponent<Tank>();
                        break;
                }
            }
        }
    }

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
    public void GameOver(Vector3 vec)
    {
        _stop = true;
        Stop(_stop);
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(_camera.GameOver(vec,3f, _score));
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
}
