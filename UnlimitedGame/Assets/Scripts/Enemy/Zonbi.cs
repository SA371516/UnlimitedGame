using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Zonbi : BaseEnemy
{
    NavMeshAgent _nav;
    Transform _target;
    enum AnimTrigger
    {
        Walk,Attack,Stop
    }

    protected override void Start()
    {
        _status = Resources.Load("Zonbi") as EnemyStatus;
        base.Start();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _Speed = 0.1f;
        _moveFlag = (int)MoveStatus.MoveUp;
        //StartCoroutine(AnimatorController(AnimTrigger.Walk));
        _animator.SetTrigger(AnimTrigger.Walk.ToString());
        _nav = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("Player").transform;
        _addScore = _status.Score;
        _HP = _status.HP + _HPchange;
        gameObject.name = _status.Name;
    }

    protected override void Update()
    {
        if (_stop) _moveFlag = (int)MoveStatus.Stop;
        else if (_Speed >= 4) { _moveFlag = (int)MoveStatus.MoveDown; /*StartCoroutine(AnimatorController(AnimTrigger.Walk))*/;}
        else if (_Speed <= 0) { _moveFlag = (int)MoveStatus.MoveUp; /*StartCoroutine(AnimatorController(AnimTrigger.Walk))*/;}
        switch (_moveFlag)
        {
            case 0:
                _Speed = 0f;
                break;
            case 1:
                _Speed += 0.1f;
                break;
            case 2:
                _Speed -= 0.1f;
                break;
        }
        //歩かせる処理
        _nav.speed = _Speed;
        _animator.speed = _Speed;
        _nav.destination = transform.position + (_target.position - transform.position);
        base.Update();//死亡処理
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.name == "HitBox")
        {
            _Speed = 0f;
            _stop = true;
            //StartCoroutine(AnimatorController(AnimTrigger.Attack,true));
            _animator.SetTrigger(AnimTrigger.Attack.ToString());
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    IEnumerator AnimatorController(AnimTrigger trigger,bool fast=false)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName(trigger.ToString()))
        {
            if (fast) break;//終わるまで待たない
            yield return new WaitForFixedUpdate();
        }
        _animator.SetTrigger(trigger.ToString());
        while (stateInfo.IsName(trigger.ToString()))
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
