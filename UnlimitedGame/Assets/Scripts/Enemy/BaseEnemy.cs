using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    protected enum MoveStatus
    {
        MoveUp = 1,
        MoveDown = 2,
        Stop = 0
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
   protected  virtual void  Update()
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
            v.DamageMove(transform.position);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            _stop = false;
        }
    }

}
