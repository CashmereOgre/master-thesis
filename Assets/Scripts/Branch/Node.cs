using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    public int id { get; set; }
    public bool isRoot { get; set; }
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }
    public float age { get; set; }
    public float maxLength { get; set; }
    public PlantSpecies plantVariables { get; set; }
    public int? parentNodeId { get; set; }
    public List<int> childNodeIds { get; set; }

    public GameObject nodeGameObject;

    public Node() {}

    public Node(Node branchPrototypeTerminalNode)
    {
        id = branchPrototypeTerminalNode.id;
        isRoot = branchPrototypeTerminalNode.isRoot;
        position = branchPrototypeTerminalNode.position;
        rotation = branchPrototypeTerminalNode.rotation;
        age = branchPrototypeTerminalNode.age;
        maxLength = branchPrototypeTerminalNode.maxLength;
        plantVariables = branchPrototypeTerminalNode.plantVariables;
        parentNodeId = null;
        childNodeIds = new List<int>();
        nodeGameObject = branchPrototypeTerminalNode.nodeGameObject;
    }

    public GameObject instantiateNode(Transform parentObjectTransform)
    {
        if(parentObjectTransform == null)
            return Instantiate(nodeGameObject, position, rotation);

        return Instantiate(nodeGameObject, position, rotation, parentObjectTransform);
    }

    public GameObject growNode()
    {
        float branchLength = Math.Min(maxLength, age * plantVariables.scalingCoefficientBeta);

        if (branchLength != maxLength)
        {
            // rotation is applied according to prototype and other calculations when node is created
            Vector3 branchVector = new Vector3(0.0f, branchLength, 0.0f);
            branchVector = rotation * branchVector;

            position = Vector3.zero + branchVector;

            float g1 = (float)Math.Pow(0.95f, age * plantVariables.tropismMature);
            float g2 = -plantVariables.g2;
            Vector3 gravityDirection = new Vector3(0.0f, -1.0f, 0.0f);

            Vector3 tropismOffset = (g1 * g2 * gravityDirection) / Math.Max(age + g1, 0.05f);

            position += tropismOffset * branchLength;

            nodeGameObject.transform.localPosition = position;
        }

        return nodeGameObject;
    }
}
