using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpecies
{
    public float maxAge { get; set; } /// Max age
	public float gp { get; set; }  /// Growth Rate
    //public float tropism { get; set; }
    public float tropism { get; set; }   /// Tropism decrease by age
    public float apicalControl { get; set; }
    public float vigorMax { get; set; }
    public float vigorMin { get; set; }
    //public float determinism { get; set; }
    //public float determinismMature { get; set; }
    public float g1 { get; set; }
	public float g2 { get; set; }   /// Tropism strength overall
    public float w1 { get; set; }
    public float w2 { get; set; }
    public float scalingCoefficientBeta { get; set; } /// Scaling Coefficient
    //public float optimalTemperature { get; set; }
    //public float optimalPrecipitation { get; set; }
    //public float floweringAge { get; set; }
    //public float tropismAngleAlfa { get; set; }

    //Commented are not needed at that moment

    public PlantSpecies(PlantSpecies plantSpecies)
    {
        maxAge = plantSpecies.maxAge;
        gp = plantSpecies.gp;
        tropism = plantSpecies.tropism;
        apicalControl = plantSpecies.apicalControl;
        vigorMax = plantSpecies.vigorMax;
        vigorMin = plantSpecies.vigorMin;
        g1 = plantSpecies.g1;
        g2 = plantSpecies.g2;
        w1 = plantSpecies.w1;
        w2 = plantSpecies.w2;
        scalingCoefficientBeta = plantSpecies.scalingCoefficientBeta;
    }

    public PlantSpecies() {}
}
