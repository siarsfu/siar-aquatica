using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayScriptAudio : MonoBehaviour {

    public AudioSource audioData;
    public GameObject DestroyWP;
    public GameObject SpecialWall;



    // Use this for initialization
    void Start () {

        AudioSource audio = GetComponent<AudioSource>();
        audioData = GetComponent<AudioSource>();
        Debug.Log("started");

        DestroyWP = transform.GetChild(0).gameObject;

    }
	
	// Update is called once per frame
	void Update () {
		
	}


     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "OVRCameraRig")
        {
            Debug.Log("Play Audio Level");
            audioData.Play(0);

            Debug.Log(other.gameObject);
         
            if (DestroyWP != null)
            {

                Destroy(DestroyWP);

                if (SpecialWall != null)
                {
                    Destroy(SpecialWall);
                }
                
            }

        }

       




        }



    }
