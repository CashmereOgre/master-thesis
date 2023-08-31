using Assets.Scripts.HelpfulStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class PlantComponent : MonoBehaviour
{
    public Plant plant { get; private set; }

    public GameObject nodePrefab;

    public Raycaster raycaster;

    private int squareHeight = 200;
    private int squareDimension = 30;
    private int sideDensity = 100;

    void Start()
    {
        raycaster.setRaysSquare(squareDimension, sideDensity, squareHeight);
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
        rootNode.parentNodeId = rootNodePrototype.parentNodeId;
        rootNode.childNodeIds = rootNodePrototype.childNodeIds;
        rootNode.nodeGameObject.transform.localRotation = rootNode.rotation;
        rootNode.nodeGameObject.name = "Root";
        Destroy(rootNode.nodeGameObject.GetComponent<CapsuleCollider>());
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

        trunk.capsuleCollider = trunk.setCapsuleCollider();

        PlantSpecies plantSpecimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0));

        plant = new Plant(trunk, plantSpecimen);

        rootNode.plant = plant;
        terminalNode.plant = plant;
    }

    private void FixedUpdate()
    {
        if (plant != null)
        {
            RaycastCollisionsLookupTable.objectRayCountDictionary = raycaster.CastRaysSquare();

            plant.totalLightExposure = plant.trunk.calculateLightExposure();
            //add full tree decay if vigor < vigorMin
            plant.age += 1f;

            float vigor = setTreeVigor(plant.totalLightExposure, 1f);

            if (vigor <= plant.plantSpecies.vigorMin)
            {
                destroyTree(plant);
                return;
            }

            plant.trunk.distributeVigor(vigor);
            plant.trunk.GrowBranch(1000f);
        }
    }

    private float setTreeVigor(float totalLightExposure, float vigorDecreaseStep)
    {
        if (plant.age >= plant.plantSpecies.maxAge)
        {
            plant.plantSpecies.vigorMax -= vigorDecreaseStep;
        }

        return totalLightExposure <= plant.plantSpecies.vigorMax ? plant.totalLightExposure : plant.plantSpecies.vigorMax;
    }

    private void destroyTree(Plant plant)
    {
        Destroy(plant.trunk.terminalNode.gameObject);
        Destroy(plant.trunk.rootNode.gameObject);
        Destroy(this);
    }
}
