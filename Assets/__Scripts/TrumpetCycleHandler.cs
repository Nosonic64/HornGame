using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrumpetCycleHandler : MonoBehaviour
{
    public FlingScript[] trumpets = new FlingScript[0];
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        // When timer hits a certain number, we set it back to 0 and for each trumpet in the array we set off their "TrumpetShot" Coroutine.
        if(timer >= 4f)
        {
            timer = 0f;
            foreach(FlingScript trumpet in trumpets)
            {
               StartCoroutine(trumpet.TrumpetShot());
            }
        }
    }
}
