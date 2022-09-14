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
        if(col.gameObject == currentRock)
        {
            StartCoroutine(KillRock());
        }
    }

    private void Update()
    {
        spawnDelay -= Time.deltaTime;

        if(spawnDelay <= 0f)
        {
            if(currentRock != null)
            {
                Destroy(currentRock);
            }
            
            currentRock = Instantiate(rock, rockSpawnPoint);
            spawnDelay = spawnDelayStart;
        }
    }

    IEnumerator KillRock()
    {
        playSFX.PlaySound();
        yield return new WaitForSeconds(0.1f);
        Destroy(currentRock);
        currentRock = null;
        yield return null;
    }
}
