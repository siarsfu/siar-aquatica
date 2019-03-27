using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBlaster : MonoBehaviour
{

    //public Vector3 handLeftPos;
    //private Vector3 handRightPos;
    private GameObject rh, lh;

    private void Awake()
    {
        rh = GameObject.FindGameObjectWithTag("RightHand");
        lh = GameObject.FindGameObjectWithTag("LeftHand");
    }

    // Start is called before the first frame update
    void Start()
    {
        //handLeftPos = GameObject.FindGameObjectWithTag("LeftHand").transform.position;
        //handRightPos = GameObject.FindGameObjectWithTag("RightHand").transform.position;

        
    }

    // Update is called once per frame
    void Update()
    {

        //null
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
        {
            Debug.Log(OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote));
            Debug.Log(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote));
        }


        





    }

    private void FixedUpdate()
    {
        // Vector3 rotRight = rh.GetComponent<Transform>().transform.rotation.eulerAngles;
        Vector3 rotRight = rh.GetComponent<Transform>().transform.position;
        Vector3 rotLeft = lh.GetComponent<Transform>().transform.position;
        //Debug.Log("Controller Right Rotation:" + rotRight);

        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.0f)
        {
            this.GetComponent<Rigidbody>().AddForce(lh.transform.forward * 5000);


            //this.GetComponent<Rigidbody>().AddForce(-rotRight.x, -rotRight.y, -rotRight.z, ForceMode.Impulse);
            //this.GetComponent<Rigidbody>().AddForce(this.transform.forward * posRight.z *100);
        
        }

        /*
        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.0f)
        {
            this.GetComponent<Rigidbody>().AddForce(rh.transform.forward * 10000);

            //this.GetComponent<Rigidbody>().AddForce(-rotRight.x, -rotRight.y, -rotRight.z, ForceMode.Impulse);
            //this.GetComponent<Rigidbody>().AddForce(this.transform.forward * posRight.z *100);
          
        }

    */

    }
}
