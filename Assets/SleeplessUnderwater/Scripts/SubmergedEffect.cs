using UnityEngine;
public class SubmergedEffect : MonoBehaviour 
{
    [Header("Submerged Settings")]
    [Tooltip("This is the Water Surface from above looking down onto the water.")]
    public GameObject waterBody;
    [Tooltip("This is the Water Surface from underneath the water looking up at the surface.")]
    public GameObject waterBodyUnder;
    [Tooltip("This is the color of the water and visibility underwater.")]
    public Color underWaterColor;
    [Tooltip("This is the water visibility while underwater.")]
    public float underWaterVisiblity;
    [HideInInspector]
    public bool aboveWaterFogMode;
    [Tooltip("This is the color of the normal world fog while out of the water. This is saved from your default fog settings on save to use when swapping as you get in and out of the water.")]
    public Color aboveWaterColor;
    [Tooltip("This is the thickness of the normal world fog while out of the water. This is saved from your default fog settings on save to use when swapping as you get in and out of the water.")]
    public float aboveWaterVisiblity;
    [Tooltip("This is a link to the prefab to use for the water particles that move around while under water.")]
    public GameObject WaterParticles;
    private GameObject Player;
    [Tooltip("This is a link to the blob light projector so we can turn the underwater caustics on and off as you enter or exit the water.")]
    public Projector Caustics;

    [HideInInspector]
    public bool checkedIfAboveWater;
	private float waterHeight;
    private AudioSource m_AudioSource;
    private AudioSource m_JumpInWaterAudioSource;
    private AudioSource m_JumpOutOfWaterAudioSource;
	private ParticleSystem.EmissionModule waterParticles_emission; // Locate and cache the underwater particles effect and enable it
	private ParticleSystem.EmissionModule bubbles1Particles_emission; // Locate and cache the first bubble effect and enable it
	private ParticleSystem.EmissionModule bubbles2Particles_emission; // Locate and cache the second bubble effect and enable it
    private ParticleSystem.EmissionModule sunshaftsParticles_emission; // Locate and cache the sunshafts particles effect and enable it

    [Header("Scuba Settings")]
    [Tooltip("This allows you to enable or disable scuba effects.")]
	public bool scubaEnabled;
	public GameObject bubblesLeft;
	public GameObject bubblesRight;

    [Header("SunShafts Settings")]
    [Tooltip("This allows you to enable or disable the sunshafts effect.")]
    public bool sunshaftsEnabled;
    public GameObject sunShafts;

