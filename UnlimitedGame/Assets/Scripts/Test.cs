using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    ParticleScr particleScr;

    void Start()
    {
        particleScr = GetComponent<ParticleScr>();
    }

    void Update()
    {
        //次の弾を撃つラグ
        if (Input.GetMouseButtonDown(0))
        {
            particleScr.PlayParticle();
        }
     
    }
}
