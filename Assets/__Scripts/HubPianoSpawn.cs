using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPianoSpawn : MonoBehaviour
{
    private ProgressHandler progressHandler;
    private AudioSource audioSource;
    void Awake()
    {
        progressHandler = FindObjectOfType<ProgressHandler>();
        audioSource = GetComponent<AudioSource>();
        if (progressHandler.hasKeyboard)
        {
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
