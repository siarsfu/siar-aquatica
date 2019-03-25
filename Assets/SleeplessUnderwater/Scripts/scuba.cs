using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scuba : MonoBehaviour
{
	[Header("Scuba Settings")]
    [Tooltip("This allows you to enable or disable scuba audio.")]
	public bool AudioEnabled;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (AudioEnabled == true)
		{
			// If Particle System is starting to blow the bubbles out then start the sound
			if (GetComponent<ParticleSystem>().time <= .1f)
			{
				GetComponent<AudioSource>().Play();
			}
		}
    }
}
