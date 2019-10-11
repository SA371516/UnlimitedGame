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
}
