using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    [Header("!Make sure to drag 'CheckPointHandler' from the scene into this, otherwise it wont function!")]
    public GameObject checkpointHandler;
    private CheckPointHandler checkpointHandlerScript;

    private void Start()
    {
        checkpointHandlerScript = checkpointHandler.GetComponent<CheckPointHandler>();
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
