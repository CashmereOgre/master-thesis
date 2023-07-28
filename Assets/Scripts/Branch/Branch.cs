using Assets.Scripts.HelpfulStructures;
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

    public SphereCollider boundingSphere;
    public float lightExposure = 0;

    private bool stop = false;

    private Dictionary<int, Vector3> childBranchesTerminalNodesIdsAndRootPositions = new Dictionary<int, Vector3>();
    private Dictionary<int, CollisionInfo> collidersDictionary = new Dictionary<int, CollisionInfo>();

    private float intersectionsSum = 0;
    private float vigor = 0;

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

            //childBranchesTerminalNodesIdsAndRootPositions = getAllInDepthChildBranchesPositions(); //Will see if it is necessary at all

            if (boundingSphere != null)
            {
                Vector3 branchCenterOfGeometry = getBranchCenterOfGeometry();
                Vector3 localizedBranchCenterOfGeometry = terminalNode.nodeGameObject.transform.InverseTransformPoint(branchCenterOfGeometry);
                float boundingSphereRadius = Vector3.Distance(rootNode.nodeGameObject.transform.position, branchCenterOfGeometry);
                boundingSphere = updateBoundingSpherePositionAndRadius(localizedBranchCenterOfGeometry, boundingSphereRadius);
            }

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
        newNode.nodeGameObject = newNode.instantiateNode(newBranchRootNode.nodeGameObject.transform);
        newNode = newNode.nodeGameObject.GetComponent<Node>();
        newNode.id = newNodeId;
        newNode.position = branchPrototypeTerminalNode.position;
        newNode.rotation = branchPrototypeTerminalNode.rotation;
        newNode.age = branchPrototypeTerminalNode.age;
        newNode.maxLength = branchPrototypeTerminalNode.maxLength;
        newNode.plantVariables = branchPrototypeTerminalNode.plantVariables;
        newNode.parentNodeId = rootNodeId;  
        newNode.childNodeIds = branchPrototypeTerminalNode.childNodeIds;
        newNode.nodeGameObject.transform.localRotation = newNode.rotation;
        newNode.nodeGameObject.name = newNode.id.ToString();
        newNode.branchLineRenderer = newNode.setBranchLineRenderer();
        NodesLookupTable.nodesDictionary.Add(newNodeId, newNode);

        Branch childBranch = newNode.nodeGameObject.GetComponent<Branch>();
        childBranch.prototype = BranchPrototypesInstances.basicBranchPrototype;
        childBranch.maxAge = maxAge; //set other maxAge, now is max age of whole tree
        childBranch.currentAge = 0.0f;
        childBranch.rootNode = newBranchRootNode;
        childBranch.terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(newNodeId); 
        childBranch.childBranches = new List<Branch>();

        if (boundingSphere == null)
        {
            childBranch.boundingSphere = childBranch.setBoundingSphere();
        }

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
                childDictionary.Add(child.terminalNode.id, child.terminalNode.nodeGameObject.transform.position);
            }

            int i = 0;
            foreach (KeyValuePair<int, Vector3> position in childDictionary)
            {
                center = center + position.Value + terminalNode.nodeGameObject.transform.position + rootNode.nodeGameObject.transform.position;
                i++;
            }

            center = center / ((childDictionary.Count + 1) * 2);

            return center;
        }

        center = (terminalNode.nodeGameObject.transform.position + rootNode.nodeGameObject.transform.position) / 2;

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

        result.Add(terminalNode.id, terminalNode.nodeGameObject.transform.position);

        return result;
    }

    public SphereCollider setBoundingSphere()
    {
        var collider = terminalNode.nodeGameObject.GetComponent<SphereCollider>();

        return collider;
    }

    public float calculateLightExposure()
    {
        intersectionsSum = 0;
        lightExposure = 0;

        if(terminalNode.id != 1)
        {
            foreach (KeyValuePair<int, CollisionInfo> collisionInfo in collidersDictionary)
            {
                intersectionsSum += collisionInfo.Value.volumeOfCollision;
            }

            float currentBranchLightExposure = Mathf.Exp(-intersectionsSum);
            lightExposure += currentBranchLightExposure;
        }

        if (childBranches.Any())
        {
            foreach (Branch childBranch in childBranches)
            {
                lightExposure += childBranch.calculateLightExposure();
            }
        }

        return lightExposure;
    }

    public void distributeVigor(float vigorObtained)
    {
        if (!childBranches.Any())
        {
            vigor = vigorObtained;
            return;
        }

        Branch mainChildBranch = getMainChildBranch();
        List<float> lateralBranchesLightExposure = getLateralBranchesLightExposure();
        float lateralBranchesLightExposureSum = lateralBranchesLightExposure.Sum();

        float apicalControl = terminalNode.plantVariables.apicalControl;
        float vigorToMain = vigorObtained * ((apicalControl * mainChildBranch.lightExposure) /
            (apicalControl * mainChildBranch.lightExposure) + (1 - apicalControl) * lateralBranchesLightExposureSum);
    }

    private Branch getMainChildBranch()
    {
        foreach (Branch childBranch in childBranches)
        {
            if (childBranch.terminalNode.isMain)
                return childBranch;
        }

        return null;
    }

    private List<float> getLateralBranchesLightExposure()
    {
        List<float> result = new List<float>();
        foreach (Branch childBranch in childBranches)
        {
            if (!childBranch.terminalNode.isMain)
                result.Add(childBranch.lightExposure);
        }

        return result;
    }

    private SphereCollider updateBoundingSpherePositionAndRadius(Vector3 position, float radius)
    {
        boundingSphere.center = position;
        boundingSphere.radius = radius;

        return boundingSphere;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (boundingSphere != null)
        {
            if (collision.collider.gameObject.name == "Root" || collision.collider.gameObject.name == "Trunk")
                return;

            int collidingObjectId = int.Parse(collision.collider.gameObject.name);

            SphereCollider collider = collision.collider.GetComponent<SphereCollider>();
            float r1 = boundingSphere.radius;
            float r2 = collider.radius;
            Vector3 boundingSphereCenter = boundingSphere.bounds.center;
            Vector3 colliderCenter = collision.collider.gameObject.transform.TransformPoint(collider.center);

            float distance = Vector3.Distance(boundingSphereCenter, colliderCenter);

            if (distance == 0)
                return;

            float term1 = Mathf.Pow(r1 + r2 - distance, 2);
            float term2 = distance * distance + 2 * distance * r2 - 3 * r2 * r2 + 2 * distance * r1 + 6 * r1 * r2 - 3 * r1 * r1;
            float volumeOfIntersection = Mathf.PI * term1 * term2 / (12 * distance);

            if (volumeOfIntersection < 0)
                return;

            collidersDictionary[collidingObjectId] = new CollisionInfo(volumeOfIntersection, colliderCenter, r2, distance);

            //Debug.Log($"Current object id: {boundingSphere}, colliding object id: {collidingObjectId}, volume: {volumeOfIntersection}");
        }
    }
}
