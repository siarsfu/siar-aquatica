using UnityEngine;
using System.Collections;
public class CrouchHeight : MonoBehaviour
{ 
    private CharacterController charController;
    private Transform tr;
    private float charHeight; //Initial height
   // FirstPersonController script;
    private void Start () 
    {
	    tr = transform;
	    charController = GetComponent<CharacterController>();
	    charHeight = charController.height;
	   // script = GetComponent<FirstPersonController>();
    }

    private void FixedUpdate () 
    {
	    float h = charHeight;
	    Vector3 tmpPosition = tr.position;
		    if (Input.GetKey("c"))
	    {
		    h = charHeight*0.5f;
	    }
	
	    float lastHeight = charController.height; //Stand up/crouch smoothly
	    charController.height = Mathf.Lerp(charController.height, h, 5*Time.deltaTime);
	    tmpPosition.y += (charController.height-lastHeight)/2; //Fix vertical position
	    tr.position = tmpPosition;
    }
}