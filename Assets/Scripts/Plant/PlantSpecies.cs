using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpecies
{
    public int id { get; set; }
    public float maxAge { get; set; }
	public float gp { get; set; }  
    public float tropism { get; set; } 
    public float apicalControl { get; set; }
    public float apicalControlMature { get; set; }
    public float vigorMax { get; set; }
    public float vigorMin { get; set; }
    public float g1 { get; set; }
	public float g2 { get; set; }   
    public float w1 { get; set; }
    public float w2 { get; set; }
    public float scalingCoefficientBeta { get; set; } 
    public float alphaTropism { get; set; }
    public float floweringAge { get; set; }
    public int seedsPerYear { get; set; }
    public float seedingRadius { get; set; }
    public BranchPrototype branchPrototype { get; set; }
    public Color branchColor { get; set; }
    public float gpAlignment { get; set; }


    public PlantSpecies(PlantSpecies plantSpecies)
    {
        id = plantSpecies.id;
        maxAge = plantSpecies.maxAge;
        gp = plantSpecies.gp;
        tropism = plantSpecies.tropism;
        apicalControl = plantSpecies.apicalControl;
        apicalControlMature = plantSpecies.apicalControlMature;
        vigorMax = plantSpecies.vigorMax;
        vigorMin = plantSpecies.vigorMin;
        g1 = plantSpecies.g1;
        g2 = plantSpecies.g2;
        w1 = plantSpecies.w1;
        w2 = plantSpecies.w2;
        scalingCoefficientBeta = plantSpecies.scalingCoefficientBeta;
        alphaTropism = plantSpecies.alphaTropism;
        floweringAge = plantSpecies.floweringAge;
        seedsPerYear = plantSpecies.seedsPerYear;
        seedingRadius = plantSpecies.seedingRadius;
        branchPrototype = plantSpecies.branchPrototype;
        branchColor = plantSpecies.branchColor;
        gpAlignment = plantSpecies.gpAlignment;
    }

    public PlantSpecies() {}
}
