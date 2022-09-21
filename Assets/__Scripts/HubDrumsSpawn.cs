using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubDrumsSpawn : MonoBehaviour
{
    private ProgressHandler progressHandler;  
    private AudioSource audioSource;   
    void Awake()
    {
        progressHandler = FindObjectOfType<ProgressHandler>();
        audioSource = GetComponent<AudioSource>();
        if (progressHandler.hasDrumkit)
        {
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
