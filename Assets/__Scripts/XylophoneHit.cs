using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneHit : MonoBehaviour
{
    //this script and everything associated with the xylophone keys is fucking esoteric
    private PlaySFX playSFX;
    public Transform rockSpawnPoint;
    [Header("Change this value to wait longer before spawning a rock")]
    public float spawnDelayStart;
    public GameObject rock;
    [Header("Dont touch this value")]
    public float spawnDelay;
    private GameObject currentRock;
    [Header("Set this key to be the last in a sequence of keys")]
    public bool lastInSequence;
    public GameObject sequenceHandler;

    void Start()
    {
        playSFX = GetComponent<PlaySFX>();
        spawnDelay = spawnDelayStart;
    }

    private void OnTriggerEnter(Collider col)
    {
        // If we collide with the rock that has spawned, we start the KillRock Coroutine
        if(col.gameObject == currentRock)
        {
            StartCoroutine(KillRock());
            // If we are the last xylophone key in a platforming sequence, we call up the XylophoneSequenceHandler to reset all the rock spawn timers
            // associated with the XylophoneSequenceHandler.
            if(lastInSequence)
            {
                var sequenceScript = sequenceHandler.GetComponent<XylophoneSequenceHandler>();
                sequenceScript.SequenceReset();   
            }
        }
    }

    private void Update()
    {
        // Decrease spawn timer until its at 0
        spawnDelay -= Time.deltaTime;

        if(spawnDelay <= 0f)
        {
            // If there is still a rock gameobject occupying the currentRock variable, we destroy it before creating another one
            // This is just incase the rock that was spawned before hasnt been destroyed for some reason.
            if(currentRock != null)
            {
                Destroy(currentRock);
            }
            
            // We spawn a rock prefab at rockSpawnPoint and set the timer to a high number.
            // We want to wait for a XylophoneSequenceHandler to reset our spawnDelay timer.
            // This is to make sure the platforming sequences made using multiple xylophone keys stay consistent.
            currentRock = Instantiate(rock, rockSpawnPoint);
            spawnDelay = 999f;
        }
    }

    IEnumerator KillRock()
    {
        // We play a sound and then wait a tiny bit before destroying the rock
        // This just looks a bit better
        playSFX.PlaySound();
        yield return new WaitForSeconds(0.1f);
        Destroy(currentRock);
        currentRock = null;
        yield return null;
    }
}
