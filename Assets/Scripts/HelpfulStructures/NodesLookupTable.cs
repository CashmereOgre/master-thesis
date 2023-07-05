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
                maxLength = 1f,
                plantVariables = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0),
                parentNodeId = null,
                childNodeIds = new List<int>{ 1 },
            }
        },
        { 1, new Node()
            {
                id = 1,
                isRoot = true,
                position = new Vector3(0f, 1f, 0f),
                rotation = Quaternion.identity,
                age = 0f,
                maxLength = 1f,
                plantVariables = PlantSpeciesLookupTable.plantSpeciesDictionary.GetValueOrDefault(0),
                parentNodeId = 0,
                childNodeIds = new List<int>{ 2, 3 },
            }   
        },
        { 2, new Node() },
        { 3, new Node() },
    };
}
