using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.MaterialProperty;
using Assets.Scripts.HelpfulStructures;
using Assets.Scripts.Sky;

public class WorldComponent : MonoBehaviour
{
    public List<Plant> plantsList { get; private set; }

    public GameObject nodePrefab;

    public GameObject plantPrefab;

    public SquareRaycaster squareRaycaster;

    public CubeRaycaster cubeRaycaster;

    private int squareHeight = 200;
    private int squareDimension = 30;
    private int sideDensity = 100;

    private void Start()
    {
        initializeWorld();
        addPlantsToList();

        foreach(Plant plant in plantsList)
        {
            plant.initializePlant();
        }
    }

    private void initializeWorld()
    {
        //squareRaycaster.setRaysSquare(squareDimension, sideDensity, squareHeight);
        cubeRaycaster.setRaysCube(squareDimension, sideDensity, squareHeight);
        BranchPrototypesInstances.Setup(nodePrefab);
        PlantSpeciesLookupTable.setupPlantSpecies();
    }

    private void addPlantsToList()
    {
        plantsList = new List<Plant>();

        PlantSpecies plant1Specimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0));
        Vector3 plant1Position = new Vector3(0f, 0f, 0f);
        Plant plant1 = new Plant(plant1Specimen, plant1Position);
        plant1.plantGameObject = plant1.instantiatePlant(plantPrefab);
        plant1.id = 0;
        plant1.plantGameObject.name = plant1.id.ToString();
        plant1.plantGameObject.transform.position = plant1Position;

        PlantSpecies plant2Specimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(1));
        Vector3 plant2Position = new Vector3(7.5f, 0f, 7.5f);
        Plant plant2 = new Plant(plant2Specimen, plant2Position);
        plant2.plantGameObject = plant2.instantiatePlant(plantPrefab);
        plant2.id = 1;
        plant2.plantGameObject.name = plant2.id.ToString();
        plant2.plantGameObject.transform.position = plant2Position;

        PlantSpecies plant3Specimen = new PlantSpecies(PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(2));
        Vector3 plant3Position = new Vector3(-7.5f, 0f, -7.5f);
        Plant plant3 = new Plant(plant3Specimen, plant3Position);
        plant3.plantGameObject = plant3.instantiatePlant(plantPrefab);
        plant3.id = 2;
        plant3.plantGameObject.name = plant3.id.ToString();
        plant3.plantGameObject.transform.position = plant3Position;

        plantsList.Add(plant1);
        plantsList.Add(plant2);
        plantsList.Add(plant3);
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
        ResearchData.worldAge += 0.01f;
        Debug.Log(ResearchData.worldAge.ToString());

        //RaycastCollisionsLookupTable.objectRayCountDictionary = squareRaycaster.castRaysSquare();
        RaycastCollisionsLookupTable.objectRayCountDictionary = cubeRaycaster.castRaysCube();

        foreach (Plant plant in plantsList.ToList())
        {
            if(plant.trunk != null)
            {
                plant.growPlant();

                if (plant.age >= plant.effectiveFloweringAge && Mathf.Abs(plant.age - Mathf.Floor(plant.age + 0.01f)) <= 0.01)
                {
                    for (int i = 0; i < plant.plantSpecies.seedsPerYear; i++)
                    {
                        Vector3 newPlantPosition = plant.getSeedPosition();
                        addNewPlant(newPlantPosition, plant);
                    }
                }
            }
        }

        ResearchData.assignDataToDictionaries();

        if (ResearchData.worldAge >= 1200f || Input.GetKeyDown(KeyCode.Escape))
        {
            quit();
        }
    }

    private void quit()
    {
        ResearchData.writeDataToFiles();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
