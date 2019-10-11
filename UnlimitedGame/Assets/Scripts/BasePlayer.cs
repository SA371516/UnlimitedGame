using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasePlayer : MonoBehaviour
{
    Rigidbody rig;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject Mycamera;
    BaseWeapon _haveWeapon;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rig = GetComponent<Rigidbody>();
        _haveWeapon = null;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //移動
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rig.velocity = new Vector3(h, 0, v) * speed;
        //攻撃
        if (_haveWeapon != null)
        {
            _haveWeapon.Update();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "SR":
                _haveWeapon = new SRWeapon();
                Destroy(other.gameObject);
                break;
        }
    }
}
