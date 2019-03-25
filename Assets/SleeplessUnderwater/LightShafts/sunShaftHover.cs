using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunShaftHover : MonoBehaviour {

    private Vector3 hoverPosition;
	// Use this for initialization
	void Start () {
        InvokeRepeating("HoverSunShafts", 5f, 5f);  //1s delay, repeat every 1s
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void HoverSunShafts()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, 78.8f, Camera.main.transform.position.z);
    }
}
