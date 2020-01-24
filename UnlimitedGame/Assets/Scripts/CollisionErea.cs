using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionErea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            if(gameObject.name == "DeadErea")
            {
                BasePlayer p = other.transform.parent.GetComponent<BasePlayer>();
                p.GetSetHP = 0;
            }else if (gameObject.name == "ExitErea")
            {
                GameManager gm = GameObject.Find("Manager").GetComponent<GameManager>();
                gm._goalChack = true;
                gm._exitTime += Time.deltaTime;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HitBox")
        {
            if (gameObject.name == "ExitErea")
            {
                GameManager gm = GameObject.Find("Manager").GetComponent<GameManager>();
                gm._goalChack = false;
            }
        }
    }
}
