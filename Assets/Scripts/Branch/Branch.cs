using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Branch: MonoBehaviour
{
    public BranchPrototype prototype { get; set; }
    public float maxAge { get; set; }
    public float currentAge { get; set; }
    public Node rootNode { get; set; }
    public Node terminalNode { get; set; }
    public List<Branch> childBranches { get; set; }

    private Dictionary<int, Vector3> childBranchesTerminalNodesIdsAndRootPositions = new Dictionary<int, Vector3>();

    public SphereCollider boundingSphere;

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
            boundingSphere = updateBoundingSpherePositionAndRadius(localizedBranchCenterOfGeometry, boundingSphereRadius);

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

        Node newNode = new Node(branchPrototypeTerminalNode);
        newNode.id = newNodeId;
        newNode.parentNodeId = rootNodeId;  
        newNode.nodeGameObject = newNode.instantiateNode(newBranchRootNode.nodeGameObject.transform);
        newNode.nodeGameObject.transform.localRotation = newNode.rotation;
        newNode.nodeGameObject.name = newNode.id.ToString();
        newNode.branchLineRenderer = newNode.setBranchLineRenderer();
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

        childBranch.boundingSphere = childBranch.setBoundingSphere();

        return childBranch;
    }

    private Vector3 getBranchCenterOfGeometry()
    {
        Vector3 center = Vector3.zero;
        var childDictionary = new Dictionary<int, Vector3>();


        if (childBranches.Any()) 
        { 
            foreach (Branch child in childBranches)
            {
                childDictionary.AddRange(child.getAllInDepthChildBranchesPositions());
            }

            foreach (KeyValuePair<int, Vector3> position in childDictionary)
            {
                center = center + position.Value;
            }

            childBranchesTerminalNodesIdsAndRootPositions = childDictionary;
        }

        center = center + terminalNode.nodeGameObject.transform.position;

        center = center / (childDictionary.Count + 1);

        return center;
    }

    private Dictionary<int,Vector3> getAllInDepthChildBranchesPositions()
    {
        Dictionary<int, Vector3> result = new Dictionary<int, Vector3>();
        if (childBranches.Any())
        {
            foreach (Branch child in childBranches)
            {
                result.AddRange(child.getAllInDepthChildBranchesPositions());
            }
        }

        result.Add(terminalNode.id, rootNode.nodeGameObject.transform.position);

        return result;
    }

    public SphereCollider setBoundingSphere()
    {
        var collider = terminalNode.nodeGameObject.GetComponent<SphereCollider>();

        return collider;
    }

    private SphereCollider updateBoundingSpherePositionAndRadius(Vector3 position, float radius)
    {
        boundingSphere.center = position;
        boundingSphere.radius = radius;

        return boundingSphere;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (childBranchesTerminalNodesIdsAndRootPositions.ContainsKey(collision.collider.gameObject.GetComponent<Node>().id))
            //TODO: make it better, without getComponent
            return;

    }
}
