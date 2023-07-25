using Assets.Scripts.HelpfulStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Node rootNode = new Node(NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(0));
        rootNode.nodeGameObject = rootNode.instantiateNode(null);
        rootNode.nodeGameObject.name = "Root";
        NodesLookupTable.nodesDictionary.Add(rootNode.id, rootNode);

        Node terminalNode = new Node(NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(1));
        terminalNode.nodeGameObject = terminalNode.instantiateNode(rootNode.nodeGameObject.transform);
        terminalNode.nodeGameObject.name = "Trunk";
        terminalNode.branchLineRenderer = terminalNode.setBranchLineRenderer();
        NodesLookupTable.nodesDictionary.Add(terminalNode.id, terminalNode);

        var trunk = rootNode.nodeGameObject.gameObject.GetComponent<Branch>();
        trunk.prototype = BranchPrototypesInstances.basicBranchPrototype;
        trunk.maxAge = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0).maxAge;
        trunk.currentAge = 0.0f;
        trunk.rootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(0);
        trunk.terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(1);
        trunk.childBranches = new List<Branch>();

        trunk.boundingSphere = trunk.setBoundingSphere();

        plant = new Plant(trunk);
    }

    private void Update()
    {
        //if (Input.GetKeyDown("space"))
        //{
            plant.trunk.GrowBranch(0.1f);
        //}
    }
}
