using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace Assets.Scripts.HelpfulStructures
{
    public class PerformanceMeasures: MonoBehaviour
    {
        private int frameCount = 0;
        private float totalTime = 0;
        private float minFPS = float.MaxValue;
        private float maxFPS = 0f;
        private float currentFPS = 0f;
        private long minRamUsage = long.MaxValue;
        private long maxRamUsage = 0;
        private long currentRamUsage = 0;

        private Dictionary <float, float> currentFPSPerTotalTime = new Dictionary<float, float>();
        private Dictionary <float, long> currentRamUsagePerTotalTime = new Dictionary<float, long>();
        private Dictionary<float, double> currentWorldAgePerTotalTime = new Dictionary<float, double>();

        private void Update()
        {
            frameCount++;
            totalTime += Time.unscaledDeltaTime;

            currentFPS = 1f / Time.unscaledDeltaTime;
            minFPS = MathF.Min(minFPS, currentFPS);
            maxFPS = MathF.Max(maxFPS, currentFPS);

            currentRamUsage = Profiler.GetTotalAllocatedMemoryLong();
            minRamUsage = (long)Mathf.Min(minRamUsage, currentRamUsage);
            maxRamUsage = (long)Mathf.Max(maxRamUsage, currentRamUsage);

            currentFPSPerTotalTime.Add(totalTime, currentFPS);
            currentRamUsagePerTotalTime.Add(totalTime, currentRamUsage);
            currentWorldAgePerTotalTime.Add(totalTime, ResearchData.worldAge);
        }

        public void writePerformanceMeasuresToFiles()
        {
            string dateTimeNow = DateTime.Now.ToString("dd.MM.yy hh-mm-ss");
            string fileName = $"overall-performance-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine($"Frame count: {frameCount}");
                sw.WriteLine($"Total time: {totalTime} s");
                sw.WriteLine($"Min FPS: {minFPS}");
                sw.WriteLine($"Max FPS: {maxFPS}");
                sw.WriteLine($"Min RAM usage: {minRamUsage}");
                sw.WriteLine($"Max RAM usage: {maxRamUsage}");
            }

            fileName = $"FPS-per-time-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("time;world age;FPS");

                foreach (KeyValuePair<float, float> pair in currentFPSPerTotalTime)
                {
                    sw.WriteLine($"{pair.Key};{currentWorldAgePerTotalTime[pair.Key]};{pair.Value}");
                }
            }

            fileName = $"RAM-usage-per-time-{dateTimeNow}.csv";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("time;world age;RAM usage");

                foreach (KeyValuePair<float, long> pair in currentRamUsagePerTotalTime)
                {
                    sw.WriteLine($"{pair.Key};{currentWorldAgePerTotalTime[pair.Key]};{pair.Value}");
                }
            }
        }
    }
}
