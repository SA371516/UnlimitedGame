﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLight : ParticleScr
{
    WFX_LightFlicker _light;
    float _partrcleTime;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _light = transform.GetChild(1).GetComponent<WFX_LightFlicker>();
        Debug.Log(gameObject.name);
        StopParticle();
    }
    public override void PlayParticle()
    {
        if (!_particleSystem.isPlaying) _particleSystem.Play();
        _light._ok = true;
        _partrcleTime = Time.time;
    }

    public override void StopParticle()
    {
        if (_partrcleTime <= Time.time - 0.05)
        {
            _particleSystem.Stop();
            _light._ok = false;
        }
    }
}
