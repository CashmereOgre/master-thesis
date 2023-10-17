using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts.HelpfulStructures;
using Assets.Scripts.Sky;
using UnityEngine.UI;

public class WorldComponent : MonoBehaviour
{
    public List<Plant> plantsList { get; private set; }

    public GameObject nodePrefab;

    public GameObject plantPrefab;

    public SquareRaycaster squareRaycaster;

    public CubeRaycaster cubeRaycaster;

    public Text worldAgeText;

    public PerformanceMeasures performanceMeasures;

    private int cubeHeight = 200;
    private int squareDimension = 100;
    private int sideDensity = 333;

    private int sideDimension = 20;
    private int frontDimension = 30;
    private float distanceBetweenRays = 0.33f;

    private int plant1LeftSideBranchesCount = 0;
    private int plant1RightSideBranchesCount = 0;
    private int plant2LeftSideBranchesCount = 0;
    private int plant2RightSideBranchesCount = 0;

    private void Start()
    {
        initializeWorld();
        //addPlantsToListExperiment1();
        addPlantsToListExperiment2();

        foreach (Plant plant in plantsList)
        {
            plant.initializePlant();
        }
    }

    private void initializeWorld()
    {
        //squareRaycaster.setRaysSquare(squareDimension, sideDensity, cubeHeight);
        //cubeRaycaster.setRaysCube(squareDimension, sideDensity, cubeHeight);
        cubeRaycaster.setRays(sideDimension, frontDimension, distanceBetweenRays, cubeHeight);
        BranchPrototypesInstances.Setup(nodePrefab);
        PlantSpeciesLookupTable.setupPlantSpecies();
    }

    private void addPlantsToListExperiment1()
    {
        plantsList = new List<Plant>();

        PlantSpecies plant1Specimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0));
        Vector3 plant1Position = new Vector3(-10f, 0f, -42f);
        Plant plant1 = new Plant(plant1Specimen, plant1Position);
        plant1.plantGameObject = plant1.instantiatePlant(plantPrefab);
        plant1.id = 0;
        plant1.plantGameObject.name = plant1.id.ToString();
        plant1.plantGameObject.transform.position = plant1Position;

        PlantSpecies plant2Specimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(1));
        Vector3 plant2Position = new Vector3(-30f, 0f, 30f);
        Plant plant2 = new Plant(plant2Specimen, plant2Position);
        plant2.plantGameObject = plant2.instantiatePlant(plantPrefab);
        plant2.id = 1;
        plant2.plantGameObject.name = plant2.id.ToString();
        plant2.plantGameObject.transform.position = plant2Position;

        PlantSpecies plant3Specimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(2));
        Vector3 plant3Position = new Vector3(40f, 0f, -5f);
        Plant plant3 = new Plant(plant3Specimen, plant3Position);
        plant3.plantGameObject = plant3.instantiatePlant(plantPrefab);
        plant3.id = 2;
        plant3.plantGameObject.name = plant3.id.ToString();
        plant3.plantGameObject.transform.position = plant3Position;

        plantsList.Add(plant1);
        plantsList.Add(plant2);
        plantsList.Add(plant3);
    }

    private void addPlantsToListExperiment2()
    {
        plantsList = new List<Plant>();

        PlantSpecies plantSpecimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0));
        Vector3 plant1Position = new Vector3(-3f, 0f, 0f);
        Plant plant1 = new Plant(plantSpecimen, plant1Position);
        plant1.plantGameObject = plant1.instantiatePlant(plantPrefab);
        plant1.id = 0;
        plant1.plantGameObject.name = plant1.id.ToString();
        plant1.plantGameObject.transform.position = plant1Position;
        plant1.isSeeding = false;

        Vector3 plant2Position = new Vector3(3f, 0f, 0f);
        Plant plant2 = new Plant(plantSpecimen, plant2Position);
        plant2.plantGameObject = plant2.instantiatePlant(plantPrefab);
        plant2.id = 1;
        plant2.plantGameObject.name = plant2.id.ToString();
        plant2.plantGameObject.transform.position = plant2Position;
        plant2.isSeeding = false;

        plantsList.Add(plant1);
        plantsList.Add(plant2);
    }

    private void addNewPlant(Vector3 position, Plant parent)
    {
        PlantSpecies plantSpecimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(parent.plantSpecies.id));
        Plant plant = new Plant(plantSpecimen, position);
        plant.plantGameObject = plant.instantiatePlant(plantPrefab);
        plant.id = plantsList.Last().id + 1;
        plant.plantGameObject.name = plant.id.ToString();
        plant.plantGameObject.transform.position = position;

        plant.initializePlant();
        plantsList.Add(plant);
    }

    private void FixedUpdate()
    {
        ResearchData.resetBranchCounts();
        ResearchData.worldAge += 0.01f;
        worldAgeText.text = ResearchData.worldAge.ToString();

        //RaycastCollisionsLookupTable.objectRayCountDictionary = squareRaycaster.castRaysSquare();
        RaycastCollisionsLookupTable.objectRayCountDictionary = cubeRaycaster.castRays();

        foreach (Plant plant in plantsList.ToList())
        {
            if(plant.trunk != null)
            {
                plant.growPlant();

                if (plant.isSeeding && plant.age >= plant.effectiveFloweringAge && Mathf.Abs(plant.age - Mathf.Floor(plant.age + 0.01f)) <= 0.01)
                {
                    for (int i = 0; i < plant.plantSpecies.seedsPerYear; i++)
                    {
                        Vector3 newPlantPosition = plant.getSeedPosition();
                        addNewPlant(newPlantPosition, plant);
                    }
                }
            }
        }

        plant1LeftSideBranchesCount = plantsList[0].numberOfLeftSideBranches;
        plant1RightSideBranchesCount = plantsList[0].numberOfRightSideBranches;
        plant2LeftSideBranchesCount = plantsList[1].numberOfLeftSideBranches;
        plant2RightSideBranchesCount = plantsList[1].numberOfRightSideBranches;

        //ResearchData.assignDataToExperiment1Dictionaries();
        ResearchData.calculateBranchesCountForExperiment2();
        ResearchData.assignDataToExperiment2Dictionaries(plant1LeftSideBranchesCount, plant1RightSideBranchesCount, plant2LeftSideBranchesCount, plant2RightSideBranchesCount);

        if (ResearchData.worldAge >= 1000f)
        {
            Application.Quit();
        }
    }

    private void OnApplicationQuit()
    {
        ResearchData.writeExperiment2DataToFiles();
        performanceMeasures.writePerformanceMeasuresToFiles();
    }
}
