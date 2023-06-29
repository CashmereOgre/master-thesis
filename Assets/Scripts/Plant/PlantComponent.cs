using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantComponent : MonoBehaviour
{
    public Plant plant { get; private set; }

    void Start()
    {
        initializePlant();
    }

    private void OnDrawGizmos()
    {
        
    }

    private void initializePlant()
    {
        Node rootNode = new Node() { isRoot = true, position = Vector3.zero, radius = 0.0f };
        Node terminalNode = new Node() { isRoot = false, position = new Vector3(0.0f, 1.0f, 0.0f), radius = 0.0f };
        BranchSegment trunkBase = new BranchSegment() { branchSegmentBase = rootNode, branchSegmentEnd = terminalNode };
        Branch trunk = new Branch() { branchBase = trunkBase };

        plant = new Plant(trunk);
    }
}
