using Assets.Scripts.HelpfulStructures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NodesLookupTable
{
    public static GameObject nodePrefab;

    public static Dictionary<int, Node> nodesDictionaryForBranchPrototypes = new Dictionary<int, Node>();

    public static Dictionary<string, Node> nodesDictionary = new Dictionary<string, Node>();

    public static void Setup(GameObject prefab)
    {
        nodePrefab = prefab;

        nodesDictionaryForBranchPrototypes = new Dictionary<int, Node>()
        {
            { 0, new Node()
                {
                    id = 0,
                    isRoot = true,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    age = 0f,
                    physiologicalAge = 0f,
                    maxLength = 0f,
                    parentNodeId = 0,
                    childNodeIds = new List<int>{ 1 },
                    nodeGameObject = nodePrefab
                }
            },
            { 1, new Node()
                {
                    id = 1,
                    isRoot = true,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    age = 0f,
                    physiologicalAge = 0f,
                    maxLength = 50f,
                    parentNodeId = 0,
                    childNodeIds = new List<int>{ 2, 3 },
                    nodeGameObject = nodePrefab
                }
            },
            { 2, new Node()
                {
                    id = 2,
                    isRoot = false,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 90f, -30f),
                    age = 0f,
                    physiologicalAge = 0f,
                    maxLength = 40f,
                    parentNodeId = 1,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
            { 3, new Node()
                {
                    id = 3,
                    isRoot = false,
                    isMain = false,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 90f, 30f),
                    age = 0f,
                    physiologicalAge = 0f,
                    maxLength = 90f,
                    parentNodeId = 1,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
        };
    }

    public static int getIdOfLastNodeInPlant(int plantId)
    {
        var matchingKeys = nodesDictionary.Keys
            .Where(key => key.StartsWith(plantId + "."))
            .ToList();

        string lastKey = matchingKeys.Last();
        int lastNodeId = int.Parse(lastKey.Split(".")[1]);

        return lastNodeId;
    }
}
