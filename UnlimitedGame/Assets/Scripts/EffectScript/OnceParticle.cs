using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnceParticle : ParticleScr
{

    // Start is called before the first frame update
    protected override void  Start()
    {
        base.Start();
        PlayParticle();
        Destroy(gameObject, _particleSystem.duration);
    }

    public override void PlayParticle()
    {
        _particleSystem.Play();
    }

    public override void StopParticle()
    {
    }
}
