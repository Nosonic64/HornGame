using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPadProgrammatic : MonoBehaviour
{
    private Vector3 startPos;
    private bool playerOn;

    [Header("Start and end markers are found in prefabs folder")]
    [Header("Leave empty for an idle, unmoving lily pad")]
    public Transform startMarker;
    public Transform endMarker;
    [Header("Speed the lilypad moves between points")]
    public float moveSpeed;
    [Header("Rate at which the lilypad sinks")]
    public float sinkSpeed;
    [Header("Rate at which lilypad comes back up")]
    public float floatSpeed;
    private float startTime;
    private float journeyLength;
    private bool atStart;
    private bool atEnd;


    private void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
        // If we have both a start and end marker, we can allow the lilypad to move
        if (startMarker != null && endMarker != null)
        {
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
            atStart = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        // Parent the players transform to the lilypads so that they move with the platform
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.parent.parent = this.gameObject.transform;
            playerOn = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        // While the player stands on the platform, it will continually move down
        if (col.gameObject.tag == "Player")
        {
            transform.position -= new Vector3(0f, sinkSpeed, 0f) * Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        // Unparent the player from the platform when they leave
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.parent.parent = null;
            playerOn = false;
        }
    }

    private void Update()
    {
        // If the current position of the lilypads Y is lower than its start position Y, and the player isnt on the lilypad, we continually move the lilypad
        // up until its at its start Y position again.
        if (transform.position.y < startPos.y && !playerOn)
        {
            transform.position += new Vector3(0f, floatSpeed, 0f) * Time.deltaTime;
        }


        // Most of this code is taken directly from https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
        // We get the lilypad to move between two markers placed in the editor
        if (atStart)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.parent.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            if(transform.parent.position == endMarker.position)
            {
                atStart = false;
                startTime = Time.time;
                atEnd = true;
            }
        }

        if (atEnd)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.parent.position = Vector3.Lerp(endMarker.position, startMarker.position, fractionOfJourney);
            if (transform.parent.position == startMarker.position)
            {
                atEnd = false;
                startTime = Time.time;
                atStart = true;
            }
        }
    }
}
