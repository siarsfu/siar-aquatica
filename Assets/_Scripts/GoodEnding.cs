using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEnding : MonoBehaviour
{

    public AudioSource audioData;

    public GameObject DestroyWP;

    public GameObject otherisFalse;



    // Start is called before the first frame update
    void Start()
    {

        AudioSource audio = GetComponent<AudioSource>();
        audioData = GetComponent<AudioSource>();


        DestroyWP = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        audioData.Play(0);
        //this.gameObject.SetActive(false);
        if (DestroyWP != null)
        {

            Destroy(DestroyWP);
            //gameObject.layer = 0;

            this.GetComponent<BoxCollider>().enabled = false;
            otherisFalse.SetActive(false);

          
        }
    }

}