    void Start () 
	{
	    
        waterParticles_emission = WaterParticles.GetComponent<ParticleSystem>().emission;
        waterParticles_emission.enabled = true;
		bubbles1Particles_emission = bubblesLeft.GetComponent<ParticleSystem>().emission;
		bubbles2Particles_emission = bubblesRight.GetComponent<ParticleSystem>().emission;
		bubbles1Particles_emission.enabled = true;
		bubbles2Particles_emission.enabled = true;
        sunshaftsParticles_emission = sunShafts.GetComponent<ParticleSystem>().emission;
        sunshaftsParticles_emission.enabled = true;
			
		underWaterColor = new Color(0.0f/255.0F,64.0f/255.0F,196.0f/255.0F,255.0f/255.0F); // Set the color for underwater fog
		Player = GameObject.FindGameObjectWithTag("Player"); // Cache the player
		Caustics.enabled = false; // Initially turn off the caustics effect as we start above water
        checkedIfAboveWater = false;
		underWaterVisiblity = 0.01f; // Set the Underwater Visibility - can be adjusted publicly
        // Cache the audiosources for underwater, splash in and splash out of water
		m_AudioSource = waterBodyUnder.GetComponent<AudioSource>();
		m_JumpInWaterAudioSource = GameObject.FindGameObjectWithTag("JumpInWater").GetComponent<AudioSource> ();
		m_JumpOutOfWaterAudioSource = GameObject.FindGameObjectWithTag("JumpOutOfWater").GetComponent<AudioSource> ();
		Camera.main.nearClipPlane = 0.1f;
		waterHeight = waterBody.transform.position.y; // This is critical! It is the height of the water plane to determine we are underwater or not
		AssignAboveWaterSettings (); // Initially set above water settings
		ApplyAboveWaterSettings ();
	}
	// Update is called once per frame
	void Update () 
	{
        // the checkedifAboveWater stops it forcing it over and over every frame if we already know where we are
        // If tghe player is above water and we haven't confirmed this yet, then set settings for above water and confirm
		if (transform.position.y >= waterHeight && checkedIfAboveWater == false) 
		{
            checkedIfAboveWater = true;
			ApplyAboveWaterSettings ();
			ToggleFlares (true);
		}
        // If we are under water and we haven't confirmed this yet, then set for under water and confirm
		if (transform.position.y < waterHeight && checkedIfAboveWater == true) 
		{
			checkedIfAboveWater = false;
			ApplyUnderWaterSettings ();
			ToggleFlares (false);
		}
        // If we enable Scuba Tank while already underwater then start it up
		if (transform.position.y < waterHeight && scubaEnabled == true && !bubblesLeft.GetComponent<ParticleSystem>().isPlaying)
		{
			bubblesLeft.GetComponent<ParticleSystem>().Play ();
			bubblesRight.GetComponent<ParticleSystem>().Play ();
			bubbles1Particles_emission.enabled = true;
			bubbles2Particles_emission.enabled = true;
		}
        // If we turn off the Scuba Tank while still underwater then stop it
        if (transform.position.y < waterHeight && scubaEnabled == false && bubblesLeft.GetComponent<ParticleSystem>().isPlaying)
        {
            bubblesLeft.GetComponent<AudioSource>().Stop();
            bubblesRight.GetComponent<AudioSource>().Stop();
            bubblesLeft.GetComponent<ParticleSystem>().Stop();
            bubbles1Particles_emission.enabled = false;
            bubblesRight.GetComponent<ParticleSystem>().Stop();
            bubbles2Particles_emission.enabled = false;
        }
        // If we enable Sunshafts while already underwater then start it up
        if (transform.position.y < waterHeight && sunshaftsEnabled == true && !sunShafts.GetComponent<ParticleSystem>().isPlaying)
        {
            sunShafts.GetComponent<ParticleSystem>().Play();
            sunshaftsParticles_emission.enabled = true;
        }
        // If we turn off the Sunshafts while still underwater then stop it
        if (transform.position.y < waterHeight && sunshaftsEnabled == false && sunShafts.GetComponent<ParticleSystem>().isPlaying)
        {
            sunShafts.GetComponent<ParticleSystem>().Stop();
            sunshaftsParticles_emission.enabled = false;
        }

    }
    // Initially assign current world fog ready for reuse later in above water 
	void AssignAboveWaterSettings () 
	{
		aboveWaterFogMode = RenderSettings.fog;
		aboveWaterColor = RenderSettings.fogColor;
		aboveWaterVisiblity = RenderSettings.fogDensity;
	}
    // Apply Abovewater default settings - enabling the above water view and effects
    void ApplyAboveWaterSettings () 
	{
        waterBody.SetActive(true);
        waterBodyUnder.SetActive(false);
        StopUnderWaterSound ();
		PlayExitSplashSound();
		Player.SendMessage("aboveWater");
		if(WaterParticles.GetComponent<ParticleSystem>().isPlaying)
		{
			WaterParticles.GetComponent<ParticleSystem>().Stop ();
			WaterParticles.GetComponent<ParticleSystem>().Clear ();
		}
		RenderSettings.fog = aboveWaterFogMode;
		RenderSettings.fogColor = aboveWaterColor;
		RenderSettings.fogDensity = aboveWaterVisiblity;
        Caustics.enabled = false;
		
		bubblesLeft.GetComponent<AudioSource>().Stop ();
		bubblesRight.GetComponent<AudioSource>().Stop ();
		bubblesLeft.GetComponent<ParticleSystem>().Stop ();
		bubbles1Particles_emission.enabled = false;
        bubblesRight.GetComponent<ParticleSystem>().Stop ();
		bubbles2Particles_emission.enabled = false;

        sunshaftsParticles_emission.enabled = false;
        sunShafts.GetComponent<ParticleSystem>().Stop();

    }

    // Apply Underwater default settings - enabling the under water view and effects
    void ApplyUnderWaterSettings () 
	{
        
        waterBody.SetActive(false);
        waterBodyUnder.SetActive(true);
        PlayEnterSplashSound();
		PlayUnderWaterSound ();
		RenderSettings.fog = aboveWaterFogMode;
		RenderSettings.fogColor = underWaterColor;
		RenderSettings.fogDensity = underWaterVisiblity;
        Caustics.enabled = true;
        Player.SendMessage("underWater");
		if (!WaterParticles.GetComponent<ParticleSystem>().isPlaying)
		{
			WaterParticles.GetComponent<ParticleSystem>().Play ();
		}
		if (scubaEnabled == true)
		{
			bubblesLeft.GetComponent<ParticleSystem>().Play ();
			bubblesRight.GetComponent<ParticleSystem>().Play ();
			bubbles1Particles_emission.enabled = true;
			bubbles2Particles_emission.enabled = true;
		}
		
        if (sunshaftsEnabled == true)
        {
            sunshaftsParticles_emission.enabled = true;
            sunShafts.GetComponent<ParticleSystem>().Play();
        }
	}
    // Toggle flares on or off depending on whether we are underwater or not
	void ToggleFlares (bool state) 
	{
		LensFlare[] lensFlares = FindObjectsOfType(typeof(LensFlare)) as LensFlare[];
		foreach (LensFlare currentFlare in lensFlares) 
		{
			currentFlare.enabled = state;
		}
	}
	private void PlayUnderWaterSound()
	{
		m_AudioSource.Play ();
	}
	private void StopUnderWaterSound()
	{
		m_AudioSource.Stop ();
	}
	private void PlayEnterSplashSound()
	{
		m_JumpInWaterAudioSource.Play ();
	}
	private void PlayExitSplashSound()
	{
		m_JumpOutOfWaterAudioSource.Play ();
	}
}
