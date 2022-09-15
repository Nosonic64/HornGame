using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(PlaySFX))]
public class FlingScript : MonoBehaviour
{
    private Rigidbody rb;
    private RigidbodyFirstPersonController player;
    public float jumpSpeed;
    private BoxCollider bc;
    private ParticleSystem particle;
    private PlaySFX playSFX;

    private void Start()
    {
        bc = GetComponent<BoxCollider>();
        particle = GetComponent<ParticleSystem>();  
        bc.enabled = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        // Get the players rigidbody, set its velocity to zero and then add the value from jumpSpeed to their velocity.
        // The direction the player is sent is based on transform.up of the object this script is attached to.
        // jumpSpeed is set in the inspector.
        if (col.gameObject.tag == "Player")
        {
            rb = col.gameObject.GetComponent<Rigidbody>();
            player = col.gameObject.GetComponent<RigidbodyFirstPersonController>();
            rb.velocity = Vector3.zero;
          //  player.advancedSettings.airControl = false;
            rb.AddForce(this.transform.up * jumpSpeed, ForceMode.VelocityChange);
            playSFX.PlaySound();
            rb = null;
        }
    }

    // Coroutine turns the collider off and on with a delay.
    // Coroutine gets called on a timer.
    // Timer can be found in "TrumpetCycleHandler" Object/Script
    public IEnumerator TrumpetShot()
    {
        bc.enabled = true;
        particle.Play();
        yield return new WaitForSeconds(1f);
        bc.enabled = false;
        yield return null;
    }
}
