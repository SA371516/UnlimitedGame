using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Zonbi : BaseEnemy
{
    NavMeshAgent _nav;
    Transform _target;

    protected override void Start()
    {
        _status = Resources.Load("Zonbi") as EnemyStatus;
        base.Start();
        _Speed = 0.1f;
        _moveFlag = (int)MoveStatus.MoveSt;
        _nav = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("Player").transform;
        _addScore = _status.Score;
        _HP = _status.HP + _HPchange;
        gameObject.name = _status.Name;
    }

    protected override void Update()
    {
        if (_stop) _moveFlag = (int)MoveStatus.Stop; 
        else if (_Speed >= 4) _moveFlag = (int)MoveStatus.MoveDw;
        else if (_Speed <= 0) _moveFlag = (int)MoveStatus.MoveSt;
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
        _nav.speed = _Speed;
        _nav.destination = transform.position + (_target.position - transform.position);
        base.Update();
    }
}
