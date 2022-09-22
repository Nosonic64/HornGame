using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSoundPosToPlayer : MonoBehaviour
{
    public GameObject soundPos;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            soundPos.transform.position = col.transform.position;
            var playSFX = soundPos.GetComponent<PlaySFX>();
            playSFX.PlaySound();
        }
    }
}
