using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioSFXPlayTest : MonoBehaviour
{
    // this is just a test script to debug PlaySFX. Dont put it anywhere in the game.
    private PlaySFX playSound;
    void Start()
    {
        playSound = GetComponent<PlaySFX>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playSound.PlaySound();

        }
    }
}
