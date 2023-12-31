﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sky
{
    public class CubeRaycaster: SquareRaycaster
    {
        private List<Vector3> raysTopStartingPoints = new List<Vector3>();
        private List<Vector3> raysLeftRightStartingPoints = new List<Vector3>();
        private List<Vector3> raysBackFrontStartingPoints = new List<Vector3>();

        private float squareDimension = 0;
        private float dimensionSide = 0;
        private float dimensionFront = 0;

        public void setRaysCube(int squareDimension, int density, float cubeHeight)
        {
            this.squareDimension = squareDimension;
            int heightDensity = Mathf.FloorToInt(density * cubeHeight / squareDimension);
            float startingCorner = -(squareDimension / 2);
            float distanceBetweenPoints = (float)squareDimension / (float)density;

            raysTopStartingPoints = setRaysSquare(squareDimension, density, cubeHeight);

            for (int i = 0; i <= heightDensity; i++)
            {
                for (int j = 0; j <= density; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(startingCorner, 0 + i * distanceBetweenPoints, startingCorner + j * distanceBetweenPoints);
                    raysBackFrontStartingPoints.Add(rayStartingPoint);
                }
            }

            for (int i = 0; i <= heightDensity; i++)
            {
                for (int j = 0; j <= density; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(-startingCorner, 0 + i * distanceBetweenPoints, startingCorner + j * distanceBetweenPoints);
                    raysBackFrontStartingPoints.Add(rayStartingPoint);
                }
            }

            for (int i = 0; i <= heightDensity; i++)
            {
                for (int j = 0; j <= density; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(startingCorner + j * distanceBetweenPoints, 0 + i * distanceBetweenPoints, startingCorner);
                    raysLeftRightStartingPoints.Add(rayStartingPoint);
                }
            }

            for (int i = 0; i <= heightDensity; i++)
            {
                for (int j = 0; j <= density; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(startingCorner + j * distanceBetweenPoints, 0 + i * distanceBetweenPoints, -startingCorner);
                    raysLeftRightStartingPoints.Add(rayStartingPoint);
                }
            }
        }

        public void setRays(int dimensionSide, int dimensionFront, float distanceBetweenPoints, float cubeHeight)
        {
            this.dimensionSide = dimensionSide;
            this.dimensionFront = dimensionFront;
            int heightDensity = Mathf.FloorToInt(cubeHeight / distanceBetweenPoints + 1);
            int sideDensity = Mathf.FloorToInt(this.dimensionSide / distanceBetweenPoints + 1);
            int frontDensity = Mathf.FloorToInt(this.dimensionFront / distanceBetweenPoints + 1);
            float startingCornerX = -(this.dimensionFront / 2);
            float startingCornerZ = -(this.dimensionSide / 2);

            for (int i = 0; i <= sideDensity; i++)
            {
                for (int j = 0; j <= frontDensity; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(startingCornerX + j * distanceBetweenPoints, cubeHeight, startingCornerZ + i * distanceBetweenPoints);
                    raysTopStartingPoints.Add(rayStartingPoint);
                }
            }

            for (int i = 0; i <= heightDensity; i++)
            {
                for (int j = 0; j <= frontDensity; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(startingCornerX, 0 + i * distanceBetweenPoints, startingCornerZ + j * distanceBetweenPoints);
                    raysBackFrontStartingPoints.Add(rayStartingPoint);
                }
            }

            for (int i = 0; i <= heightDensity; i++)
            {
                for (int j = 0; j <= frontDensity; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(-startingCornerX, 0 + i * distanceBetweenPoints, startingCornerZ + j * distanceBetweenPoints);
                    raysBackFrontStartingPoints.Add(rayStartingPoint);
                }
            }

            for (int i = 0; i <= heightDensity; i++)
            {
                for (int j = 0; j <= sideDensity; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(startingCornerX + i * distanceBetweenPoints, 0 + j * distanceBetweenPoints, startingCornerZ);
                    raysLeftRightStartingPoints.Add(rayStartingPoint);
                }
            }

            for (int i = 0; i <= heightDensity; i++)
            {
                for (int j = 0; j <= sideDensity; j++)
                {
                    Vector3 rayStartingPoint = new Vector3(startingCornerX + i * distanceBetweenPoints, 0 + j * distanceBetweenPoints, -startingCornerZ);
                    raysLeftRightStartingPoints.Add(rayStartingPoint);
                }
            }
        }

        public Dictionary<string, int> castRays()
        {
            Dictionary<string, int> objectRayCountsDictionary = new Dictionary<string, int>();

            foreach (Vector3 rayStartingPoint in raysTopStartingPoints)
            {
                Vector3 endPoint = new Vector3(rayStartingPoint.x, 0, rayStartingPoint.z);
                Vector3 direction = (endPoint - rayStartingPoint).normalized;
                Ray ray = new Ray(rayStartingPoint, direction);
                RaycastHit hit;

                Physics.Raycast(ray, out hit, rayStartingPoint.y);

                addRayToDictionary(hit, objectRayCountsDictionary);
            }

            foreach (Vector3 rayStartingPoint in raysBackFrontStartingPoints)
            {
                Vector3 endPoint = new Vector3(-rayStartingPoint.x, rayStartingPoint.y, rayStartingPoint.z);
                Vector3 direction = (endPoint - rayStartingPoint).normalized;
                Ray ray = new Ray(rayStartingPoint, direction);
                RaycastHit hit;

                Physics.Raycast(ray, out hit, dimensionSide);

                addRayToDictionary(hit, objectRayCountsDictionary);
            }

            foreach (Vector3 rayStartingPoint in raysLeftRightStartingPoints)
            {
                Vector3 endPoint = new Vector3(rayStartingPoint.x, rayStartingPoint.y, -rayStartingPoint.z);
                Vector3 direction = (endPoint - rayStartingPoint).normalized;
                Ray ray = new Ray(rayStartingPoint, direction);
                RaycastHit hit;

                Physics.Raycast(ray, out hit, dimensionFront);

                addRayToDictionary(hit, objectRayCountsDictionary);
            }

            return objectRayCountsDictionary;
        }
    }
}
