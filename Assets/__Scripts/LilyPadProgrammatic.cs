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
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        if (startMarker != null && endMarker != null)
        {
            atStart = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.parent = this.gameObject.transform;
            playerOn = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            transform.position -= new Vector3(0f, sinkSpeed, 0f) * Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.parent = null;
            playerOn = false;
        }
    }

    private void Update()
    {
        if (transform.position.y < startPos.y && !playerOn)
        {
            transform.position += new Vector3(0f, floatSpeed, 0f) * Time.deltaTime;
        }

        

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
