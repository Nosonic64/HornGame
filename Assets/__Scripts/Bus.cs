using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bus : MonoBehaviour
{
    FMOD.Studio.Bus bus;

    public AudioMixer mixer;
    private float ourVolume;
    // Start is called before the first frame update
    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/Bus");
        
    }

    // Update is called once per frame
    void Update()
    {
        mixer.GetFloat("characterVolume", out ourVolume);
        bus.setVolume(DecibelToLinear(ourVolume));
    }

    private float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20f);
        return linear;
    }
}
