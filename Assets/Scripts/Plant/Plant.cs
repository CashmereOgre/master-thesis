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

    public GameObject plantGameObject;

    public Plant(PlantSpecies _plantSpecimen, Vector3 _position)
    {
        trunk = null;
        position = _position;
        plantSpecies = _plantSpecimen;
        totalLightExposure = 0;
        age = 0;
    }

    public GameObject instantiatePlant(GameObject plantPrefab)
    {
        plantGameObject = Instantiate(plantPrefab);
        return plantGameObject;
    }

    public void initializePlant()
    {
        Node rootNodePrototype = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(0);
        Node rootNode = new Node(rootNodePrototype);
        rootNode.nodeGameObject = rootNode.instantiateNode(plantGameObject.transform);
        rootNode = rootNode.nodeGameObject.GetComponent<Node>();
        rootNode.id = rootNodePrototype.id;
        rootNode.name = $"{id}.root";
        rootNode.isMain = rootNodePrototype.isMain;
        rootNode.position = rootNodePrototype.position;
        rootNode.rotation = rootNodePrototype.rotation;
        rootNode.age = rootNodePrototype.age;
        rootNode.physiologicalAge = rootNodePrototype.physiologicalAge;
        rootNode.maxLength = rootNodePrototype.maxLength;
        rootNode.parentNodeId = rootNodePrototype.parentNodeId;
        rootNode.childNodeIds = rootNodePrototype.childNodeIds;
        rootNode.nodeGameObject.transform.localRotation = rootNode.rotation;
        rootNode.nodeGameObject.name = rootNode.name;
        Destroy(rootNode.nodeGameObject.GetComponent<CapsuleCollider>());
        NodesLookupTable.nodesDictionary.Add(rootNode.nodeGameObject.name, rootNode);

        Node terminalNodePrototype = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(1);
        Node terminalNode = new Node(terminalNodePrototype);
        terminalNode.nodeGameObject = terminalNode.instantiateNode(rootNode.nodeGameObject.transform);
        terminalNode = terminalNode.nodeGameObject.GetComponent<Node>();
        terminalNode.id = terminalNodePrototype.id;
        terminalNode.name = $"{id}.trunk";
        terminalNode.isMain = terminalNodePrototype.isMain;
        terminalNode.position = terminalNodePrototype.position;
        terminalNode.rotation = terminalNodePrototype.rotation;
        terminalNode.age = terminalNodePrototype.age;
        terminalNode.physiologicalAge = terminalNodePrototype.physiologicalAge;
        terminalNode.maxLength = terminalNodePrototype.maxLength;
        terminalNode.parentNodeId = terminalNodePrototype.parentNodeId;
        terminalNode.parentNodeName = rootNode.name;
        terminalNode.childNodeIds = terminalNodePrototype.childNodeIds;
        terminalNode.nodeGameObject.transform.rotation = terminalNode.rotation;
        terminalNode.nodeGameObject.name = terminalNode.name;
        terminalNode.branchLineRenderer = terminalNode.setBranchLineRenderer();
        NodesLookupTable.nodesDictionary.Add(terminalNode.nodeGameObject.name, terminalNode);

        Branch trunk = terminalNode.nodeGameObject.GetComponent<Branch>();
        trunk.prototype = BranchPrototypesInstances.basicBranchPrototype;
        trunk.maxAge = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0).maxAge;
        trunk.currentAge = 0.0f;
        trunk.rootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(rootNode.name);
        trunk.terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(terminalNode.name);
        trunk.childBranches = new List<Branch>();

        trunk.capsuleCollider = trunk.setCapsuleCollider();

        this.trunk = trunk;
        
        rootNode.plant = this;
        terminalNode.plant = this;
    }

    public void growPlant()
    {
        totalLightExposure = trunk.calculateLightExposure();
        age += 1f;

        float vigor = setVigor(totalLightExposure, 1f);

        if (vigor <= plantSpecies.vigorMin)
        {
            destroyPlant();
            return;
        }

        trunk.distributeVigor(vigor);
        trunk.GrowBranch(1000f);
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
        Destroy(trunk.terminalNode.gameObject);
        Destroy(trunk.rootNode.gameObject);
        Destroy(this);
    }
}
