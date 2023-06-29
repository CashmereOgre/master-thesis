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
        if(plant != null)
        {
            plant.trunk.drawBranch();
        }
    }

    private void initializePlant()
    {
        Node rootNode = new Node() { isRoot = true, position = Vector3.zero, radius = 0.0f };
        Node terminalNode = new Node() { isRoot = false, position = new Vector3(0.0f, 1.0f, 0.0f), radius = 0.0f };
        BranchSegment trunkBase = new BranchSegment() { branchSegmentBase = rootNode, branchSegmentEnd = terminalNode };

        Node child1BaseNode = new Node() { isRoot = false, position = new Vector3(0.0f, 1.0f, 0.0f), radius = 0.0f };
        Node child1TerminalNode = new Node() { isRoot = false, position = new Vector3(1.0f, 2.0f, 0.0f), radius = 0.0f };
        BranchSegment child1Branch = new BranchSegment() { branchSegmentBase = child1BaseNode, branchSegmentEnd = child1TerminalNode };

        Node child1childBaseNode = new Node() { isRoot = false, position = new Vector3(1.0f, 2.0f, 0.0f), radius = 0.0f };
        Node child1childTerminalNode = new Node() { isRoot = false, position = new Vector3(1.5f, 2.0f, 1.0f), radius = 0.0f };
        BranchSegment child1childBranch = new BranchSegment() { branchSegmentBase = child1childBaseNode, branchSegmentEnd = child1childTerminalNode };
        Branch child1child = new Branch() { branchBase = child1childBranch };

        Branch child1 = new Branch() { branchBase = child1Branch, childBranches = new List<Branch>() { child1child } };

        Node child2BaseNode = new Node() { isRoot = false, position = new Vector3(0.0f, 1.0f, 0.0f), radius = 0.0f };
        Node child2TerminalNode = new Node() { isRoot = false, position = new Vector3(2.0f, 1.0f, 0.0f), radius = 0.0f };
        BranchSegment child2Branch = new BranchSegment() { branchSegmentBase = child2BaseNode, branchSegmentEnd = child2TerminalNode };
        Branch child2 = new Branch() { branchBase = child2Branch };

        Branch trunk = new Branch() { branchBase = trunkBase, childBranches = new List<Branch>() { child1, child2 } };

        plant = new Plant(trunk);
    }
}
