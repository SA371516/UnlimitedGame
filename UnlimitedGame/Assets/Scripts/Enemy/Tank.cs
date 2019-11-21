using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : BaseEnemy
{
    Transform _target;
    GameObject _head;
    float _attackTime;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _target = GameObject.Find("Player").transform;
        _head = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    { 
        _head.transform.LookAt(_target);//砲塔をプレイヤーに向かせる

        _attackTime += Time.deltaTime;
        if (_attackTime > 30f)
        {
            _attackTime = 0f;
            Shotting();
        }

        base.Update();//死亡処理
    }

    void Shotting()
    { 
        //プレイヤーの半径5メートル以内にランダムで着弾
        float _xPosition = Random.Range(_target.position.x - 5f, _target.position.x + 5f);
        float _zPosition = Random.Range(_target.position.z - 5f, _target.position.z + 5f);

        //着弾まで待つ

        if (_target.position.x == _xPosition && _target.position.z == _zPosition)
        {
            
        }


    }
}
