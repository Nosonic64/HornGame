using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlockTurnOn : MonoBehaviour
{
    private Animator anim;
    private BoxCollider bc;

    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            anim.SetBool("glockChallenge1", true);
            bc.enabled = false;
        }
    }
}
