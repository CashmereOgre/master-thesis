using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.HelpfulStructures
{
    public static class PlantSpeciesLookupTable
    {
        public static Dictionary<int, PlantSpecies> plantSpeciesDictionary = new Dictionary<int, PlantSpecies>()
        {
            { 0, new PlantSpecies()
                {
                    id = 0,
                    maxAge = 950f,
                    gp = 0.12f,
                    tropism = 0.2f,
                    apicalControl = 0.87f,
                    apicalControlMature = 0.34f,
                    vigorMax = 900f,
                    vigorMin = 2f,
                    g1 = 0.2f,
                    g2 = 0.5f, // self-made parameter
                    w1 = 0.5f,
                    w2 = 0.14f,
                    scalingCoefficientBeta = 1.29f,
                    alphaTropism = 0.66f,
                    floweringAge = 35f,
                    seedsPerYear = 2,
                    seedingRadius = 15,
                }
            },
            { 1, new PlantSpecies()
                {
                    id = 1,
                    maxAge = 300f,
                    gp = 0.20f,
                    tropism = 0.5f,
                    apicalControl = 0.8f,
                    apicalControlMature = 0.8f,
                    vigorMax = 600f,
                    vigorMin = 2f,
                    g1 = 1.0f,
                    g2 = 0.5f, // self-made parameter
                    w1 = 0.5f,
                    w2 = 0.81f,
                    scalingCoefficientBeta = 1.6f,
                    alphaTropism = -0.19f,
                    floweringAge = 20f,
                    seedsPerYear = 1,
                    seedingRadius = 30,
                }
            },
            { 2, new PlantSpecies()
                {
                    id = 2,
                    maxAge = 52f,
                    gp = 0.55f,
                    tropism = 0.5f,
                    apicalControl = 0.96f,
                    apicalControlMature = 0.43f,
                    vigorMax = 200f,
                    vigorMin = 2f,
                    g1 = 0.73f,
                    g2 = 0.5f, 
                    w1 = 0.5f,
                    w2 = 0.43f,
                    scalingCoefficientBeta = 2.5f,
                    alphaTropism = -0.27f,
                    floweringAge = 10f,
                    seedsPerYear = 3,
                    seedingRadius = 40,
                }
            }
        };
    }
}
