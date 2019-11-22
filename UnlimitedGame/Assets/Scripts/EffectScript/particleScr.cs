using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleScr : MonoBehaviour
{
    protected ParticleSystem particleSystem;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public abstract void PlayParticle();
    public abstract void StopParticle();
}
