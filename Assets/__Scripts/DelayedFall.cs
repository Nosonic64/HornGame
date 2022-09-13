using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedFall : MonoBehaviour
{
    // Source AudioClips to Play
    [SerializeField]
    private AudioClip noteToPlay;
    [SerializeField]
    private AudioClip breakSFX;

    //Get Components
    private AudioSource keyAudioSource;
    private Rigidbody keyRigidbody;

    //Other Useful Variables
    private bool objectTriggered;

    void Start()
    {
        keyAudioSource = transform.GetComponent<AudioSource>();
        keyRigidbody = transform.GetComponent<Rigidbody>();
        keyRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter()
    {
        objectTriggered = true;
        //Play Note
        keyAudioSource.clip = noteToPlay;
        keyAudioSource.Play(0);
        StartCoroutine(ObjectFall());
        //
    }

    void OnCollisionExit()
    {
        objectTriggered = false;
        //Play Note
        /*keyAudioSource.clip = noteToPlay;
        keyAudioSource.Play(0);
        StartCoroutine(ObjectFall());*/
        Debug.Log("FallingEarly!");
        keyAudioSource.clip = breakSFX;
        keyAudioSource.Play(0);
        //
    }

    public IEnumerator ObjectFall()
    {
        yield return new WaitForSeconds(5f);
        if (objectTriggered)
        {
            Debug.Log("WillNowFall");
            keyAudioSource.clip = breakSFX;
            keyAudioSource.Play(0);
        }
       
    }
}
