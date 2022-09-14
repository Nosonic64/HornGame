using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    // Start is called before the first frame update
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
        

    }
    void Update()
    {
        if (IsGrounded())
        {
            jumps = 0;
            animator.SetBool("Falling", false);
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
            Debug.Log("TOO FAST");
            float brakeSpeed = Currspeed - maximumSpeed;  // calculate the speed decrease

            Vector3 normalisedVelocity = charRigidBody.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value

            charRigidBody.AddForce(-brakeVelocity * 2);  // apply opposing brake force
        }

        isJumping = Input.GetButtonDown("Jump");
        
        if (Input.GetButtonDown("Jump") && jumps < 2)
        {
            animator.SetBool("Falling", true);
            charRigidBody.AddForce(new Vector3(0, 1500, 500));
            Debug.Log("JUMP");
            jumps++;
        }
        

        velocity = charRigidBody.velocity.magnitude;
        velocityZ = localVelocityX;
        animator.SetFloat(VelocityHash, velocity);
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityYHash, localVelocityY);
        animator.SetBool("Grounded", IsGrounded());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float moveRight = Input.GetAxis("Horizontal");
        float moveForward = Input.GetAxis("Vertical");
        Vector3 newForce = new Vector3(moveRight, 0, moveForward);
        if (IsGrounded())
        {
            charRigidBody.AddForce(newForce * speed);
            //Debug.Log(moveForward);
            if (moveForward * lastForward < 0 || moveRight * lastRight < 0)
            {
                Debug.Log("Change DIRECTION");
                animator.SetBool("Slide", true);
                StartCoroutine(SlideStop());
                
            }
            lastForward = moveForward;
            lastRight = moveRight;
            
        }
        else
        {
            charRigidBody.AddForce(newForce * (speed / 5));
        }
        /*float moveRight = Input.GetAxis("Horizontal");
        float moveForward = Input.GetAxis("Vertical");
        Vector3 newForce = new Vector3(moveRight, 0, moveForward);*/
        //charRigidBody.AddForce(newForce * speed);
        

        
    }

    private bool IsGrounded()
    {
        bool raycastHit = Physics.Raycast(cc.bounds.center, Vector3.down, cc.bounds.extents.y + 0.1f);
        Color rayColor;
        if (raycastHit)
        {
            rayColor = Color.green;
        }
        else {
            rayColor = Color.red;
        }

        Debug.DrawRay(cc.bounds.center, Vector3.down * (cc.bounds.extents.y + 0.1f));
        return raycastHit;
    }

    public IEnumerator SlideStop()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Slide", false);


    }

}
