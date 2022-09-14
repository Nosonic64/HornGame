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
        keyRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
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
        PushAway();
        //
    }

    public IEnumerator ObjectFall()
    {
        yield return new WaitForSeconds(5f);
        PushAway();


    }

    private void PushAway()
    {
        if (objectTriggered)
        {
            Debug.Log("WillNowFall");
            keyAudioSource.clip = breakSFX;
            keyAudioSource.Play(0);
            objectTriggered = false;
            keyRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            keyRigidbody.mass = 1;
            
        }
    }
}
