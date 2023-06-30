using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Node : MonoBehaviour
{
    public bool isRoot { get; set; }
    public Vector3 position { get; set; }
    public float radius { get; set; }

    private ProBuilderMesh mesh;

    public void Start()
    {
        mesh = GetComponent<ProBuilderMesh>();
    }


    public void CreateNodeSphere()
    {

    }
}
