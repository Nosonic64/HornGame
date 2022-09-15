using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneHit : MonoBehaviour
{
    private PlaySFX playSFX;
    public Transform rockSpawnPoint;
    public float spawnDelayStart;
    public GameObject rock;
    private float spawnDelay;
    private GameObject currentRock;

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
            
            // We spawn a rock prefab at rockSpawnPoint and reset the spawn timer
            currentRock = Instantiate(rock, rockSpawnPoint);
            spawnDelay = spawnDelayStart;
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
