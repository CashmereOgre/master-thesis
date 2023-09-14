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
    public Node rootNode { get; set; }
    public Node terminalNode { get; set; }
    public List<Branch> childBranches { get; set; }

    public CapsuleCollider capsuleCollider;
    public float lightExposure = 0;

    private Dictionary<string, Vector3> collisionsDictionary = new Dictionary<string, Vector3>();

    private float vigor = 0;
    private bool createdChildBranches = false;

    public void GrowBranch(float ageStep)
    {
        capsuleCollider = updateCapsuleColliderDimensions();

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

        if (vigor > terminalNode.plant.plantSpecies.vigorMin && !createdChildBranches)
        {
            foreach (Node prototypeTerminalNode in prototype.terminalNodes)
            {
                int newNodeId = NodesLookupTable.getIdOfLastNodeInPlant(terminalNode.plant.id) + 1;

                Branch childBranch = AttachBranch(terminalNode.nodeGameObject.name, newNodeId, prototypeTerminalNode, terminalNode.plant);
                childBranches.Add(childBranch);
            }

            createdChildBranches = true;
        }
    }

    private Branch AttachBranch(string rootNodeName, int newNodeId, Node branchPrototypeTerminalNode, Plant plant)
    {
        Node newBranchRootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(rootNodeName);

        Node newNode = new Node(branchPrototypeTerminalNode);
        newNode.nodeGameObject = newNode.instantiateNode(newBranchRootNode.nodeGameObject.transform);
        newNode = newNode.nodeGameObject.GetComponent<Node>();
        newNode.id = newNodeId;
        newNode.name = $"{plant.id}.{newNode.id}";
        newNode.isMain = branchPrototypeTerminalNode.isMain;
        newNode.position = branchPrototypeTerminalNode.position;
        newNode.rotation = branchPrototypeTerminalNode.rotation;
        newNode.physiologicalAge = branchPrototypeTerminalNode.physiologicalAge;
        newNode.maxLength = branchPrototypeTerminalNode.maxLength;
        newNode.plant = plant;
        newNode.parentNodeId = newBranchRootNode.id;
        newNode.parentNodeName = rootNodeName;
        newNode.childNodeIds = branchPrototypeTerminalNode.childNodeIds;
        newNode.nodeGameObject.transform.localRotation = newNode.rotation;
        newNode.nodeGameObject.name = newNode.name;
        newNode.branchLineRenderer = newNode.setBranchLineRenderer();
        NodesLookupTable.nodesDictionary.Add(newNode.name, newNode);

        Branch childBranch = newNode.nodeGameObject.GetComponent<Branch>();
        childBranch.prototype = prototype;
        childBranch.rootNode = newBranchRootNode;
        childBranch.terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(newNode.name); 
        childBranch.childBranches = new List<Branch>();

        childBranch.capsuleCollider = childBranch.setCapsuleCollider();

        return childBranch;
    }

    private Vector3 getBranchCenterOfGeometry()
    {
        Vector3 center = (terminalNode.nodeGameObject.transform.position + rootNode.nodeGameObject.transform.position) / 2;

        return center;
    }

    public CapsuleCollider setCapsuleCollider()
    {
        var collider = terminalNode.nodeGameObject.GetComponent<CapsuleCollider>();

        return collider;
    }

    public float calculateLightExposure()
    {
        lightExposure = 0;

        if (!childBranches.Any() && RaycastCollisionsLookupTable.objectRayCountDictionary.TryGetValue(gameObject.name, out int rays))
        {
            lightExposure = rays / capsuleCollider.height;
            return lightExposure;
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
                if (vigorToMain < vigorMin)
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
