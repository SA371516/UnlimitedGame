using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Zonbi : BaseEnemy
{
    NavMeshAgent _nav;
    protected override void Start()
    {
        _status = Resources.Load("Zonbi") as EnemyStatus;
        base.Start();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _Speed = 0.1f;
        _moveFlag = (int)MoveStatus.MoveUp;
        StartCoroutine(AnimatorController(AnimTrigger.Walk));
        _nav = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        if (_stop && _moveFlag != (int)MoveStatus.Attack) _moveFlag = (int)MoveStatus.Stop;
        else if (_moveFlag == (int)MoveStatus.Attack) _moveFlag = (int)MoveStatus.Attack;
        else if (_Speed >= 4) { _moveFlag = (int)MoveStatus.MoveDown; }
        else if (_Speed <= 0) { _moveFlag = (int)MoveStatus.MoveUp; }
        switch (_moveFlag)
        {
            case 0:
                _Speed = 0f;
                _nav.speed = _Speed;
                _animator.speed = _Speed;
                break;
            case 1:
                _Speed += 0.1f;
                _nav.speed = _Speed;
                _animator.speed = _Speed;
                StartCoroutine(AnimatorController(AnimTrigger.Walk));
                break;
            case 2:
                _Speed -= 0.1f;
                _nav.speed = _Speed;
                if (_Speed < 0) _animator.speed = 0f;
                else _animator.speed = _Speed;
                StartCoroutine(AnimatorController(AnimTrigger.Walk));
                break;
            case 3://攻撃時はアニメーションのスピードを止めないため
                _Speed = 0;
                _nav.speed = _Speed;
                _animator.speed = 1f;
                break;
        }
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
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    protected override void DeadFunction()
    {
        StartCoroutine(AnimatorController(AnimTrigger.Dead));
        base.DeadFunction();
    }
}
