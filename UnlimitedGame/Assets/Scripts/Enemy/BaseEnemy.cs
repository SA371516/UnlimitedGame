using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            var v = other.gameObject.transform.parent.GetComponent<BasePlayer>();
            v.DamageMove(transform.position);
            _Speed = 0f;
            _moveFlag = 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            _moveFlag = 0;
        }
    }

}
