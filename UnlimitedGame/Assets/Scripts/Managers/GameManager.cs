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
    List<Transform> _weponlis = new List<Transform>();
    List<Transform> _lis = new List<Transform>();
    List<GameObject> _weaponControll = new List<GameObject>();
    List<GameObject> _nowEnemy = new List<GameObject>();
    [SerializeField]
    GameObject[] _enemys;
    [SerializeField]
    List<GameObject> _weapons = new List<GameObject>();
    float _time;
    float _nextime;
    BasePlayer _player;
    
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<BasePlayer>();
        _time = 0;
        _nextime = 2f;
        foreach(Transform v in transform.GetChild(0).transform)
        {
            _lis.Add(v);
        }
        foreach (Transform v in transform.GetChild(1).transform)
        {
            _weponlis.Add(v);
        }
        foreach(var v in _weapons)
        {
            _weaponControll.Add(ObjectInctance(v, _weponlis[Random.Range(0, 4)].position));
        }
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time > _nextime)
        {
            _weaponControll = _weaponControll.Where(j => j != null).ToList(); //Null以外を挿入
            //武器生成
            if(_player._weaponName.Count>0)
            {
                foreach(var v in _player._weaponName)
                {
                    switch (v)
                    {
                        case Weapons.AR:
                            _weaponControll.Add(ObjectInctance( _weapons.Find(item=>item.name=="AR"), _weponlis[Random.Range(0, 4)].position));//武器を探して出現させる
                            //_weaponControll.Add(ObjectInctance( _weapons.Where(item => item.name == "AR"), _weponlis[Random.Range(0, 4)].position));//
                            break;
                        case Weapons.SR:
                            _weaponControll.Add(ObjectInctance(_weapons.Find(item => item.name == "SR"), _weponlis[Random.Range(0, 4)].position));//武器を探して出現させる
                            break;
                        default:
                            Debug.Log("異なる値が挿入されています");
                            break;
                    }
                }
                _player._weaponName.Clear();
            }
            _nowEnemy = _nowEnemy.Where(j => j != null).ToList(); //Null以外を挿入
            int _Enemycount = _nowEnemy.Count;
            if (_Enemycount > 10) return;
            //敵生成
            _time = 0f;
            _nowEnemy.Add(ObjectInctance(_enemys[0], _lis[Random.Range(0, 4)].position));
        }
    }

    public GameObject ObjectInctance(GameObject obj,Vector3 pos)
    {
        return Instantiate(obj, pos, Quaternion.identity);
    }
}
