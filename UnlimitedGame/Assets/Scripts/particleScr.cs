using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleScr : MonoBehaviour
{
    protected ParticleSystem particleSystem;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public virtual void PlayParticle()
    {
    }
    public virtual void StopParticle()
    {
    }
}
