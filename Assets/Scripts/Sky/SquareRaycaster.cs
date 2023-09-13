using Assets.Scripts.HelpfulStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SquareRaycaster : MonoBehaviour
{
    private List<Vector3> raysSquareStartingPoints = new List<Vector3>();

    public List<Vector3> setRaysSquare(int squareDimension, int sideDensity, float squareHeight)
    {
        float startingCorner = -(squareDimension / 2);
        float distanceBetweenPoints = (float)squareDimension / (float)sideDensity;

        for (int i = 0; i <= sideDensity; i++)
        {
            for (int j = 0; j <= sideDensity; j++)
            {
                Vector3 rayStartingPoint = new Vector3(startingCorner + distanceBetweenPoints * j, squareHeight, startingCorner + distanceBetweenPoints * i);
                raysSquareStartingPoints.Add(rayStartingPoint);
            }
        }

        return raysSquareStartingPoints;
    }

    public Dictionary<string, int> castRaysSquare()
    {
        Dictionary<string, int> objectRayCountsDictionary = new Dictionary<string, int>();

        foreach (Vector3 rayPoint in raysSquareStartingPoints)
        {
            List<Vector3> directions = getDirections(rayPoint);

            foreach (Vector3 direction in directions)
            {
                Ray ray = new Ray(rayPoint, direction);
                RaycastHit hit;

                Physics.Raycast(ray, out hit, rayPoint.y * Mathf.Sqrt(2));

                addRayToDictionary(hit, objectRayCountsDictionary);
            }
        }

        return objectRayCountsDictionary;
    }

    private List<Vector3> getDirections(Vector3 currentPoint)
    {
        Vector3 endPoint1 = new Vector3(currentPoint.x, 0, currentPoint.z);
        Vector3 endPoint2 = new Vector3(currentPoint.x + currentPoint.y, 0, currentPoint.z);
        Vector3 endPoint3 = new Vector3(currentPoint.x - currentPoint.y, 0, currentPoint.z);
        Vector3 endPoint4 = new Vector3(currentPoint.x, 0, currentPoint.z + currentPoint.y);
        Vector3 endPoint5 = new Vector3(currentPoint.x, 0, currentPoint.z - currentPoint.y);

        return new List<Vector3>
        {
            (endPoint1 - currentPoint).normalized,
            (endPoint2 - currentPoint).normalized,
            (endPoint3 - currentPoint).normalized,
            (endPoint4 - currentPoint).normalized,
            (endPoint5 - currentPoint).normalized
        };
    }

    protected void addRayToDictionary(RaycastHit hit, Dictionary<string, int> objectRayCountsHitDictionary)
    {
        if (hit.collider != null)
        {
            string colliderGameObjectName = hit.collider.gameObject.name;
            if (objectRayCountsHitDictionary.ContainsKey(colliderGameObjectName))
            {
                objectRayCountsHitDictionary[colliderGameObjectName] += 1;
                return;
            }

            objectRayCountsHitDictionary.Add(colliderGameObjectName, 1);
        }
    }
}
