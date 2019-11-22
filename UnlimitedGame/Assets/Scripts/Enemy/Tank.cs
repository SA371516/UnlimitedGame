using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : BaseEnemy
{
    GameObject BulletPos;   //爆発エリア
    GameObject _Explosion;  //爆発パーティクル
    GameObject _head;       //砲塔
    float _attackTime;
    protected override void Start()
    {
        _status = Resources.Load("Tank") as EnemyStatus;
        base.Start();
        _head = transform.GetChild(0).gameObject;
        BulletPos = Resources.Load("Prefabs/BulletErea") as GameObject;
        _Explosion = Resources.Load("Prefabs/Explosion") as GameObject;
    }

    protected override void Update()
    {
        if (_stop) return;
        _head.transform.LookAt(_target);
        _head.transform.Rotate(new Vector3(0, -180, 0));                                                    //砲塔をプレイヤーに向かせる

        _attackTime += Time.deltaTime;
        if (_attackTime > 10f)
        {
            _attackTime = 0f;
            StartCoroutine(Shotting());
        }

        base.Update();                                                                                                        //死亡処理
    }

    IEnumerator Shotting()
    {                                                                                                                                   //プレイヤーの半径5メートル以内にランダムで着弾
        float _xPosition = Random.Range(_target.position.x - 5f, _target.position.x + 5f);
        float _zPosition = Random.Range(_target.position.z - 5f, _target.position.z + 5f);
        Vector3 vec = new Vector3(_xPosition, 0, _zPosition);
        yield return new WaitForFixedUpdate();
        float time = 0f;
        GameObject obj = _manager.ObjectInctance(BulletPos, vec,gameObject);          //着弾範囲を表示
        while (time < 6f)                                                                                                      //着弾まで待つ
        {
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(obj);
        _manager.ObjectInctance(_Explosion,vec);                                                           //爆発エフェクト

        int dis = (int)Vector3.Distance(_target.position, vec);                                             //距離を測る
        if (dis <= 5f&&dis>=0)
        {
            int _giveATK = ATK - (ATK / 5 * dis);                                                                    //距離に応じてダメージを変えるため
            BasePlayer player = _target.gameObject.GetComponent<BasePlayer>();
            player.DamageMove(vec, _giveATK);
        }
        yield return new WaitForFixedUpdate();
    }

    protected override void DeadFunction()
    {
        _manager.ObjectInctance(_Explosion, gameObject.transform.position);                //爆発エフェクト
        base.DeadFunction();
    }
}
