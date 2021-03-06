﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLight : ParticleScr
{
    WFX_LightFlicker _light;

    protected override void Start()
    {
        base.Start();
        _light = transform.GetChild(1).GetComponent<WFX_LightFlicker>();
        StopParticle();
    }
    public override void PlayParticle()
    {
        if (!_particleSystem.isPlaying) {
            _particleSystem.time = 0f;
            _particleSystem.Play();
        }
        _light._ok = true;
    }
    private void Update()
    {
        if (_light._ok)
        {
            StopParticle();
        }
    }
    //ARやSRで止める時間を変える必要がある
    public override void StopParticle()
    {
        if (_particleSystem.time > _stopTime)
        {
            _particleSystem.Stop();
            _light._ok = false;
        }
    }
}
