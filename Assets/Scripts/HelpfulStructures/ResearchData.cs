using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts.HelpfulStructures
{
    public static class ResearchData
    {
        public static float worldAge = 0;

        private static int species1PlantCount = 0;
        private static int species2PlantCount = 0;
        private static int species3PlantCount = 0;

        private static int species1CurrentBranchCount = 0;
        private static int species2CurrentBranchCount = 0;
        private static int species3CurrentBranchCount = 0;

        private static int plant1CurrentBranchCount = 0;
        private static int plant1FallOffBranchCount = 0;
        private static int plant1OverallBranchCount = 0;

        private static int plant2CurrentBranchCount = 0;
        private static int plant2FallOffBranchCount = 0;
        private static int plant2OverallBranchCount = 0;

        private static int plant3CurrentBranchCount = 0;
        private static int plant3FallOffBranchCount = 0;
        private static int plant3OverallBranchCount = 0;

        private static Dictionary<float, int> species1PlantCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> species2PlantCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> species3PlantCountInWorldAge = new Dictionary<float, int>();

        private static Dictionary<float, int> species1CurrentBranchCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> species2CurrentBranchCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> species3CurrentBranchCountInWorldAge = new Dictionary<float, int>();

        private static Dictionary<float, int> plant1CurrentBranchCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> plant1FallOffBranchCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> plant1OverallBranchCountInWorldAge = new Dictionary<float, int>();

        private static Dictionary<float, int> plant2CurrentBranchCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> plant2FallOffBranchCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> plant2OverallBranchCountInWorldAge = new Dictionary<float, int>();

        private static Dictionary<float, int> plant3CurrentBranchCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> plant3FallOffBranchCountInWorldAge = new Dictionary<float, int>();
        private static Dictionary<float, int> plant3OverallBranchCountInWorldAge = new Dictionary<float, int>();

        public static void increasePlantCountForSpecies(int id)
        {
            if (id == 0)
            {
                species1PlantCount++;
            }
            else if (id == 1)
            {
                species2PlantCount++;
            }
            else if (id == 2)
            {
                species3PlantCount++;
            }
        }

        public static void decreasePlantCountForSpecies(int id)
        {
            if (id == 0)
            {
                species1PlantCount--;
            }
            else if (id == 1)
            {
                species2PlantCount--;
            }
            else if (id == 2)
            {
                species3PlantCount--;
            }
        }

        public static void increaseBranchCountForSpecies(int id)
        {
            if (id == 0)
            {
                species1CurrentBranchCount++;
            }
            else if (id == 1)
            {
                species2CurrentBranchCount++;
            }
            else if (id == 2)
            {
                species3CurrentBranchCount++;
            }
        }

        public static void decreaseBranchCountForSpecies(int id)
        {
            if (id == 0)
            {
                species1CurrentBranchCount--;
            }
            else if (id == 1)
            {
                species2CurrentBranchCount--;
            }
            else if (id == 2)
            {
                species3CurrentBranchCount--;
            }
        }

        public static void increaseCurrentBranchCountForFirstTrees(int id)
        {
            if (id == 0)
            {
                plant1CurrentBranchCount++;
            }
            else if (id == 1)
            {
                plant2CurrentBranchCount++;
            }
            else if (id == 2)
            {
                plant3CurrentBranchCount++;
            }
        }

        public static void decreaseCurrentBranchCountForFirstTrees(int id)
        {
            if (id == 0)
            {
                plant1CurrentBranchCount--;
            }
            else if (id == 1)
            {
                plant2CurrentBranchCount--;
            }
            else if (id == 2)
            {
                plant3CurrentBranchCount--;
            }
        }

        public static void increaseFallOffBranchCountForFirstTrees(int id)
        {
            if (id == 0)
            {
                plant1FallOffBranchCount++;
            }
            else if (id == 1)
            {
                plant2FallOffBranchCount++;
            }
            else if (id == 2)
            {
                plant3FallOffBranchCount++;
            }
        }

        public static void increaseOverallBranchCountForFirstTrees(int id)
        {
            if (id == 0)
            {
                plant1OverallBranchCount++;
            }
            else if (id == 1)
            {
                plant2OverallBranchCount++;
            }
            else if (id == 2)
            {
                plant3OverallBranchCount++;
            }
        }

        public static void assignDataToDictionaries()
        {
            species1PlantCountInWorldAge.Add(worldAge, species1PlantCount);
            species2PlantCountInWorldAge.Add(worldAge, species2PlantCount);
            species3PlantCountInWorldAge.Add(worldAge, species3PlantCount);

            species1CurrentBranchCountInWorldAge.Add(worldAge, species1CurrentBranchCount);
            species2CurrentBranchCountInWorldAge.Add(worldAge, species2CurrentBranchCount);
            species3CurrentBranchCountInWorldAge.Add(worldAge, species3CurrentBranchCount);

            plant1CurrentBranchCountInWorldAge.Add(worldAge, plant1CurrentBranchCount);
            plant1FallOffBranchCountInWorldAge.Add(worldAge, plant1FallOffBranchCount);
            plant1OverallBranchCountInWorldAge.Add(worldAge, plant1OverallBranchCount);

            plant2CurrentBranchCountInWorldAge.Add(worldAge, plant2CurrentBranchCount);
            plant2FallOffBranchCountInWorldAge.Add(worldAge, plant2FallOffBranchCount);
            plant2OverallBranchCountInWorldAge.Add(worldAge, plant2OverallBranchCount);

            plant3CurrentBranchCountInWorldAge.Add(worldAge, plant3CurrentBranchCount);
            plant3FallOffBranchCountInWorldAge.Add(worldAge, plant3FallOffBranchCount);
            plant3OverallBranchCountInWorldAge.Add(worldAge, plant3OverallBranchCount);
        }

        public static void writeDataToFiles()
        {
            string dateTimeNow = DateTime.Now.ToString("dd.MM.yy hh-mm-ss");
            string fileName = $"species-plant-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;species 1;species 2;species 3");

                foreach (KeyValuePair<float, int> pair in species1PlantCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{species2PlantCountInWorldAge[pair.Key]};{species3PlantCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"species-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;species 1;species 2;species 3");

                foreach (KeyValuePair<float, int> pair in species1CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{species2CurrentBranchCountInWorldAge[pair.Key]};{species3CurrentBranchCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"plant-1-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;current branch count;fall off branch count;overall branch count");

                foreach (KeyValuePair<float, int> pair in plant1CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{plant1FallOffBranchCountInWorldAge[pair.Key]};{plant1OverallBranchCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"plant-2-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;current branch count;fall off branch count;overall branch count");

                foreach (KeyValuePair<float, int> pair in plant2CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{plant2FallOffBranchCountInWorldAge[pair.Key]};{plant2OverallBranchCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"plant-3-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;current branch count;fall off branch count;overall branch count");

                foreach (KeyValuePair<float, int> pair in plant3CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{plant3FallOffBranchCountInWorldAge[pair.Key]};{plant3OverallBranchCountInWorldAge[pair.Key]}");
                }
            }
        }
    }
}
