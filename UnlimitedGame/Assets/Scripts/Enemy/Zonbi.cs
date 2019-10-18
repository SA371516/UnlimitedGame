using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Zonbi : BaseEnemy
{
    NavMeshAgent _nav;
    Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _Speed = 0.1f;
        _moveFlag = 0;
        _nav = GetComponent<NavMeshAgent>();
        _target = GameObject.Find("Player").transform;
    }

    protected override void Update()
    {
        switch (_moveFlag)
        {
            case 0:
                _Speed += 0.1f;
                if (_Speed > 4) _moveFlag = 1;
                break;
            case 1:
                _Speed -= 0.1f;
                if (_Speed < 0) _moveFlag = 0;
                break;
            case 2:
                _Speed = 0f;
                break;
        }
        _nav.speed = _Speed;
        _nav.destination = transform.position + (_target.position - transform.position);
        base.Update();
    }
}
