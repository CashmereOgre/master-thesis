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
        public Vector3 colliderPosition { get; set; }
        public float colliderRadius { get; set; }
        public float colliderDistance { get; set; }

        public CollisionInfo(float volume, Vector3 position, float radius, float distance) 
        { 
            volumeOfCollision = volume;
            colliderPosition = position;
            colliderRadius = radius;
            colliderDistance = distance;
        }
    }
}
