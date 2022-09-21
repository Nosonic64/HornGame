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
    public AudioSource keyAudioSource;
    private Rigidbody keyRigidbody;

    //Other Useful Variables
    private bool objectTriggered;

    private Vector3 returnPos;
    void Start()
    {
        returnPos = transform.position;
        //keyAudioSource = transform.GetComponent<AudioSource>();
        keyRigidbody = transform.GetComponent<Rigidbody>();
        keyRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.tag == "Player")
        {
            col.collider.gameObject.transform.parent.parent = this.gameObject.transform;
            objectTriggered = true;
            //Play Note
            keyAudioSource.clip = noteToPlay;
            keyAudioSource.Play(0);
            StartCoroutine(ObjectFall());
        }
        
        //
    }

    void OnCollisionExit(Collision col)
    {
        if(col.collider.gameObject.tag == "Player")
        {
            col.collider.gameObject.transform.parent.parent = null;
            //PushAway();
            PushAway();
            StartCoroutine(ObjectFall());
        }
        
    }

    public IEnumerator ObjectFall()
    {
        yield return new WaitForSeconds(5f);
        PushAway();
        yield return new WaitForSeconds(5f);
        ComeBack();


    }

    private void PushAway()
    {
        if (objectTriggered)
        {
            Debug.Log("WillNowFall");
            //keyAudioSource.clip = breakSFX;
            //keyAudioSource.Play(0);
            objectTriggered = false;
            keyRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; ;
            keyRigidbody.mass = 1;
            keyRigidbody.useGravity = true;
            
        }
    }

    private void ComeBack()
    {
        transform.position = returnPos;
        keyRigidbody.useGravity = false;
        keyRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}
