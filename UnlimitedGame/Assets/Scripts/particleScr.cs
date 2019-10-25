using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleScr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, particleSystem.duration);
    }

}
