using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySFX : MonoBehaviour
{
    [Header("Drag sounds here")]
    [Tooltip("If you place more than one sound in here, and random sound isnt ticked, we play the sounds in sequence")]
    public AudioClip[] sounds = new AudioClip[0];
    AudioSource audioPlayer;
    private int soundAmount;
    private int i;
    [Header("Turn this on to have the sound use a random volume")]
    [Tooltip("Random volume between volumeMin and volumeMax will be chosen everytime the sound is played")]
    public bool useRandomVolume;
    [Range(0f, 1f)]
    public float volumeMin;
    [Range(0f, 1f)]
    public float volumeMax;
    [Header("Turn this on to have the sound use a random pitch")]
    [Tooltip("Random pitch between pitchMin and pitchMax will be chosen everytime the sound is played")]
    public bool useRandomPitch;
    [Range(0f, 3f)]
    public float pitchMin;
    [Range(0f, 3f)]
    public float pitchMax;
    private int lastChosenSound;
    [Header("Turn this on to have a random sound from sounds play everytime")]
    public bool randomSound;
    [Header("Turn this on to have the last sound played not repeat")]
    public bool dontRepeatSound;
    [Header("Turn this on to have a sound play through entirely"), Space(1), Header ("before another sound can be played")]
    public bool dontInterrupt;
    [Header("Add an object with a trigger volume here to have it trigger this sound")]
    public GameObject trigger;


    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.playOnAwake = false;
        soundAmount = sounds.Length;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (trigger != null && col.gameObject.tag == "Player")
        {
            PlaySound();
        }
    }

    public void PlaySound()
    {
        if (sounds != null)
        {
            // If dontInterrupt is true, and we are currently playing a sound, we just return. We dont want to interrupt the currently playing sound.
            if(audioPlayer.isPlaying & dontInterrupt)
            {
                return;
            }

            // If we have more than one sound and randomSound is true, we select a random sound between 0 and the amount of sounds we have in the array.
            // If dontRepeatSound is true and the random sound we picked is the same as the last sound we picked, we go back and redo the random number generation.
            if (soundAmount > 1 & randomSound)
            {
            redo:
                i = Random.Range(0, soundAmount);
                if (i == lastChosenSound && dontRepeatSound)
                {
                    goto redo;
                }
                else
                {
                    lastChosenSound = i;
                }
            }

            // If useRandomVolume is true, we select a random volume for the sound between the volumeMin and volumeMax
            if (useRandomVolume)
            {
                audioPlayer.volume = Random.Range(volumeMin, volumeMax);
            }

            // If useRandomPitch is true, we select a random volume for the sound between the pitchMin and pitchMax
            if (useRandomPitch)
            {
                audioPlayer.pitch = Random.Range(pitchMin, pitchMax);
            }

            // If randomSound is true, we just play the selected sound and return.
            // We cant go over the number in the array while randomSound is true, so we dont have to check for it.
            if (randomSound)
            {
                audioPlayer.clip = sounds[i];
                audioPlayer.Play();
                return;
            }

            // The default behaviour of PlaySound will go through sounds in the array sequentially.
            // We add 1 to i after we play a sound, ensuring the next sound to be played is next in the sequence.
            // If i isnt under the amount of sounds we have next time we try and play a sound, we set i to 0 before adding to it again.
            // This is to ensure we dont leave the bounds of the array. 
            if(i < soundAmount)
            {
                audioPlayer.clip = sounds[i];
                audioPlayer.Play();
                i += 1;
            }
            else
            {
                i = 0;
                audioPlayer.clip = sounds[i];
                audioPlayer.Play();
                i += 1;
            }
        }  
    }
}
