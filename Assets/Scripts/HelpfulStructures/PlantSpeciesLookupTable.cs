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
            { 0, new PlantSpecies
                {
                    maxAge = 950f,
                    gp = 0.12f,
                    tropism = 0.2f,
                    apicalControl = 0.5f,
                    vigorMax = 900f,
                    vigorMin = 2f,
                    g1 = 0.2f,
                    g2 = 0.5f, // self-made parameter
                    w2 = 0.14f,
                    scalingCoefficientBeta = 1.29f
                }
            }
        };
    }
}
