using Assets.Scripts.HelpfulStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class PlantComponent : MonoBehaviour
{
    public Plant plant { get; private set; }

    public GameObject nodePrefab;

    void Start()
    {
        initializePlant();
    }

    private void initializePlant()
    {
        BranchPrototypesInstances.Setup(nodePrefab);

        Node rootNodePrototype = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(0);
        Node rootNode = new Node(rootNodePrototype);
        rootNode.nodeGameObject = rootNode.instantiateNode(null);
        rootNode = rootNode.nodeGameObject.GetComponent<Node>();
        rootNode.id = rootNodePrototype.id;
        rootNode.isMain = rootNodePrototype.isMain;
        rootNode.position = rootNodePrototype.position;
        rootNode.rotation = rootNodePrototype.rotation;
        rootNode.age = rootNodePrototype.age;
        rootNode.physiologicalAge = rootNodePrototype.physiologicalAge;
        rootNode.maxLength = rootNodePrototype.maxLength;
        rootNode.plantVariables = rootNodePrototype.plantVariables;
        rootNode.parentNodeId = rootNodePrototype.parentNodeId;
        rootNode.childNodeIds = rootNodePrototype.childNodeIds;
        rootNode.nodeGameObject.transform.localRotation = rootNode.rotation;
        rootNode.nodeGameObject.name = "Root";
        rootNode.nodeGameObject.transform.position = new Vector3(10f, 0f, 10f);
        NodesLookupTable.nodesDictionary.Add(rootNode.id, rootNode);

        Node terminalNodePrototype = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(1);
        Node terminalNode = new Node(terminalNodePrototype);
        terminalNode.nodeGameObject = terminalNode.instantiateNode(rootNode.nodeGameObject.transform);
        terminalNode = terminalNode.nodeGameObject.GetComponent<Node>();
        terminalNode.id = terminalNodePrototype.id;
        terminalNode.isMain = terminalNodePrototype.isMain;
        terminalNode.position = terminalNodePrototype.position;
        terminalNode.rotation = terminalNodePrototype.rotation;
        terminalNode.age = terminalNodePrototype.age;
        terminalNode.physiologicalAge = terminalNodePrototype.physiologicalAge;
        terminalNode.maxLength = terminalNodePrototype.maxLength;
        terminalNode.plantVariables = terminalNodePrototype.plantVariables;
        terminalNode.parentNodeId = terminalNodePrototype.parentNodeId;
        terminalNode.childNodeIds = terminalNodePrototype.childNodeIds;
        terminalNode.nodeGameObject.transform.rotation = terminalNode.rotation;
        terminalNode.nodeGameObject.name = "Trunk";
        terminalNode.branchLineRenderer = terminalNode.setBranchLineRenderer();
        NodesLookupTable.nodesDictionary.Add(terminalNode.id, terminalNode);

        Branch trunk = terminalNode.nodeGameObject.GetComponent<Branch>();
        trunk.prototype = BranchPrototypesInstances.basicBranchPrototype;
        trunk.maxAge = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0).maxAge;
        trunk.currentAge = 0.0f;
        trunk.rootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(0);
        trunk.terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(1);
        trunk.childBranches = new List<Branch>();

        //trunk.boundingSphere = trunk.setBoundingSphere();
        trunk.capsuleCollider = trunk.setCapsuleCollider();

        plant = new Plant(trunk);
    }

    private void FixedUpdate()
    {
        //if (Input.GetKeyDown("space"))
        //{

        //plant.totalLightExposure = plant.trunk.calculateLightExposure();
        //Debug.Log(plant.totalLightExposure);

        //float vigor = plant.totalLightExposure <= plant.plantSpecies.vigorMax ? plant.totalLightExposure : plant.plantSpecies.vigorMax;
        //plant.trunk.distributeVigor(vigor);

        plant.trunk.GrowBranch(0.1f);
        //}
    }
}
