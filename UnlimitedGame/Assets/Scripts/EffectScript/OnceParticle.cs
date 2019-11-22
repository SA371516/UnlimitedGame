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
        Destroy(gameObject, particleSystem.duration);
    }

    public override void PlayParticle()
    {
        particleSystem.Play();
    }

    public override void StopParticle()
    {
    }
}
