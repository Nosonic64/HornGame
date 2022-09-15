using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    //[Header("Make sure there is only !ONE! CheckPointHandler object in the scene")]


    private void Start()
    {

    }
    private void OnTriggerEnter(Collider col)
    {
        // When the player enters a kill trigger, we set their position to be whatever Vector3 is currently saved in CheckPointHandler. 
        // We also turn their velocity to zero, so that they dont carry any velocity they had with them.
        if (col.gameObject.tag == "Player")
        {
            //StartCoroutine(PlayDeath(col));
            var _playerController = col.gameObject.GetComponent<ThirdPersonController>();
            _playerController.StartCoroutine(_playerController.CharacterDeath(col));
        }
    }
    IEnumerator PlayDeath(Collider col)
    {
        while (true)
        {
            Debug.Log("Time to Respawn!");
            yield return new WaitForSeconds(3.5f);
            Debug.Log("Time to Respawn!2");
            yield return null;
        }
        
    }
}
