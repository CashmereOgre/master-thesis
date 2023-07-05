using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpecies
{
    public float maxAge { get; set; } /// Max age
	public float gp { get; set; }  /// Growth Rate
    //public float tropism { get; set; }
    public float tropismMature { get; set; }   /// Tropism decrease by age
    //public float determinism { get; set; }
    //public float determinismMature { get; set; }
	public float g2 { get; set; }   /// Tropism strength overall
    public float scalingCoefficientBeta { get; set; } /// Scaling Coefficient
    //public float optimalTemperature { get; set; }
    //public float optimalPrecipitation { get; set; }
    //public float floweringAge { get; set; }
    //public float tropismAngleAlfa { get; set; }

    //Commented are not needed at that moment
}
