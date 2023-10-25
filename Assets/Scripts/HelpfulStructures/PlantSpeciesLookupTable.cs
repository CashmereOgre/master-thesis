using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.HelpfulStructures
{
    public static class PlantSpeciesLookupTable
    {
        public static Dictionary<int, PlantSpecies> plantSpeciesDictionary = new Dictionary<int, PlantSpecies>();

        public static void setupPlantSpecies()
        {
            plantSpeciesDictionary = new Dictionary<int, PlantSpecies>()
            {
                { 0, new PlantSpecies()
                    {
                        id = 0,
                        maxAge = 950f,
                        gp = 0.12f,
                        apicalControl = 0.75f,
                        apicalControlMature = 0.34f,
                        vigorMax = 900f,
                        vigorMin = 0.5f,
                        g1 = 0.2f,
                        g2 = 0.5f,
                        w1 = 0.5f,
                        w2 = 0.14f,
                        scalingCoefficientBeta = 1.29f,
                        alphaTropism = 0.66f,
                        floweringAge = 20f,
                        seedsPerYear = 2,
                        seedingRadius = 15,
                        branchPrototype = BranchPrototypesInstances.branchPrototype1,
                        branchColor = Color.red,
                        gpAlignment = 1f
                    }
                },
                { 1, new PlantSpecies()
                    {
                        id = 1,
                        maxAge = 300f,
                        gp = 0.20f,
                        apicalControl = 0.65f,
                        apicalControlMature = 0.65f,
                        vigorMax = 600f,
                        vigorMin = 2f,
                        g1 = 1.0f,
                        g2 = 0.5f,
                        w1 = 0.5f,
                        w2 = 0.81f,
                        scalingCoefficientBeta = 1.6f,
                        alphaTropism = -0.19f,
                        floweringAge = 10f,
                        seedsPerYear = 1,
                        seedingRadius = 30,
                        branchPrototype = BranchPrototypesInstances.branchPrototype2,
                        branchColor = Color.green,
                        gpAlignment = 0.6f
                    }
                },
                { 2, new PlantSpecies()
                    {
                        id = 2,
                        maxAge = 52f,
                        gp = 0.55f,
                        apicalControl = 0.78f,
                        apicalControlMature = 0.43f,
                        vigorMax = 200f,
                        vigorMin = 4f,
                        g1 = 0.73f,
                        g2 = 0.5f,
                        w1 = 0.5f,
                        w2 = 0.43f,
                        scalingCoefficientBeta = 2.5f,
                        alphaTropism = -0.27f,
                        floweringAge = 3f,
                        seedsPerYear = 3,
                        seedingRadius = 40,
                        branchPrototype = BranchPrototypesInstances.branchPrototype3,
                        branchColor = Color.blue,
                        gpAlignment = 0.3f
                    }
                }
            };
        }
    }
}
