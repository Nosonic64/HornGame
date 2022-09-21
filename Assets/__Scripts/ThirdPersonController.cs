using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    public float jumpHeight;
    // Start is called before the first frame update
    private CheckPointHandler checkpointHandlerScript;
    public CanvasTransition canvasTransition;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maximumSpeed;
    private Rigidbody charRigidBody;
    private bool bJumping;

    private Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int VelocityHash;
    int VelocityZHash;
    int VelocityYHash;

    /*float velocity = 0.0f;
    float velocityZ = 0.0f;
    float velocityY = 0.0f;*/


    private float acceleration = 10000f;

    //Getting Local Forward, Right, and Up;
    float localVelocityX;
    float localVelocityZ;
    float localVelocityY;

    //For implimenting slide mechanics [not implemented atm]
    /*private float lastForward;
    private float lastRight;
    private bool sliding;*/

    private CapsuleCollider cc;
    private int jumps;

    private Vector2 turn;
    public GameObject lookAt;
    public CinemachineVirtualCamera virtualCam;
    private bool doubleJump;

    private bool dying;
    private CinemachineComposer composer;

    public LayerMask layerMask;

    private bool groundedCheck;

    private Vector3 moveInput;
    private Vector3 brakeVelocity;

    public float lastOnGroundTime;
    public float lastPressedJumpTime;
    public AudioSource audioPlayer;

    //SOUNDS
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip[] walkingSoundsMetal;
    [SerializeField]
    private AudioClip[] walkingSoundsChime;
    [SerializeField]
    private AudioClip[] walkingSoundsDrum;

    private FMOD.Studio.EventInstance instance;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    //Walking Bool
    private bool amWalking;

    private Vector3 moveRight;
    private Vector3 moveForward;

    


    void Awake()
    {
        //Get our Rigidbody, Animator, and Capsule Collider.
        charRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider>();
        checkpointHandlerScript = FindObjectOfType<CheckPointHandler>();
        canvasTransition = FindObjectOfType<CanvasTransition>();
        composer = virtualCam.GetCinemachineComponent<CinemachineComposer>();

        //Setup hashes for faster calls to the animator.
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        VelocityHash = Animator.StringToHash("Velocity");
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityYHash = Animator.StringToHash("VelocityY");

    }

    void Start()
    {
        //Lock Cursor,
        Cursor.lockState = CursorLockMode.Locked;

        //Make screen start black and open up!
        canvasTransition.OpenBlackScreen();
        
    }
    #region Update Method
    void Update()
    {
        moveRight = Input.GetAxis("Horizontal") * transform.right;
        moveForward = Input.GetAxis("Vertical") * transform.forward;
        moveInput = moveRight + moveForward;
        // get force of acceleration
        float forceOfAcceleration = Mathf.Abs(charRigidBody.mass * Physics.gravity.y);

        //Check if we are grounded!
        groundedCheck = IsGrounded();

        //Allow the player to rotate the camera for better views.
        lookAt.transform.position = transform.position;
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        turn.y = Mathf.Clamp(turn.y, -45, 35);
        lookAt.transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0);

        //Reset Jumps if we are grounded
        if (groundedCheck)
        {
            jumps = 0;
        }
        else
        {
            //If we are not grounded and we have not jumped yet, set jumps to 1 so that the player may only double jump.
            if (jumps == 0)
            {
                jumps = 1;
            }
        }

        //Get the local velocity in transformForward, transformRight, transformUp
        localVelocityX = transform.InverseTransformDirection(charRigidBody.velocity).x;
        localVelocityZ = transform.InverseTransformDirection(charRigidBody.velocity).z;
        localVelocityY = transform.InverseTransformDirection(charRigidBody.velocity).y;
        
        //Get Current Speed of the Player.
        float Currspeed = Vector3.Magnitude(charRigidBody.velocity);  // test current object speed

        //If the player is moving faster than our maximum speed we need to slow them down by applying an opposite force.
        if (Currspeed > maximumSpeed)
        {
            float brakeSpeed = Currspeed - maximumSpeed;  // calculate the speed decrease
            Vector3 normalisedVelocity = charRigidBody.velocity.normalized;
            brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value
            //charRigidBody.AddForce(-brakeVelocity * 2);  // apply opposing brake force
        }
        
        //If the player wants to jump and we have jumped up to twice before hitting the ground
        if (Input.GetButtonDown("Jump") && jumps < 2)
        {
            bJumping = true;
        }
        
        //Update the animator with velocity directions.
        animator.SetFloat(VelocityHash, charRigidBody.velocity.magnitude);
        animator.SetFloat(VelocityZHash, localVelocityX);
        animator.SetFloat(VelocityYHash, localVelocityY);
        animator.SetFloat("VelocityY", localVelocityY);
        animator.SetBool("Grounded", groundedCheck);
    }
    #endregion

    #region Fixed Update Method
    void FixedUpdate()
    {
        #region JUMP
        //If the player has pressed jump then we start jumping!
        if (bJumping)
        {
            //Apply jump height -- I recieved a tip that if you multiply by Time.deltaTime it should make the jumpHeight frame independent?
            float newjumpHeight = jumpHeight * Time.deltaTime;
            float jumpForward = 5 * Time.deltaTime;

            //If Jumps are at 0 we do a default jump
            if (jumps == 0)
            {
                animator.SetBool("Falling", true);
                charRigidBody.AddForce(new Vector3(0, newjumpHeight, 0) + (jumpForward * transform.forward), ForceMode.Impulse);
                //charRigidBody.velocity = new Vector3(0, 20, 0) + (transform.forward * 5);

                instance = FMODUnity.RuntimeManager.CreateInstance("event:/Player/Jump");
                instance.start();
                instance.release();
            }
            //If Jumps are at 1, we are in the air, so we do a special double jump!
            else
            {
                animator.SetBool("DoubleJump", true);
                
                //Reset velocity in the y direction so that the jump height may remain the same no matter your downwards velocity
                charRigidBody.velocity = new Vector3(charRigidBody.velocity.x, 0, charRigidBody.velocity.z);
                charRigidBody.AddForce(new Vector3(0, newjumpHeight, 0), ForceMode.Impulse);

                instance = FMODUnity.RuntimeManager.CreateInstance("event:/Player/Voice");
                instance.start();
                instance.release();

            }
            jumps++;
            bJumping = false;
        }
        #endregion

        

        if (charRigidBody.velocity.y < -.50)
        {
            charRigidBody.velocity += Vector3.up * Physics.gravity.y * (2.4f - 1) * Time.deltaTime;
        }
        if (!dying)
        {
            
            
            if (moveInput.magnitude > 0)
            {
                charRigidBody.MoveRotation(Quaternion.Euler(0, turn.x, 0));
                
                

            }
            if (groundedCheck)
            {
                
                if (moveInput.magnitude > 0)
                {
                    /*Run(1);*/
                    charRigidBody.angularDrag = 0.2f;
                    //charRigidBody.AddForce(moveInput * speed);
                    charRigidBody.AddForce(moveInput * speed * Time.deltaTime);
                    /*if (OnSlope())
                    {
                        Debug.Log("ON SLOPE");
                        charRigidBody.AddForce(GetSlopeMoveDirection() * speed * Time.deltaTime);

                        if (charRigidBody.velocity.y > 0)
                            charRigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
                    }
                    else
                    {
                        
                    }*/
                    Debug.Log(moveInput * speed);
                    if (!amWalking)
                    {
                        StartCoroutine(Walking());
                    }
                }
                else
                {
                    charRigidBody.angularDrag = 1;
                    charRigidBody.AddForce(-transform.forward * charRigidBody.velocity.magnitude * Time.deltaTime);
                }
                //Debug.Log(moveForward);
                /*if (moveForward * lastForward < 0 || moveRight * lastRight < 0)
                {
                    Debug.Log("Change DIRECTION");
                    //animator.SetBool("Slide", true);
                    //StartCoroutine(SlideStop());

                }*/
                //lastForward = moveForward;
                //lastRight = moveRight;

            }
            else
            {
                /*Run(1);*/
                charRigidBody.AddForce(moveInput * (speed / 2) * Time.deltaTime);
            }
            /*float moveRight = Input.GetAxis("Horizontal");
            float moveForward = Input.GetAxis("Vertical");
            Vector3 newForce = new Vector3(moveRight, 0, moveForward);*/
            //charRigidBody.AddForce(newForce * speed);
            float Currspeed = Vector3.Magnitude(charRigidBody.velocity);  // test current object speed

            //If the player is moving faster than our maximum speed we need to slow them down by applying an opposite force.
            if (Currspeed > maximumSpeed)
            {
                charRigidBody.AddForce(-brakeVelocity * 2);
            }
                
        }
    }
    #endregion

    #region Check Grounded
    private bool IsGrounded()
    {

        //SPHERECAST origin, direction, extent
        RaycastHit m_Hit;
        bool raycastHit = Physics.SphereCast(transform.position + transform.up, 0.4f, -transform.up, out m_Hit, 1f, layerMask, QueryTriggerInteraction.UseGlobal);
        Color rayColor;
        if (raycastHit)
        {
            rayColor = Color.green;
        }
        else {
            rayColor = Color.red;
        }

        //Debug.DrawRay(cc.bounds.center, Vector3.down * (cc.bounds.extents.y + 0.1f));
        if (raycastHit)
        {
            lastOnGroundTime = 0.1f;
        }
        Debug.Log(raycastHit);
        return raycastHit;
    }
    #endregion

    #region Drawing Gizmos Method
    private void OnDrawGizmosSelected()
    {
        if (groundedCheck)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        
        Debug.DrawLine(transform.position, transform.position + -transform.up * (1f));
        Gizmos.DrawWireSphere(transform.position + -transform.up * (1f), 0.4f);
    }
    #endregion

    public IEnumerator CharacterDeath(Collider col)
    {
        if (!dying)
        {

            instance = FMODUnity.RuntimeManager.CreateInstance("event:/Player/Death");
            instance.start();
            instance.release();


            instance = FMODUnity.RuntimeManager.CreateInstance("event:/Player/Transition");
            instance.start();
            instance.release();
            /*audioPlayer.clip = deathSound;
            audioPlayer.Play(0);*/
            dying = true;
            animator.SetBool("Death", true);
            canvasTransition.CloseBlackScreen();
            yield return new WaitForSeconds(2f);
            var rb = col.GetComponent<Rigidbody>();
            col.gameObject.transform.position = checkpointHandlerScript.currentCheckpointLocation;
            rb.velocity = Vector3.zero;
            rb = null;
            canvasTransition.OpenBlackScreen();
            dying = false;
        }
        
    }

    private IEnumerator Walking()
    {

        amWalking = true;
        /*int randomClip = Random.Range(0, 5);
        audioPlayer.clip = walkingSoundsMetal[randomClip];
        audioPlayer.Play(0);*/
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/Player/Footsteps");
        instance.start();
        instance.release();
        float waitSpeed =  1 / charRigidBody.velocity.magnitude;
        waitSpeed = Mathf.Clamp(waitSpeed, 0.3f, 0.6f);
        yield return new WaitForSeconds(waitSpeed);
        amWalking = false;



    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position + transform.forward + (transform.up * 2), Vector3.down, out slopeHit, 5f, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            bool asd123 = angle < maxSlopeAngle && angle != 0;
            Debug.Log(angle + " / " + asd123 + " THISANGLE");
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveInput, slopeHit.normal).normalized;
    }

    /*#region Run Method
    private void Run(float lerpAmount)
    {
        float targetSpeed = moveInput.magnitude * speed;
        targetSpeed = Mathf.Lerp(charRigidBody.velocity.magnitude, targetSpeed, lerpAmount);

        float accelRate;

        //Get how much we accelerate by depending if we are accelerating or decelerating and also taking into account if we are in air or not.
        if (lastOnGroundTime > 0)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
        }
        else
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;
        }

        if (data.doConserveMomentum && Mathf.Abs(charRigidBody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(charRigidBody.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && lastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }

        float speedDif = targetSpeed - charRigidBody.velocity.magnitude;

        float movement = speedDif * accelRate;

        charRigidBody.AddForce(movement * moveInput, ForceMode.Force);
        Debug.Log("EEEEEEEEEEEEEE");
    }
    #endregion*/
}
