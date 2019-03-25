using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnJellyBloom : MonoBehaviour
{


    public GameObject FalseUntilTriggered;
    public GameObject FalseUntilTriggered2;

    // Start is called before the first frame update
    void Start()
    {


        FalseUntilTriggered.SetActive(false);
        FalseUntilTriggered2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "OVRCameraRig")
        {
            FalseUntilTriggered.SetActive(true);
            FalseUntilTriggered2.SetActive(true);

        }
    }

}
