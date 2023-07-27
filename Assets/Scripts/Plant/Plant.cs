using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant
{
    public Branch trunk { get; private set; }
    public Vector3 position { get; private set; }
    public PlantSpecies plantSpecies { get; private set; }
    public float totalLightExposure { get; set; }

    public Plant(Branch _trunk)
    {
        trunk = _trunk;
        position = Vector3.zero;
        plantSpecies = _trunk.rootNode.plantVariables;
        totalLightExposure = 0;
    }
}
