using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    // Use this for initialization



    float maxDistance = 1000;

    void Start () {


	}




    //check for objects with tag Destroyable, if raycast hits said object with collider, it will be destoryed. 
    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * 1000, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, maxDistance) && hit.transform.tag == "Destroyable")
        {
           // hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.black;
        }


        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.0f)
        {

            if (Physics.Raycast(transform.position, fwd, out hit, maxDistance) && hit.transform.tag == "Destroyable")
            {
                print("This object can be destroyed");
                Destroy(hit.transform.gameObject);

            }
        }
    }

 




}
