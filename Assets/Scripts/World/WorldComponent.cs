using Assets.Scripts.HelpfulStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class WorldComponent : MonoBehaviour
{
    public List<Plant> plantsList { get; private set; }

    public GameObject nodePrefab;

    public GameObject plantPrefab;

    public Raycaster raycaster;

    private int squareHeight = 200;
    private int squareDimension = 30;
    private int sideDensity = 100;

    private void Start()
    {
        addPlantsToList();
        initializeWorld();

        foreach(Plant plant in plantsList)
        {
            plant.initializePlant();
        }
    }

    private void initializeWorld()
    {
        raycaster.setRaysSquare(squareDimension, sideDensity, squareHeight);
        BranchPrototypesInstances.Setup(nodePrefab);
    }

    private void addPlantsToList()
    {
        plantsList = new List<Plant>();

        PlantSpecies plant1Specimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0));
        Vector3 plant1Position = new Vector3(0f, 0f, 0f);
        Plant plant1 = new Plant(plant1Specimen, plant1Position);
        plant1.plantGameObject = plant1.instantiatePlant(plantPrefab);
        plant1.id = 1;
        plant1.plantGameObject.name = plant1.id.ToString();
        plant1.plantGameObject.transform.position = plant1Position;

        PlantSpecies plant2Specimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0));
        Vector3 plant2Position = new Vector3(7.5f, 0f, 7.5f);
        Plant plant2 = new Plant(plant2Specimen, plant2Position);
        plant2.plantGameObject = plant2.instantiatePlant(plantPrefab);
        plant2.id = 2;
        plant2.plantGameObject.name = plant2.id.ToString();
        plant2.plantGameObject.transform.position = plant2Position;

        plantsList.Add(plant1);
        plantsList.Add(plant2);
    }

    private void FixedUpdate()
    {
        RaycastCollisionsLookupTable.objectRayCountDictionary = raycaster.castRaysSquare();

        foreach (Plant plant in plantsList)
        {
            plant.growPlant();
        }
    }
}
