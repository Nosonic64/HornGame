using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMesh : MonoBehaviour
{
    private MeshFilter mesh;
    public Mesh[] meshes = new Mesh[0];
    private int i;
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>();
        i = Random.Range(0,5);
        mesh.mesh = meshes[i];
    }
}
