using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    // Start is called before the first frame update
    private CheckPointHandler checkpointHandlerScript;
    private CanvasTransition canvasTransition;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maximumSpeed;
    private Rigidbody charRigidBody;
    private bool isJumping = false;

    private Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int VelocityHash;
    int VelocityZHash;
    int VelocityYHash;

    float velocity = 0.0f;
    float velocityZ = 0.0f;
    float velocityY = 0.0f;

    [SerializeField]
    private float acceleration = 10000f;

    float localVelocityX;
    float localVelocityZ;
    float localVelocityY;

    private float lastForward;
    private float lastRight;

    private CapsuleCollider cc;
    private bool sliding;
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

    public float lastOnGroundTime;
    public float lastPressedJumpTime;
    public AudioSource audioPlayer;

    //SOUNDS
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip deathSound;

    void Awake()
    {
        charRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        VelocityHash = Animator.StringToHash("Velocity");
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityYHash = Animator.StringToHash("VelocityY");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        checkpointHandlerScript = FindObjectOfType<CheckPointHandler>();
        canvasTransition = FindObjectOfType<CanvasTransition>();
        composer = virtualCam.GetCinemachineComponent<CinemachineComposer>();
        canvasTransition.OpenBlackScreen();
    }
    #region Update Method
    void Update()
    {
        lastOnGroundTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;
        groundedCheck = IsGrounded();
        lookAt.transform.position = transform.position;
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        turn.y = Mathf.Clamp(turn.y, -45, 35);
        //Debug.Log(turn.y);
        lookAt.transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0);
            


        //composer.m_TrackedObjectOffset = new Vector3(composer.m_TrackedObjectOffset.x, -turn.y * 0.1f, composer.m_TrackedObjectOffset.z);

        if (Input.GetButtonDown("Fire2"))
        {
            
        }

        if (groundedCheck)
        {
            jumps = 0;
        }
        else
        {
            if (jumps == 0)
            {
                jumps = 1;
            }
        }

        localVelocityX = transform.InverseTransformDirection(charRigidBody.velocity).x;
        localVelocityZ = transform.InverseTransformDirection(charRigidBody.velocity).z;
        localVelocityY = transform.InverseTransformDirection(charRigidBody.velocity).y;

        float Currspeed = Vector3.Magnitude(charRigidBody.velocity);  // test current object speed

        if (Currspeed > maximumSpeed)

        {
            //Debug.Log("TOO FAST");
            float brakeSpeed = Currspeed - maximumSpeed;  // calculate the speed decrease

            Vector3 normalisedVelocity = charRigidBody.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value

            charRigidBody.AddForce(-brakeVelocity * 2);  // apply opposing brake force
        }

        isJumping = Input.GetButtonDown("Jump");
        
        if (Input.GetButtonDown("Jump") && jumps < 2)
        {
            if(jumps == 0)
            {
                animator.SetBool("Falling", true);
            }
            else
            {
                animator.SetBool("DoubleJump", true);
                audioPlayer.clip = jumpSound;
                audioPlayer.Play(0);
            }
            //animator.SetBool("Falling", true);
            charRigidBody.AddForce(new Vector3(0, 950, 0) + (transform.forward * 50));
            //Debug.Log("JUMP");
            jumps++;
        }
        

        velocity = charRigidBody.velocity.magnitude;
        velocityZ = localVelocityX;
        animator.SetFloat(VelocityHash, velocity);
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityYHash, localVelocityY);
        animator.SetFloat("VelocityY", localVelocityY);
        animator.SetBool("Grounded", groundedCheck);
    }
    #endregion

    #region Fixed Update Method
    void FixedUpdate()
    {
        if (!dying)
        {
            Vector3 moveRight = Input.GetAxis("Horizontal") * transform.right;
            Vector3 moveForward = Input.GetAxis("Vertical") * transform.forward;
            moveInput = moveRight + moveForward;
            if (moveInput.magnitude > 0)
            {
                charRigidBody.MoveRotation(Quaternion.Euler(0, turn.x, 0));
            }
            if (groundedCheck)
            {
                
                if (moveInput.magnitude > 0)
                {
                    /*Run(1);*/
                    charRigidBody.AddForce(moveInput * speed);
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
                charRigidBody.AddForce(moveInput * (speed / 2));
            }
            /*float moveRight = Input.GetAxis("Horizontal");
            float moveForward = Input.GetAxis("Vertical");
            Vector3 newForce = new Vector3(moveRight, 0, moveForward);*/
            //charRigidBody.AddForce(newForce * speed);
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
            audioPlayer.clip = deathSound;
            audioPlayer.Play(0);
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
