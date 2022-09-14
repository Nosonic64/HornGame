using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    [Header("Make sure there is only !ONE! CheckPointHandler object in the scene")]
    private CheckPointHandler checkpointHandlerScript;

    private void Start()
    {
        checkpointHandlerScript = FindObjectOfType<CheckPointHandler>();
    }
    private void OnTriggerEnter(Collider col)
    {
        // When the player enters a kill trigger, we set their position to be whatever Vector3 is currently saved in CheckPointHandler. 
        // We also turn their velocity to zero, so that they dont carry any velocity they had with them.
        if (col.gameObject.tag == "Player")
        {
            var rb = col.GetComponent<Rigidbody>();
            col.gameObject.transform.position = checkpointHandlerScript.currentCheckpointLocation;
            rb.velocity = Vector3.zero;
            rb = null;
        }
    }
}
