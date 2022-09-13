using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointHandler : MonoBehaviour
{
    public Vector3 currentCheckpointLocation;

    private void Awake()
    {
        currentCheckpointLocation = Vector3.zero;
    }
}
