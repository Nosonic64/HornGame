using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlingScript : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpSpeed;
    public BoxCollider bc;

    private void Start()
    {
        bc = GetComponent<BoxCollider>();
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
            rb.velocity = Vector3.zero;
            rb.AddForce(this.transform.up * jumpSpeed, ForceMode.VelocityChange);
            rb = null;
        }
    }

    // Coroutine turns the collider off and on with a delay.
    // Coroutine gets called on a timer.
    // Timer can be found in "TrumpetCycleHandler" Object/Script
    public IEnumerator TrumpetShot()
    {
        bc.enabled = true;
        // !Add Feature!:  Play a burst of a particle system attached to this object, gives an indication that the player can now use the trumpet air to fling themselves
        yield return new WaitForSeconds(1f);
        bc.enabled = false;
        yield return null;
    }
}
