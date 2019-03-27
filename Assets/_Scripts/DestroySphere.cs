using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySphere : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           // Destroy(this.gameObject);
            Debug.Log("BLOW UP BITCH");
        }



    }
}


