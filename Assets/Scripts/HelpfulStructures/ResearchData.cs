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
        public static double worldAge = 0;

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

        private static Dictionary<double, int> species1PlantCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> species2PlantCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> species3PlantCountInWorldAge = new Dictionary<double, int>();

        private static Dictionary<double, int> species1CurrentBranchCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> species2CurrentBranchCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> species3CurrentBranchCountInWorldAge = new Dictionary<double, int>();

        private static Dictionary<double, int> plant1CurrentBranchCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant1FallOffBranchCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant1OverallBranchCountInWorldAge = new Dictionary<double, int>();

        private static Dictionary<double, int> plant2CurrentBranchCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant2FallOffBranchCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant2OverallBranchCountInWorldAge = new Dictionary<double, int>();

        private static Dictionary<double, int> plant3CurrentBranchCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant3FallOffBranchCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant3OverallBranchCountInWorldAge = new Dictionary<double, int>();

        private static Dictionary<double, int> plant1LeftSideBranchesCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant1RightSideBranchesCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant2LeftSideBranchesCountInWorldAge = new Dictionary<double, int>();
        private static Dictionary<double, int> plant2RightSideBranchesCountInWorldAge = new Dictionary<double, int>();

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

        public static void resetBranchCounts()
        {
            plant1CurrentBranchCount = 0;
            plant1FallOffBranchCount = 0;
            plant1OverallBranchCount = 0;

            plant2CurrentBranchCount = 0;
            plant2FallOffBranchCount = 0;
            plant2OverallBranchCount = 0;
        }

        public static void calculateBranchesCountForExperiment2()
        {
            foreach (KeyValuePair<string, Node> keyValue in NodesLookupTable.nodesDictionary)
            {
                string[] plantName = keyValue.Key.Split('.');
                int plantId = int.Parse(plantName.First());

                if (plantName.Last() == "0")
                    continue;

                if (plantId == 0)
                {
                    if (keyValue.Value != null)
                    {
                        plant1CurrentBranchCount++;
                    }
                    else
                    {
                        plant1FallOffBranchCount++;
                    }

                    plant1OverallBranchCount++;
                }
                else if (plantId == 1)
                {
                    if (keyValue.Value != null)
                    {
                        plant2CurrentBranchCount++;
                    }
                    else
                    {
                        plant2FallOffBranchCount++;
                    }

                    plant2OverallBranchCount++;
                }
            }
        }

        public static void assignDataToExperiment1Dictionaries()
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

        public static void assignDataToExperiment2Dictionaries(int plant1LeftSideBranchCount, int plant1RightSideBranchCount, int plant2LeftSideBranchCount, int plant2RightSideBranchCount)
        {
            plant1LeftSideBranchesCountInWorldAge.Add(worldAge, plant1LeftSideBranchCount);
            plant1RightSideBranchesCountInWorldAge.Add(worldAge, plant1RightSideBranchCount);
            plant2LeftSideBranchesCountInWorldAge.Add(worldAge, plant2LeftSideBranchCount);
            plant2RightSideBranchesCountInWorldAge.Add(worldAge, plant2RightSideBranchCount);

            plant1CurrentBranchCountInWorldAge.Add(worldAge, plant1CurrentBranchCount);
            plant1FallOffBranchCountInWorldAge.Add(worldAge, plant1FallOffBranchCount);
            plant1OverallBranchCountInWorldAge.Add(worldAge, plant1OverallBranchCount);

            plant2CurrentBranchCountInWorldAge.Add(worldAge, plant2CurrentBranchCount);
            plant2FallOffBranchCountInWorldAge.Add(worldAge, plant2FallOffBranchCount);
            plant2OverallBranchCountInWorldAge.Add(worldAge, plant2OverallBranchCount);
        }

        public static void writeExperiment1DataToFiles()
        {
            string dateTimeNow = DateTime.Now.ToString("dd.MM.yy hh-mm-ss");
            string fileName = $"species-plant-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;species 1;species 2;species 3");

                foreach (KeyValuePair<double, int> pair in species1PlantCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{species2PlantCountInWorldAge[pair.Key]};{species3PlantCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"species-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;species 1;species 2;species 3");

                foreach (KeyValuePair<double, int> pair in species1CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{species2CurrentBranchCountInWorldAge[pair.Key]};{species3CurrentBranchCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"plant-1-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;current branch count;fall off branch count;overall branch count");

                foreach (KeyValuePair<double, int> pair in plant1CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{plant1FallOffBranchCountInWorldAge[pair.Key]};{plant1OverallBranchCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"plant-2-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;current branch count;fall off branch count;overall branch count");

                foreach (KeyValuePair<double, int> pair in plant2CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{plant2FallOffBranchCountInWorldAge[pair.Key]};{plant2OverallBranchCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"plant-3-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;current branch count;fall off branch count;overall branch count");

                foreach (KeyValuePair<double, int> pair in plant3CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{plant3FallOffBranchCountInWorldAge[pair.Key]};{plant3OverallBranchCountInWorldAge[pair.Key]}");
                }
            }
        }

        public static void writeExperiment2DataToFiles()
        {
            string dateTimeNow = DateTime.Now.ToString("dd.MM.yy hh-mm-ss");
            string fileName = $"plant-1-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;current branch count;fall off branch count;overall branch count;left side branches count;right side branches count");

                foreach (KeyValuePair<double, int> pair in plant1CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{plant1FallOffBranchCountInWorldAge[pair.Key]};{plant1OverallBranchCountInWorldAge[pair.Key]};{plant1LeftSideBranchesCountInWorldAge[pair.Key]};{plant1RightSideBranchesCountInWorldAge[pair.Key]}");
                }
            }

            fileName = $"plant-2-branch-count-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("world age;current branch count;fall off branch count;overall branch count;left side branches count;right side branches count");

                foreach (KeyValuePair<double, int> pair in plant2CurrentBranchCountInWorldAge)
                {
                    sw.WriteLine($"{pair.Key};{pair.Value};{plant2FallOffBranchCountInWorldAge[pair.Key]};{plant2OverallBranchCountInWorldAge[pair.Key]};{plant2LeftSideBranchesCountInWorldAge[pair.Key]};{plant2RightSideBranchesCountInWorldAge[pair.Key]}");
                }
            }
        }
    }
}
