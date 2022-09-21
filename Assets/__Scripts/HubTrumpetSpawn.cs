using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubTrumpetSpawn : MonoBehaviour
{
    private ProgressHandler progressHandler;
    private AudioSource audioSource;
    void Awake()
    {
        progressHandler = FindObjectOfType<ProgressHandler>();
        audioSource = GetComponent<AudioSource>();
        if (progressHandler.hasElectricGuitar)
        {
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
