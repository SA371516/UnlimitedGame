using UnityEngine;
using System.Collections;

/**
 *	Rapidly sets a light on/off.
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
	public float time = 0.05f;
    public bool _ok;
	private float timer;
	
	void Start ()
	{
        _ok = false;
		timer = time;
		StartCoroutine("Flicker");
	}
	
	IEnumerator Flicker()
	{
		while(_ok)
		{
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
			do
			{
				timer -= Time.deltaTime;
				yield return null;
			}
			while(timer > 0);
			timer = time;
		}
	}
}
