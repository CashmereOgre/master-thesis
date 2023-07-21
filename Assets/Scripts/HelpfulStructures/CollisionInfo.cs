using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.HelpfulStructures
{
    public class CollisionInfo
    {
        public float volumeOfCollision { get; set; }
        public int collisionPointsCount { get; set; }

        public CollisionInfo(float volume, int count) 
        { 
            this.volumeOfCollision = volume;
            this.collisionPointsCount = count;
        }
    }
}
