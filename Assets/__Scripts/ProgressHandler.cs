using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressHandler : MonoBehaviour
{
    public static ProgressHandler ProgressHandlerInstance;
    // Progress handler keeps track of what instruments the player has collected.
    public bool hasInstrumentOne;
    public bool hasInstrumentTwo;
    public bool hasInstrumentThree;
    public bool hasInstrumentFour;

    void Awake()
    {
        // This code allows the progress handler to continue between scenes, so we know what instruments the player has collected.
        DontDestroyOnLoad(this);

        if (ProgressHandlerInstance == null)
        {
            ProgressHandlerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
