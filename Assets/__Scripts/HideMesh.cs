using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMesh : MonoBehaviour
{
    private MeshRenderer mesh;

    // We want things like markers and kill triggers easily placable in the editor, but at runtime we
    // disable the meshes of these things as we want them to be invisible to the player
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }
}
