using UnityEngine;
using System.Collections;
public class AnimatedProjector : MonoBehaviour { 
	public float fps = 30.0f; 
	public Texture2D[] frames;
	private int frameIndex;
	private Projector projector;

    // This script sets the Projector to an Animated Projector using the series of animated textures.
    // This effect will overlay the terrain, only when underwater, with a caustic effect.
    void Start()
	 {
		projector = GetComponent<Projector>();
		NextFrame();
		InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
	}
	 
	void NextFrame()
	{
		projector.material.SetTexture("_ShadowTex", frames[frameIndex]);
		frameIndex = (frameIndex + 1) % frames.Length;
	}
} 
