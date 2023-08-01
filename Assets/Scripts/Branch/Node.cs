using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    public int id { get; set; }
    public bool isRoot { get; set; }
    public bool isMain { get; set; }
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }
    public float age { get; set; }
    public float physiologicalAge { get; set; }
    public float maxLength { get; set; }
    public PlantSpecies plantVariables { get; set; }
    public int parentNodeId { get; set; }
    public List<int> childNodeIds { get; set; }

    public GameObject nodeGameObject;

    public LineRenderer branchLineRenderer;
    private Node parentNode;
    private float growthRate;

    public Node() {}

    public Node(Node branchPrototypeTerminalNode)
    {
        id = branchPrototypeTerminalNode.id;
        isRoot = branchPrototypeTerminalNode.isRoot;
        isMain = branchPrototypeTerminalNode.isMain;
        position = branchPrototypeTerminalNode.position;
        rotation = branchPrototypeTerminalNode.rotation;
        age = branchPrototypeTerminalNode.age;
        physiologicalAge = branchPrototypeTerminalNode.physiologicalAge;
        maxLength = branchPrototypeTerminalNode.maxLength;
        plantVariables = branchPrototypeTerminalNode.plantVariables;
        parentNodeId = 0;
        childNodeIds = new List<int>();
        nodeGameObject = branchPrototypeTerminalNode.nodeGameObject;
    }

    public GameObject instantiateNode(Transform parentObjectTransform)
    {
        if(parentObjectTransform == null)
            return Instantiate(nodeGameObject);

        return Instantiate(nodeGameObject, parentObjectTransform);
    }

    public GameObject growNode(float physAge)
    {
        physiologicalAge = physAge;

        float branchLength = Math.Min(maxLength, physiologicalAge * 100 * plantVariables.scalingCoefficientBeta);

        if (branchLength != maxLength)
        {
            // Debug.Log($"id: {id}, position: {position}, age: {age}, parent node id: {parentNodeId}, child node ids: {childNodeIds}");
            // rotation is applied according to prototype and other calculations when node is created
            Vector3 branchVector = new Vector3(0.0f, branchLength, 0.0f);
            branchVector = rotation * branchVector;

            position = Vector3.zero + branchVector;

            float g1 = plantVariables.g1;
            float g2 = plantVariables.g2;
            Vector3 gravityDirection = new Vector3(0.0f, -1.0f, 0.0f);

            Vector3 tropismOffset = (g1 * g2 * gravityDirection) / (physiologicalAge * 100 + g1);

            if (float.IsNaN(tropismOffset.x))
                Debug.LogWarning("NaN");

            position += tropismOffset * branchLength;

            nodeGameObject.transform.localPosition = position;

            branchLineRenderer.SetPositions(new Vector3[2] { nodeGameObject.transform.position, parentNode.nodeGameObject.transform.position });
        }

        return nodeGameObject;
    }

    public LineRenderer setBranchLineRenderer()
    {
        var lineRenderer = nodeGameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        parentNode = new Node(NodesLookupTable.nodesDictionary[parentNodeId]); 

        return lineRenderer;
    }
}
