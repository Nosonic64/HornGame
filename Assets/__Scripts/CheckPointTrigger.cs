using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [Header("Make sure there is only !ONE! CheckPointHandler object in the scene")]
    private CheckPointHandler checkpointHandlerScript;

    private void Start()
    {
        checkpointHandlerScript = FindObjectOfType<CheckPointHandler>();    
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {           
            checkpointHandlerScript.currentCheckpointLocation = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z);
        }
    }
}
