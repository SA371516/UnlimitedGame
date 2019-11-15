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
        Attack = 3,
    }

    protected enum AnimTrigger
    {
        Walk, Attack, Stop
    }

    protected Rigidbody rig;
    protected int _HP;
    public int GetSetHP
    {
        get { return _HP; }
        set { _HP = value; }
    }
    protected int _HPchange;
    protected float _Speed;
    protected int _moveFlag;
    protected GameManager _manager;
    protected int _addScore;
    protected EnemyStatus _status;
    protected Animator _animator;

    public bool _stop;

    protected virtual void Start()
    {
        _manager = GameObject.Find("Manager").GetComponent<GameManager>();
        _HPchange = _manager._enemyHPChange;
    }
    protected virtual void Update()
    {
        if (_HP <= 0)
        {
            Destroy(gameObject);
            _manager.GetSetScore += _addScore;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            var v = other.gameObject.transform.parent.GetComponent<BasePlayer>();
            _moveFlag = (int)MoveStatus.Attack;
            StartCoroutine(AnimatorController(AnimTrigger.Attack,v));
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HitBox")
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

        //攻撃アニメーションの真ん中でノックバックさせるため
        float _animTime = clipInfo[0].clip.length / 2;
        float _time = Time.time;
        while (_time + _animTime >= Time.time && P != null)
        {
            Debug.Log(_animTime);
            yield return new WaitForFixedUpdate();
        }
        if (P != null) P.DamageMove(transform.position);
    }
}
