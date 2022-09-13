using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
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
        if(col.gameObject.tag == "Player")
        {           
            checkpointHandlerScript.currentCheckpointLocation = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z);
        }
    }
}
