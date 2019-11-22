using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesErea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            BasePlayer p = other.transform.parent.GetComponent<BasePlayer>();
            p.GetSetHP = 0;
        }
    }
}
