using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.HelpfulStructures
{
    public static class ResearchData
    {
        public static float worldAge = 0;

        public static Dictionary<int, int> speciesPlantCount = new Dictionary<int, int>()
        {
            {0, 0},
            {1, 0},
            {2, 0}
        };

        public static Dictionary<int, int> plantCurrentBranchCount = new Dictionary<int, int>();

        public static Dictionary<int, int> plantFallOffBranchCount = new Dictionary<int, int>();

        public static Dictionary<int, int> plantSpeciesCurrentBranchCount = new Dictionary<int, int>()
        {
            {0, 0},
            {1, 0},
            {2, 0}
        };
    }
}
