using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
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

    private Rigidbody rb;
    float localVelocityX;
    float localVelocityZ;
    float localVelocityY;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        VelocityHash = Animator.StringToHash("Velocity");
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityYHash = Animator.StringToHash("VelocityY");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Falling", true);
        }
        localVelocityX = transform.InverseTransformDirection(rb.velocity).x;
        localVelocityZ = transform.InverseTransformDirection(rb.velocity).z;
        localVelocityY = transform.InverseTransformDirection(rb.velocity).y;

        /*animator.SetBool(isWalkingHash, false);
        animator.SetBool(isRunningHash, false);*/

        /*if (Input.GetButton("Vertical") && velocity < 1.1f)
        {
            //animator.SetBool(isWalkingHash, true);
            velocity = rb.velocity.magnitude;
        }
        if (Input.GetButton("Horizontal") && velocity < 1.1f)
        {
            //animator.SetBool(isWalkingHash, true);
            velocity = rb.velocity.magnitude;
            
        }

        if (!Input.GetButton("Vertical") && velocity > 0)
        {
            //animator.SetBool(isWalkingHash, true);
            velocity -= Time.deltaTime * acceleration;
        }*/

        /*if(Input.GetButton("Sprint"))
        {
            animator.SetBool(isRunningHash, true);
        }*/
        velocity = rb.velocity.magnitude;
        velocityZ = localVelocityX;
        animator.SetFloat(VelocityHash, velocity);
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityYHash, localVelocityY);
    }
}
