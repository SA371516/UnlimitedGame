using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Zonbi : BaseEnemy
{
    NavMeshAgent _nav;
    Transform _target;
    float _Speed;
    int _moveFlag;
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
        _nav.destination = _target.position - new Vector3(1, 0, 1);
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Hitbox")
        {
            _Speed = 0f;
            _moveFlag = 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Hitbox")
        {
            _moveFlag = 0;
        }
    }
}
