using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPlayer : MonoBehaviour
{

    public GameObject Rigidbody;


    void OnTriggerEnter(Collider other)
    {
        Rigidbody.GetComponent<Rigidbody>().useGravity = false;
        Rigidbody.GetComponent<Rigidbody>().isKinematic = true;

    }


}

