using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubBassSpawn : MonoBehaviour
{
    private ProgressHandler progressHandler;
    private AudioSource audioSource;
    void Awake()
    {
        progressHandler = FindObjectOfType<ProgressHandler>();
        audioSource = GetComponent<AudioSource>();
        if (progressHandler.hasBassGuitar)
        {
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
