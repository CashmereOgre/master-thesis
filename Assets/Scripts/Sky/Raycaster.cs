using Assets.Scripts.HelpfulStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private int numberOfRays = 360;
    private float maxDistance = 100;

    private void Update()
    {
        castRays();
    }

    private void castRays()
    {
        Dictionary<string, int> objectRayCountsDictionary = new Dictionary<string, int>();
        Vector3 startingPoint = new Vector3(maxDistance, 0f, 0f);
        float angleBetweenPoints = 360f / numberOfRays;

        for (int i = 0; i <= numberOfRays / 2; i++)
        {
            float angleZ = i * angleBetweenPoints;

            for (int j = 0; j < numberOfRays; j++)
            {
                float angleY = j * angleBetweenPoints;
                Vector3 point = getRotatedPoint(startingPoint, angleZ, angleY);
                Vector3 direction = Vector3.zero - point;
                direction.Normalize();
                Ray ray = new Ray(point, direction);
                RaycastHit hit;

                Physics.Raycast(ray, out hit, maxDistance * 2);

                addRayToDictionary(hit, objectRayCountsDictionary);
            }
        }

        RaycastCollisionsLookupTable.objectRayCountsDictionary = objectRayCountsDictionary;
    }

    private void addRayToDictionary(RaycastHit hit, Dictionary<string, int> objectRayCountsDictionary)
    {
        if (hit.collider != null)
        {
            string colliderGameObjectName = hit.collider.gameObject.name;
            if (objectRayCountsDictionary.ContainsKey(colliderGameObjectName))
            {
                objectRayCountsDictionary[colliderGameObjectName] += 1;
                return;
            }

            objectRayCountsDictionary.Add(colliderGameObjectName, 1);
        }
    }

    private Vector3 getRotatedPoint(Vector3 point, float angleZ, float angleY)
    {
        Quaternion quaternion = Quaternion.Euler(0f, angleY, angleZ);

        return quaternion * point;
    }


}
