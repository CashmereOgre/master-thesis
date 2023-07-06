using Assets.Scripts.HelpfulStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodesLookupTable
{
    public static Dictionary<int, Node> nodesDictionaryForBranchPrototypes = new Dictionary<int, Node>()
    {
        { 0, new Node()
            {
                id = 0,
                isRoot = true,
                position = Vector3.zero,
                rotation = Quaternion.identity,
                age = 0f,
                maxLength = 0f,
                plantVariables = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0),
                parentNodeId = null,
                childNodeIds = new List<int>{ 1 },
            }
        },
        { 1, new Node()
            {
                id = 1,
                isRoot = true,
                position = Vector3.zero,
                rotation = Quaternion.identity,
                age = 0f,
                maxLength = 1f,
                plantVariables = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0),
                parentNodeId = 0,
                childNodeIds = new List<int>{ 2, 3 },
            }
        },
        { 2, new Node()
            {
                id = 2,
                isRoot = false,
                position = Vector3.zero,
                rotation = Quaternion.Euler(0f, 90f, -30f),
                age = 0f,
                maxLength = 0.9f,
                plantVariables = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0),
                parentNodeId = 1,
                childNodeIds = new List<int>()
            }
        },
        { 3, new Node()
            {
                id = 3,
                isRoot = false,
                position = new Vector3(-0.216f, 0.630f, 0.743f),
                rotation = Quaternion.Euler(0f, 90f, 30f),
                age = 0f,
                maxLength = 0.9f,
                plantVariables = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0),
                parentNodeId = 1,
                childNodeIds = new List<int>()
            }
        },
    };

    public static Dictionary<int, Node> nodesDictionary = new Dictionary<int, Node>();
}
