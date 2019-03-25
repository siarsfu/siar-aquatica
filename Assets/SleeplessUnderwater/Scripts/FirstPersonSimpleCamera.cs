using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class FirstPersonSimpleCamera : MonoBehaviour 
{

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;
    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;
    public float frameCounter = 20;
    Quaternion originalRotation;
    public bool m_IsSwimming;
    public bool m_IsUnderwater;
    public bool m_IsJumping;

    //public FreeMovementMotor motor;
    //public Transform character;
    //public GameObject cursorPrefab;
    //public GameObject player;
    
    public Vector3 movementDirection;

    // Simpler motors might want to drive movement based on a target purely
    public Vector3 movementTarget;

    // The direction the character wants to face towards, in world space.
    public Vector3 facingDirection;

    public float walkingSpeed = 5.0f;
    public float walkingSnappyness = 50f;
    public float turningSmoothing = 0.3f;

    private GameObject waterBody;
    void Start()
    {
        m_IsSwimming = false;
        m_IsJumping = false;
        m_IsUnderwater = false;
        waterBody = GameObject.FindGameObjectWithTag("Water");
        //Rigidbody rb = GetComponent<Rigidbody>();
        //if (rb)
         //   rb.freezeRotation = true;
        originalRotation = transform.localRotation;
    }
    void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            //Resets the average rotation
            rotAverageY = 0f;
            rotAverageX = 0f;

            //Gets rotational input from the mouse
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            //Adds the rotation values to their relative array
            rotArrayY.Add(rotationY);
            rotArrayX.Add(rotationX);

            //If the arrays length is bigger or equal to the value of frameCounter remove the first value in the array
            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }

            //Adding up all the rotational input values from each array
            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }

            //Standard maths to find the average
            rotAverageY /= rotArrayY.Count;
            rotAverageX /= rotArrayX.Count;

            //Clamp the rotation average to be within a specific value range
            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            //Get the rotation you will be at next as a Quaternion
            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

            //Rotate
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotAverageX = 0f;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotArrayX.Add(rotationX);
            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }
            rotAverageX /= rotArrayX.Count;
            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotAverageY = 0f;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotArrayY.Add(rotationY);
            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            rotAverageY /= rotArrayY.Count;
            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            transform.localRotation = originalRotation * yQuaternion;
        }
        // HANDLE CHARACTER MOVEMENT DIRECTION
        //if (Input.GetMouseButton(1) || Input.GetButton("Vertical"))
        //{
           // transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        //}
        // Make sure the direction vector doesn't exceed a length of 1
        // so the character can't move faster diagonally than horizontally or vertically
        if (movementDirection.sqrMagnitude > 1)
            movementDirection.Normalize();

        // optimization (instead of newing Plane):
        //playerMovementPlane.normal = character.up;
        //playerMovementPlane.distance = -character.position.y + cursorPlaneHeight;
        // Player is jumping if SPACE and not already jumping and not swimming
        

        var moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);

        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * 9.0f * Time.deltaTime);

        if (transform.position.y < waterBody.transform.position.y - 0.5f)
        {
            m_IsSwimming = true;
        }
        else
        {
            m_IsSwimming = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && m_IsJumping == false && m_IsUnderwater == false)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 400, ForceMode.Impulse);
        }
    }
    void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.tag == "Terrain" || theCollision.gameObject.tag == "Obstacle")
        {
            m_IsJumping = false;
        }
    }
    // Check to see if we are leaving a ground surface, we are most likely jumping
    void OnCollisionExit(Collision theCollision)
    {
        if (theCollision.gameObject.tag == "Terrain" || theCollision.gameObject.tag == "Obstacle")
        {
            m_IsJumping = true;
        }
    }
    void FixedUpdate()
    {
        // Handle the movement of the character
        Vector3 targetVelocity = movementDirection * walkingSpeed;
        Vector3 deltaVelocity = targetVelocity - GetComponent<Rigidbody>().velocity;
        if (GetComponent<Rigidbody>().useGravity)
            deltaVelocity.y = 0;
        GetComponent<Rigidbody>().AddForce(deltaVelocity * walkingSnappyness, ForceMode.Acceleration);
              
       
        // If we are swimming, then disable gravity and slowly sink
        if (m_IsUnderwater == true || m_IsSwimming == true)
        {
            GetComponent<Rigidbody>().useGravity = false;
            //GetComponent<Rigidbody>().AddForce(-Vector3.up * -5f, ForceMode.Acceleration);
        }
        else
        {
            // Otherwise we are not swimming so enable gravity
            GetComponent<Rigidbody>().useGravity = true;
        }
        
        // If we are swimming, and we hit SPACE, then head up slowly
        if (Input.GetKey(KeyCode.Space) && m_IsUnderwater == true)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 80, ForceMode.Acceleration);
        }
        //Crouching to Swim Down
        if (Input.GetKey(KeyCode.C) && m_IsUnderwater == true)
        {
            GetComponent<Rigidbody>().AddForce(-Vector3.up * 80, ForceMode.Acceleration);
        }
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
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
