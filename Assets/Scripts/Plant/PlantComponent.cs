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

        Node rootNode = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(0);
        rootNode.nodeGameObject = rootNode.instantiateNode(null);
        NodesLookupTable.nodesDictionary.Add(rootNode.id, rootNode);

        Node terminalNode = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(1);
        terminalNode.nodeGameObject = terminalNode.instantiateNode(rootNode.nodeGameObject.transform);
        NodesLookupTable.nodesDictionary.Add(terminalNode.id, terminalNode);

        Branch trunk = new Branch()
        {
            prototype = BranchPrototypesInstances.basicBranchPrototype,
            maxAge = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0).maxAge,
            currentAge = 0.0f,
            rootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(0),
            terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(1),
            childBranches = new List<Branch>()
        };

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
