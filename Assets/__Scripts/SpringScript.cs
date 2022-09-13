using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaySFX))]
public class SpringScript : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpSpeed; 
    private PlaySFX playSFX;

    private void Start()
    {
        playSFX = GetComponent<PlaySFX>();
    }
    private void OnTriggerEnter(Collider col)
    {
        // Get the players rigidbody, set its velocity to zero and then add the value from jumpSpeed to their Y velocity.
        // jumpSpeed is set in the inspector.
        if(col.gameObject.tag == "Player")
        {
            rb = col.gameObject.GetComponent<Rigidbody>(); 
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * jumpSpeed, ForceMode.VelocityChange);
            playSFX.PlaySound();
            rb = null;
        }
    }
}
