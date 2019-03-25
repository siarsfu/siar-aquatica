using UnityEngine;
using System.Collections;
public class AnimatedProjector_Movement : MonoBehaviour { 
	public GameObject player;
	public GameObject WaterProjector;
	void Start()
	{
        // This script simply checks for the players position compared to the position of the projector.
        // If the player has moved 30m or more away from projector, move the projector to the player on X and Z, leaving the Y high up.
        // You could make it more than 30m if you notice the refresh occuring too often.
    }

    void Update ()
	{
		Vector3 my3dPos = player.transform.position;
		my3dPos.y = 95.0f;
		Vector3 WaterProjectorPos = transform.position;
		WaterProjectorPos.y = 95.0f;
		var distanceToTarget = Vector3.Distance(my3dPos, WaterProjectorPos);
            if (distanceToTarget >= 30.0f)
            {
                transform.position = my3dPos;
            }
       
	}
} 
