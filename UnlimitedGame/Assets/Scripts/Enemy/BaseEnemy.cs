using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    protected enum MoveStatus
    {
        Stop = 0,
        MoveUp = 1,
        MoveDown = 2,
        Attack = 3
    }
    protected enum AnimTrigger
    {
        Walk, Attack, Stop,Dead
    }

    protected Rigidbody rig;
    protected int _HP;
    public int GetSetHP
    {
        get { return _HP; }
        set { _HP = value; }
    }
    protected int ATK = 0;
    protected float _Speed;
    protected int _moveFlag;
    protected GameManager _manager;
    protected int _addScore;
    protected EnemyStatus _status;
    protected Animator _animator;
    protected Transform _target;


    public bool _stop;
    public bool _debug;

    protected virtual void Start()
    {
        if (_debug)
        {
            _target = GameObject.Find("Player").transform;
            GetSetHP += _status.HP;
            ATK = _status.ATK;
            _addScore = _status.Score;
            gameObject.name = _status.Name;
        }
        else
        {
            _target = GameObject.Find("Player").transform;
            _manager = GameObject.Find("Manager").GetComponent<GameManager>();
            int _addHP = _manager.status.AddHP;
            int _addATK = _manager.status.AddHP;
            GetSetHP += _addHP + _status.HP;
            ATK = _status.ATK + _addATK;
            _addScore = _status.Score;
            gameObject.name = _status.Name;
        }
    }
    protected virtual void Update()
    {
        if (_HP >= 0) return;
        DeadFunction();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HitBox"&&gameObject.name!="Tank")
        {
            var v = other.gameObject.transform.parent.GetComponent<BasePlayer>();
            _moveFlag = (int)MoveStatus.Attack;
            StartCoroutine(AnimatorController(AnimTrigger.Attack,v));
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HitBox"&& gameObject.name != "Tank")
        {
            _stop = false;
            _moveFlag = (int)MoveStatus.MoveUp;
        }
    }

    protected IEnumerator AnimatorController(AnimTrigger trigger, BasePlayer P = null)
    {
        //=======デバッグ用=====
        AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        // 再生中のクリップ名
        string clipName = "名前：" + clipInfo[0].clip.name;
        Debug.Log(clipName);
        //====================

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(trigger.ToString()))//同じものが再生されていたら再生しない
        {
            _animator.SetTrigger(trigger.ToString());
            yield return new WaitForFixedUpdate();
        }

        float _animTime = clipInfo[0].clip.length / 2;        //攻撃アニメーションの真ん中でノックバックさせるため
        float _time = Time.time;
        while (_time + _animTime >= Time.time && P != null)
        {
            yield return new WaitForFixedUpdate();
        }
        if (P != null) P.DamageMove(transform.position,ATK);
    }

    protected virtual void DeadFunction()
    {
        Destroy(gameObject);
        _manager.GetSetScore += _addScore;
    }
}
