using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ChangePostProcess : MonoBehaviour
{

    public PostProcessingProfile normal, fx;
    private PostProcessingBehaviour camImageFx;

    // Start is called before the first frame update
    void Start()
    {
        camImageFx = FindObjectOfType<PostProcessingBehaviour>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLayer"))
        {
            camImageFx.profile = fx;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLayer"))
        {
            camImageFx.profile = normal;
        }
    }

}
