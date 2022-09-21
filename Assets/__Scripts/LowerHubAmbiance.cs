using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerHubAmbiance : MonoBehaviour
{
    private ProgressHandler progressHandler;
    private AudioSource audioSource;
    void Awake()
    {
        progressHandler = FindObjectOfType<ProgressHandler>();
        audioSource = GetComponent<AudioSource>();
        for(int i = progressHandler.instrumentCount; i > 0; i--)
        {
            audioSource.volume -= 0.25f;
        }
        
    }
}
