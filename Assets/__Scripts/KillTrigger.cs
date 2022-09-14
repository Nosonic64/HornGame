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
        if (col.gameObject.tag == "Player")
        {
            var rb = col.GetComponent<Rigidbody>();
            col.gameObject.transform.position = checkpointHandlerScript.currentCheckpointLocation;
            rb.velocity = Vector3.zero;
            rb = null;
        }
    }
}
