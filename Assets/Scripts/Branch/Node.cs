using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    public bool isRoot { get; set; }
    public Vector3 position { get; set; }
    public float age { get; set; }
    public float maxLength { get; set; }
    public PlantSpecies plantVariables { get; set; }
    public Node parentNode { get; set; }

    public GameObject nodeGameObject;

    public void Start()
    {
        nodeGameObject = GetComponent<GameObject>();
    }

    public void SimulationStep(float ageToAdd, bool mature, bool decay)
    {
        age += ageToAdd;

        float branchLength = Math.Min(maxLength, age * plantVariables.beta);

        //position = nodeGameObject.transform.rotation;
    }
}
