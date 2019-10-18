using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    protected Rigidbody rig;
    private int _HP;
    public int GetSetHP
    {
        get { return _HP; }
        set { _HP = value; }
    }
    protected float _Speed;
    protected int _moveFlag;

    protected BaseEnemy()
    {
        _HP = 5;
    }
   protected  virtual void  Update()
    {
        if (_HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            var v = other.gameObject.transform.parent.GetComponent<BasePlayer>();
            v.GetSetHP--;
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
