using Assets.Scripts.HelpfulStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant: MonoBehaviour
{
    public int id { get; set; }
    public Branch trunk { get; private set; }
    public Vector3 position { get; private set; }
    public PlantSpecies plantSpecies { get; private set; }
    public float totalLightExposure { get; set; }
    public float age { get; set; }
    public float effectiveFloweringAge { get; set; }
    public bool isSeeding { get; set; }

    public GameObject plantGameObject;

    public int numberOfLeftSideBranches = 0;
    public int numberOfRightSideBranches = 0;


    public Plant(PlantSpecies _plantSpecimen, Vector3 _position)
    {
        trunk = null;
        position = _position;
        plantSpecies = _plantSpecimen;
        totalLightExposure = 0;
        age = 0;
        effectiveFloweringAge = _plantSpecimen.floweringAge;
        isSeeding = true;
    }

    public GameObject instantiatePlant(GameObject plantPrefab)
    {
        plantGameObject = Instantiate(plantPrefab);
        return plantGameObject;
    }

    public void initializePlant()
    {
        Node rootNodePrototype = plantSpecies.branchPrototype.rootNode;
        Node rootNode = new Node(rootNodePrototype);
        rootNode.nodeGameObject = rootNode.instantiateNode(plantGameObject.transform);
        rootNode = rootNode.nodeGameObject.GetComponent<Node>();
        rootNode.id = 0;
        rootNode.name = $"{id}.{rootNode.id}";
        rootNode.isMain = rootNodePrototype.isMain;
        rootNode.position = rootNodePrototype.position;
        rootNode.rotation = getRandomPlantRotation();
        rootNode.physiologicalAge = rootNodePrototype.physiologicalAge;
        rootNode.maxLength = rootNodePrototype.maxLength;
        rootNode.parentNodeId = rootNodePrototype.parentNodeId;
        rootNode.childNodeIds = rootNodePrototype.childNodeIds;
        rootNode.nodeGameObject.transform.localRotation = rootNode.rotation;
        rootNode.nodeGameObject.name = rootNode.name;
        Destroy(rootNode.nodeGameObject.GetComponent<CapsuleCollider>());
        NodesLookupTable.nodesDictionary.Add(rootNode.nodeGameObject.name, rootNode);

        Node terminalNodePrototype = plantSpecies.branchPrototype.centerNode;
        Node terminalNode = new Node(terminalNodePrototype);
        terminalNode.nodeGameObject = terminalNode.instantiateNode(rootNode.nodeGameObject.transform);
        terminalNode = terminalNode.nodeGameObject.GetComponent<Node>();
        terminalNode.id = 1;
        terminalNode.name = $"{id}.{terminalNode.id}";
        terminalNode.isMain = terminalNodePrototype.isMain;
        terminalNode.position = terminalNodePrototype.position;
        terminalNode.rotation = terminalNodePrototype.rotation;
        terminalNode.physiologicalAge = terminalNodePrototype.physiologicalAge;
        terminalNode.maxLength = terminalNodePrototype.maxLength;
        terminalNode.parentNodeId = terminalNodePrototype.parentNodeId;
        terminalNode.parentNodeName = rootNode.name;
        terminalNode.childNodeIds = terminalNodePrototype.childNodeIds;
        terminalNode.nodeGameObject.transform.localRotation = terminalNode.rotation;
        terminalNode.nodeGameObject.name = terminalNode.name;
        NodesLookupTable.nodesDictionary.Add(terminalNode.nodeGameObject.name, terminalNode);

        Branch trunk = terminalNode.nodeGameObject.GetComponent<Branch>();
        trunk.prototype = plantSpecies.branchPrototype;
        trunk.rootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(rootNode.name);
        trunk.terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(terminalNode.name);
        trunk.childBranches = new List<Branch>();

        trunk.capsuleCollider = trunk.setCapsuleCollider();

        this.trunk = trunk;
        
        rootNode.plant = this;
        terminalNode.plant = this;

        terminalNode.branchLineRenderer = terminalNode.setBranchLineRenderer();

        ResearchData.increasePlantCountForSpecies(plantSpecies.id);
        //ResearchData.increaseBranchCountForSpecies(plantSpecies.id);
        //ResearchData.increaseCurrentBranchCountForFirstTrees(id);
        //ResearchData.increaseOverallBranchCountForFirstTrees(id);
    }

    public void growPlant()
    {
        numberOfLeftSideBranches = 0;
        numberOfRightSideBranches = 0;

        totalLightExposure = trunk.calculateLightExposure();
        age += 0.01f;

        float vigor = setVigor(totalLightExposure, 1f);

        effectiveFloweringAge = plantSpecies.floweringAge * plantSpecies.vigorMax / vigor;

        
        if (age >= effectiveFloweringAge && plantSpecies.apicalControl != plantSpecies.apicalControlMature)
        {
            plantSpecies.apicalControl = plantSpecies.apicalControlMature;
        }

        if (vigor <= plantSpecies.vigorMin && trunk.childBranches.Count == 0)
        {
            destroyPlant();
            return;
        }

        trunk.distributeVigor(vigor);
        trunk.GrowBranch(1f);
    }

    public Vector3 getSeedPosition()
    {
        float randomX = Random.Range(-1.0f, 1.0f);
        float randomZ = Random.Range(-1.0f, 1.0f);
        float randomRange = Random.Range(0f, plantSpecies.seedingRadius);

        Vector3 seedPosition = plantGameObject.transform.position + (new Vector3(randomX, 0, randomZ).normalized * randomRange);

        return seedPosition;
    }

    private Quaternion getRandomPlantRotation()
    {
        float randomY = Random.Range(0, 360);
        Vector3 rotationEuler = new Vector3(0, randomY, 0);

        return Quaternion.Euler(rotationEuler);
    }

    private float setVigor(float totalLightExposure, float vigorDecreaseStep)
    {
        if (age >= plantSpecies.maxAge)
        {
            plantSpecies.vigorMax -= vigorDecreaseStep;
        }

        return totalLightExposure <= plantSpecies.vigorMax ? totalLightExposure : plantSpecies.vigorMax;
    }

    private void destroyPlant()
    {
        ResearchData.decreasePlantCountForSpecies(plantSpecies.id);
        //ResearchData.decreaseBranchCountForSpecies(plantSpecies.id);
        //ResearchData.decreaseCurrentBranchCountForFirstTrees(id);
        //ResearchData.increaseFallOffBranchCountForFirstTrees(id);
        Destroy(trunk.terminalNode.gameObject);
        Destroy(trunk.rootNode.gameObject);
        Destroy(plantGameObject);
    }
}
