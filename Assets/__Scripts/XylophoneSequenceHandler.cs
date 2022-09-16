using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneSequenceHandler : MonoBehaviour
{
    public XylophoneHit[] xyKeys = new XylophoneHit[0];
   public void SequenceReset()
    {
        // We reset each xylophone keys spawn delay timer to what it was originally set to in the inspector.
        foreach(XylophoneHit xyKey in xyKeys)
        { 
            xyKey.spawnDelay = xyKey.spawnDelayStart;
        }
    }
}
