using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From the Unity AngryBots Demo
// We have added the ability to handle underwater effects, jumping and swimming in this script
public class PlayerMoveController : MonoBehaviour
{
    // Objects to drag in
    public FreeMovementMotor motor;
    public Transform character;
    public GameObject cursorPrefab;
    public GameObject player;

    // Settings
    public float cameraSmoothing = 0.01f;
    public float cameraPreview = 2.0f;

    // Cursor settings
    public float cursorPlaneHeight = 0;
    public float cursorFacingCamera = 0;
    public float cursorSmallerWithDistance = 0;
    public float cursorSmallerWhenClose = 1;

    // Private member data
    private Camera mainCamera;

    private Transform mainCameraTransform;
    private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 cameraOffset = Vector3.zero;
    private Vector3 initOffsetToPlayer;

    //private Vector3 targetCamOffset;
    private Vector3 aimCamOffset;

    // Prepare a cursor point varibale. This is the mouse position on PC and controlled by the thumbstick on mobiles.
    private Vector3 cursorScreenPosition;

    private Plane playerMovementPlane;

    private Quaternion screenMovementSpace;
    private Vector3 screenMovementForward;
    private Vector3 screenMovementRight;

    public bool m_IsSwimming;
    public bool m_IsUnderwater;
    public bool m_IsJumping;

    private GameObject waterBody;
    void Awake()
    {
        m_IsSwimming = false;
        m_IsJumping = false;
        m_IsUnderwater = false;
        motor.movementDirection = Vector2.zero;
        motor.facingDirection = Vector2.zero;

        // Set main camera
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;

        waterBody = GameObject.FindGameObjectWithTag("Water");
        // Ensure we have character set
        // Default to using the transform this component is on
        if (!character)
            character = transform;

        initOffsetToPlayer = mainCameraTransform.position - character.position;
        aimCamOffset = mainCameraTransform.position - new Vector3(character.position.x - 2.0f, character.position.y + 2.0f, character.position.z - 5.0f);

        // Save camera offset so we can use it in the first frame
        cameraOffset = mainCameraTransform.position - character.position;
        // Set the initial cursor position to the center of the screen
        cursorScreenPosition = new Vector3(0.5f * Screen.width, 0.5f * Screen.height, 0);
        // caching movement plane
        playerMovementPlane = new Plane(character.up, character.position + character.up * cursorPlaneHeight);
    }

    void Start()
    {
        // it's fine to calculate this on Start () as the camera is static in rotation
        screenMovementSpace = Quaternion.Euler(0, mainCameraTransform.eulerAngles.y, 0);
        screenMovementForward = screenMovementSpace * Vector3.forward;
        screenMovementRight = screenMovementSpace * Vector3.right;
    }

    void Update()
    {
        // HANDLE CHARACTER MOVEMENT DIRECTION
        if (Input.GetMouseButton(1) || Input.GetButton("Vertical"))
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }
        // Make sure the direction vector doesn't exceed a length of 1
        // so the character can't move faster diagonally than horizontally or vertically
        if (motor.movementDirection.sqrMagnitude > 1)
            motor.movementDirection.Normalize();

        // optimization (instead of newing Plane):
        playerMovementPlane.normal = character.up;
        playerMovementPlane.distance = -character.position.y + cursorPlaneHeight;

        if (transform.position.y < waterBody.transform.position.y - 0.5f)
        {
            m_IsSwimming = true;
        }
        else
        {
            m_IsSwimming = false;
        }
    }

    public static Vector3 PlaneRayIntersection(Plane plane, Ray ray)
    {
        float dist;
        plane.Raycast(ray, out dist);
        return ray.GetPoint(dist);
    }

    public void inWater()
    {
        m_IsSwimming = true;
    }
    public void outOfWater()
    {
        m_IsSwimming = false;
    }
    public void underWater()
    {
        m_IsUnderwater = true;
    }
    public void aboveWater()
    {
        m_IsUnderwater = false;
    }
}