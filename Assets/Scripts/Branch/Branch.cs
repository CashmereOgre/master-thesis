using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Branch: MonoBehaviour
{
    public BranchPrototype prototype { get; set; }
    public float maxAge { get; set; }
    public float currentAge { get; set; }
    public Node rootNode { get; set; }
    public Node terminalNode { get; set; }
    public List<Branch> childBranches { get; set; }

    private List<int> childBranchesTerminalNodesIds = new List<int>();
    private bool stop = false;

    public void GrowBranch(float ageStep)
    {
        if (stop)
            return;

        if(prototype != null)
        {
            terminalNode.age += ageStep;

            // TODO to improve when adding branches shredding and blooming
            //bool isBecomingMature = newAge >= prototype.maturityAge ? rootNode.age < prototype.maturityAge : false;
            //bool decay = rootNode.age >= prototype.maturityAge;

            terminalNode.nodeGameObject = terminalNode.growNode();

            Vector3 branchCenterOfGeometry = getBranchCenterOfGeometry();
            Vector3 localizedBranchCenterOfGeometry = terminalNode.nodeGameObject.transform.InverseTransformPoint(branchCenterOfGeometry);
            float boundingSphereRadius = Vector3.Distance(rootNode.nodeGameObject.transform.position, branchCenterOfGeometry);
            terminalNode.boundingSphere = terminalNode.updateBoundingSpherePositionAndRadius(localizedBranchCenterOfGeometry, boundingSphereRadius);

            //Debug.Log($"Terminal node id: {terminalNode.id}, terminal node position: {terminalNode.nodeGameObject.transform.position}, center of geometry: {getBranchCenterOfGeometry()}");

            if (terminalNode.age >= prototype.maturityAge)
            {
                if (childBranches.Any())
                {
                    foreach (Branch childBranch in childBranches)
                    {
                        childBranch.GrowBranch(ageStep);
                    }
                    return;
                }

                foreach (Node prototypeTerminalNode in prototype.terminalNodes)
                {
                    int lookupTableLastKey = NodesLookupTable.nodesDictionary.Last().Key;

                    //if (lookupTableLastKey > 3)
                    //{
                    //    stop = true;
                    //    return;
                    //}


                    Branch childBranch = AttachBranch(terminalNode.id, lookupTableLastKey + 1, prototypeTerminalNode);
                    childBranches.Add(childBranch);
                }
            }
        }
    }

    private Branch AttachBranch(int rootNodeId, int newNodeId, Node branchPrototypeTerminalNode)
    {
        Node newBranchRootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(rootNodeId);

        childBranchesTerminalNodesIds.Add(newNodeId);

        Node newNode = new Node(branchPrototypeTerminalNode);
        newNode.id = newNodeId;
        newNode.parentNodeId = rootNodeId;  
        newNode.nodeGameObject = newNode.instantiateNode(newBranchRootNode.nodeGameObject.transform);
        newNode.nodeGameObject.transform.localRotation = newNode.rotation;
        newNode.nodeGameObject.name = newNode.id.ToString();
        newNode.branchLineRenderer = newNode.setBranchLineRenderer();
        newNode.boundingSphere = newNode.setBoundingSphere();
        NodesLookupTable.nodesDictionary.Add(newNodeId, newNode);

        Branch childBranch = new Branch()
        {
            prototype = BranchPrototypesInstances.basicBranchPrototype,
            maxAge = maxAge, //set other maxAge, now is max age of whole tree
            currentAge = 0.0f,
            rootNode = newBranchRootNode, 
            terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(newNodeId), 
            childBranches = new List<Branch>()
        };

        return childBranch;
    }

    private Vector3 getBranchCenterOfGeometry()
    {
        Vector3 center = Vector3.zero;
        List<Vector3> childBranchesPosition = new List<Vector3>();
        
        if(childBranches.Any()) 
        { 
            foreach (Branch child in childBranches)
            {
                childBranchesPosition.AddRange(child.getAllInDepthChildBranchesPositions());
            }

            foreach (Vector3 position in childBranchesPosition)
            {
                center = center + position;
            }
        }

        center = center + terminalNode.nodeGameObject.transform.position;

        center = center / (childBranchesPosition.Count + 1);

        return center;
    }

    private List<Vector3> getAllInDepthChildBranchesPositions()
    {
        List<Vector3> result = new List<Vector3>();
        if (childBranches.Any())
        {
            foreach (Branch child in childBranches)
            {
                result.AddRange(child.getAllInDepthChildBranchesPositions());
            }
        }

        result.Add(rootNode.nodeGameObject.transform.position);

        return result;
    }
}
