using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnHover : MonoBehaviour {


    Material m_Material;
    Color32 s_Material;

    // Use this for initialization
    void Start () {
        //Fetch the Material from the Renderer of the GameObject
        m_Material = GetComponent<Renderer>().material;
        s_Material = GetComponent<Renderer>().material.color;


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        m_Material.color = Color.black;
        Debug.Log("OnObject");
    }

    private void OnTriggerExit(Collider other)
    {
        m_Material.color = s_Material;
        Debug.Log("Exit");
    }



}
