using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressHandler : MonoBehaviour
{
    public static ProgressHandler ProgressHandlerInstance;
    // Progress handler keeps track of what instruments the player has collected.
    public bool hasDrumkit;
    public bool hasBassGuitar;
    public bool hasElectricGuitar;
    public bool hasKeyboard;

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
