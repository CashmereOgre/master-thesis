using Assets.Scripts.HelpfulStructures;
using System;
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
    public CapsuleCollider capsuleCollider;
    public float lightExposure = 0;

    private bool stop = false;

    private Dictionary<int, Vector3> childBranchesTerminalNodesIdsAndRootPositions = new Dictionary<int, Vector3>();
    private Dictionary<string, Vector3> collisionsDictionary = new Dictionary<string, Vector3>();

    private float vigor = 0;

    public void GrowBranch(float ageStep)
    {
        if (stop)
            return;

        if (prototype != null)
        {
            terminalNode.age += ageStep;

            // TODO to improve when adding branches shredding and blooming
            //bool isBecomingMature = newAge >= prototype.maturityAge ? rootNode.age < prototype.maturityAge : false;
            //bool decay = rootNode.age >= prototype.maturityAge;

            childBranchesTerminalNodesIdsAndRootPositions = getAllInDepthChildBranchesPositions(); //Will see if it is necessary at all

            capsuleCollider = updateCapsuleColliderDimensions();

            //Debug.Log($"Terminal node id: {terminalNode.id}, terminal node position: {terminalNode.nodeGameObject.transform.position}, center of geometry: {getBranchCenterOfGeometry()}");

            if (terminalNode.physiologicalAge < prototype.maturityAge)
            {
                float physiologicalAge = getPhysiologicalAge(ageStep);
                Quaternion optimisedBranchOrientation = getOptimisedBranchOrientation(terminalNode.rotation, terminalNode.plant.plantSpecies.alphaTropism);
                terminalNode.nodeGameObject = terminalNode.growNode(physiologicalAge, optimisedBranchOrientation);
                return;
            }

            if (childBranches.Any())
            {
                foreach (Branch childBranch in childBranches)
                {
                    childBranch.GrowBranch(ageStep);
                }
                return;
            }

            if (vigor > terminalNode.plant.plantSpecies.vigorMin)
            {
                foreach (Node prototypeTerminalNode in prototype.terminalNodes)
                {
                    int lookupTableLastKey = NodesLookupTable.nodesDictionary.Last().Key;

                    Branch childBranch = AttachBranch(terminalNode.id, lookupTableLastKey + 1, prototypeTerminalNode, terminalNode.plant);
                    childBranches.Add(childBranch);
                }
            }
        }
    }

    private Branch AttachBranch(int rootNodeId, int newNodeId, Node branchPrototypeTerminalNode, Plant plant)
    {
        Node newBranchRootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(rootNodeId);

        Node newNode = new Node(branchPrototypeTerminalNode);
        newNode.nodeGameObject = newNode.instantiateNode(newBranchRootNode.nodeGameObject.transform);
        newNode = newNode.nodeGameObject.GetComponent<Node>();
        newNode.id = newNodeId;
        newNode.isMain = branchPrototypeTerminalNode.isMain;
        newNode.position = branchPrototypeTerminalNode.position;
        newNode.rotation = branchPrototypeTerminalNode.rotation;
        newNode.age = branchPrototypeTerminalNode.age;
        newNode.physiologicalAge = branchPrototypeTerminalNode.physiologicalAge;
        newNode.maxLength = branchPrototypeTerminalNode.maxLength;
        newNode.plant = plant;
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

        childBranch.capsuleCollider = childBranch.setCapsuleCollider();

        return childBranch;
    }

    private Vector3 getBranchCenterOfGeometry()
    {
        Vector3 center = (terminalNode.nodeGameObject.transform.position + rootNode.nodeGameObject.transform.position) / 2;

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

    public CapsuleCollider setCapsuleCollider()
    {
        var collider = terminalNode.nodeGameObject.GetComponent<CapsuleCollider>();

        return collider;
    }

    public float calculateLightExposure()
    {
        float currentBranchLightExposure = 0;
        lightExposure = 0;

        if (RaycastCollisionsLookupTable.objectRayCountDictionary.TryGetValue(gameObject.name, out int rays))
        {
            currentBranchLightExposure = rays / capsuleCollider.height;
        }
        
        lightExposure += currentBranchLightExposure;

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
        vigor = vigorObtained;

        if (!childBranches.Any())
        {
            return;
        }

        Branch mainChildBranch = getMainChildBranch();
        Dictionary<int, float> branchesLightExposure = getLateralBranchesLightExposure();
        float lateralBranchesLightExposureSum = 0;

        foreach (KeyValuePair<int, float> lateralBranch in branchesLightExposure)
        {
            lateralBranchesLightExposureSum += lateralBranch.Value;
        }

        float apicalControl = terminalNode.plant.plantSpecies.apicalControl;
        float vigorToMain = 0;
        float vigorToLateral = 0;

        if (mainChildBranch == null)
        {
            if (lateralBranchesLightExposureSum > 0)
            {
                vigorToLateral = vigorObtained;
            }
        }
        else
        {
            if (lateralBranchesLightExposureSum <= 0)
            {
                if (mainChildBranch.lightExposure > 0)
                {
                    vigorToMain = vigorObtained;
                }
            }
            else
            {
                vigorToMain = vigorObtained * ((apicalControl * mainChildBranch.lightExposure) /
                    ((apicalControl * mainChildBranch.lightExposure) + ((1 - apicalControl) * lateralBranchesLightExposureSum)));
                vigorToLateral = vigorObtained - vigorToMain;
            }
        }


        foreach (Branch childBranch in childBranches.ToList())
        {
            float vigorMin = childBranch.terminalNode.plant.plantSpecies.vigorMin;

            if (childBranch.terminalNode.isMain)
            {
                if(vigorToMain < vigorMin)
                {
                    destroyChildBranch(childBranch);
                    continue;
                }
                childBranch.distributeVigor(vigorToMain);
                continue;
            }

            float childBranchLightExposure = branchesLightExposure.GetValueOrDefault(childBranch.terminalNode.id);
            float childBranchVigor;

            if (lateralBranchesLightExposureSum <= 0)
            {
                childBranchVigor = 0;
            }
            else
            {
                childBranchVigor = vigorToLateral * (childBranchLightExposure / lateralBranchesLightExposureSum);
            }

            if (childBranchVigor < vigorMin)
            {
                destroyChildBranch(childBranch);
                continue;
            }

            childBranch.distributeVigor(childBranchVigor);
        }
    }

    private float getGrowthRate()
    {
        float vigorMin = terminalNode.plant.plantSpecies.vigorMin;
        float vigorMax = terminalNode.plant.plantSpecies.vigorMax;
        float x = (vigor - vigorMin) / (vigorMax - vigorMin);

        return (3 * Mathf.Pow(x, 2) - 2 * Mathf.Pow(x, 3)) * terminalNode.plant.plantSpecies.gp;
    }

    private float getPhysiologicalAge(float timeStep)
    {
        return terminalNode.physiologicalAge + (timeStep * getGrowthRate());
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

    private Dictionary<int, float> getLateralBranchesLightExposure()
    {
        Dictionary<int, float> result = new Dictionary<int, float>();
        foreach (Branch childBranch in childBranches)
        {
            if (!childBranch.terminalNode.isMain)
                result.Add(childBranch.terminalNode.id, childBranch.lightExposure);
        }

        return result;
    }

    private CapsuleCollider updateCapsuleColliderDimensions()
    {
        Vector3 branchCenterOfGeometry = getBranchCenterOfGeometry();
        Vector3 localizedBranchCenterOfGeometry = terminalNode.nodeGameObject.transform.InverseTransformPoint(branchCenterOfGeometry);
        float capsuleColliderHeight  = Vector3.Distance(rootNode.nodeGameObject.transform.position, terminalNode.nodeGameObject.transform.position);

        capsuleCollider.center = localizedBranchCenterOfGeometry;
        capsuleCollider.height = capsuleColliderHeight > 1 ? capsuleColliderHeight : 1;

        return capsuleCollider;
    }

    private void destroyChildBranch(Branch childBranch)
    {
        childBranches.Remove(childBranch);
        Destroy(childBranch.terminalNode.gameObject);
    }

    private Quaternion getOptimisedBranchOrientation(Quaternion startingRotation, float alphaTropism)
    {
        Vector3 startingRotationEuler = startingRotation.eulerAngles;
        float alpha = 5f;

        return optimize(startingRotationEuler, alpha, alphaTropism);
    }

    private Quaternion optimize(Vector3 rotation, float alpha, float alphaTropism)
    {
        float minQuality = float.MaxValue;
        Vector3 optimalAngle = rotation;

        Vector3[] optimisationAngles = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(alpha, 0f, 0f),
            new Vector3(-alpha, 0f, 0f),
            new Vector3(0f, 0f, alpha),
            new Vector3(0f, 0f, -alpha)
        };

        foreach(Vector3 optimisationAngle in optimisationAngles)
        {
            Vector3 newAngle = rotation + optimisationAngle;
            float quality = fDistribution(alphaTropism, Quaternion.Euler(newAngle));

            if(quality < minQuality)
            {
                minQuality = quality;
                optimalAngle = newAngle;
            }
        }

        return Quaternion.Euler(optimalAngle);
    }

    private float fDistribution(float alphaTropism, Quaternion alpha)
    {
        return terminalNode.plant.plantSpecies.w1 * fCollisions(collisionsDictionary) + terminalNode.plant.plantSpecies.w2 * fTropism(alphaTropism, alpha);
    }

    private float fCollisions(Dictionary<string, Vector3> collisionPoints)
    {
        float fCollisions = 0;

        foreach (KeyValuePair<string, Vector3> collisionPoint in collisionPoints)
        {
            fCollisions += Vector3.Distance(collisionPoint.Value, terminalNode.nodeGameObject.transform.position);
        }

        return fCollisions;
    }

    private float fTropism(float alphaTropism, Quaternion alpha)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(alpha);
        float cosAlpha = rotationMatrix.m00 + rotationMatrix.m11 + rotationMatrix.m22;
        float cosAlphaTropism = Mathf.Cos(alphaTropism);

        return Mathf.Abs(cosAlphaTropism - cosAlpha);
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = new ContactPoint[20];
        int contactPointsCount = collision.GetContacts(contactPoints);

        for (int i = 0; i < contactPointsCount; i++)
        {
            string collidingObjectName = contactPoints[i].thisCollider.gameObject.name;

            if (collisionsDictionary.ContainsKey(collidingObjectName))
            {
                collisionsDictionary[collidingObjectName] = contactPoints[i].point;
                continue;
            }

            collisionsDictionary.Add(collidingObjectName, contactPoints[i].point);

        }
    }
}
