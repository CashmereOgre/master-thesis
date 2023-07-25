using Assets.Scripts.HelpfulStructures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Branch: MonoBehaviour
{
    public BranchPrototype prototype;
    public float maxAge;
    public float currentAge;
    public Node rootNode;
    public Node terminalNode;
    public List<Branch> childBranches;

    public SphereCollider boundingSphere;

    private bool stop = false;

    private Dictionary<int, Vector3> childBranchesTerminalNodesIdsAndRootPositions = new Dictionary<int, Vector3>();

    private Dictionary<int, CollisionInfo> collidersDictionary = new Dictionary<int, CollisionInfo>();

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


        var childBranch = newNode.nodeGameObject.gameObject.GetComponent<Branch>();
        childBranch.prototype = BranchPrototypesInstances.basicBranchPrototype;
        childBranch.maxAge = maxAge;//set other maxAge, now is max age of whole tree
        childBranch.currentAge = 0.0f;
        childBranch.rootNode = newBranchRootNode;
        childBranch.terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(1);
        childBranch.childBranches = new List<Branch>();

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
        if(terminalNode != null)
        {
            if (collision.collider.gameObject.name == "Root" || collision.collider.gameObject.name == "Trunk")
                return;

            int collidingObjectId = int.Parse(collision.collider.gameObject.name);
            int collisionPointsCount = collision.contactCount;

            if (childBranchesTerminalNodesIdsAndRootPositions.ContainsKey(collidingObjectId) ||
                (collidersDictionary.ContainsKey(collidingObjectId) &&
                collidersDictionary[collidingObjectId].collisionPointsCount == collision.contactCount))
                return;

            SphereCollider collider = collision.collider.GetComponent<SphereCollider>();
            float r1 = boundingSphere.radius;
            float r2 = collider.radius;
            Vector3 boundingSphereCenter = terminalNode.nodeGameObject.transform.TransformPoint(boundingSphere.center);
            Vector3 colliderCenter = collision.collider.gameObject.transform.TransformPoint(collider.center);

            float distance = Vector3.Distance(boundingSphereCenter, colliderCenter);
            float heightOfIntersection = r1 + r2 - distance;
            float radiusOfIntersection = Mathf.Pow(Mathf.Pow(r1, 2) - Mathf.Pow(heightOfIntersection - r2, 2), 0.5f);
            float volumeOfIntersection = (1 / 6) * Mathf.PI * heightOfIntersection * (3 * Mathf.Pow(radiusOfIntersection, 2) + Mathf.Pow(heightOfIntersection, 2));

            collidersDictionary[collidingObjectId] = new CollisionInfo(volumeOfIntersection, collisionPointsCount);

            Debug.Log($"Id: {collidingObjectId}, volume: {volumeOfIntersection}, count: {collisionPointsCount}");
        }
    }
}
