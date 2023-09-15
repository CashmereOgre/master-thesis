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
                    physiologicalAge = 0f,
                    maxLength = 2.5f,
                    parentNodeId = 0,
                    childNodeIds = new List<int>{ 2, 3, 4 },
                    nodeGameObject = nodePrefab
                }
            },
            { 2, new Node()
                {
                    id = 2,
                    isRoot = false,
                    isMain = false,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 90f, -30f),
                    physiologicalAge = 0f,
                    maxLength = 2f,
                    parentNodeId = 1,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
            { 3, new Node()
                {
                    id = 3,
                    isRoot = false,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 90f, 0f),
                    physiologicalAge = 0f,
                    maxLength = 2.5f,
                    parentNodeId = 1,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
            { 4, new Node()
                {
                    id = 4,
                    isRoot = false,
                    isMain = false,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 90f, 30f),
                    physiologicalAge = 0f,
                    maxLength = 2f,
                    parentNodeId = 1,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
            { 5, new Node()
                {
                    id = 5,
                    isRoot = true,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    physiologicalAge = 0f,
                    maxLength = 0f,
                    parentNodeId = 0,
                    childNodeIds = new List<int>{ 6 },
                    nodeGameObject = nodePrefab
                }
            },
            { 6, new Node()
                {
                    id = 6,
                    isRoot = true,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    physiologicalAge = 0f,
                    maxLength = 2.5f,
                    parentNodeId = 5,
                    childNodeIds = new List<int>{ 7, 8, 9 },
                    nodeGameObject = nodePrefab
                }
            },
            { 7, new Node()
                {
                    id = 7,
                    isRoot = false,
                    isMain = false,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 0f, 70f),
                    physiologicalAge = 0f,
                    maxLength = 2f,
                    parentNodeId = 6,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
            { 8, new Node()
                {
                    id = 8,
                    isRoot = false,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 30f, 0f),
                    physiologicalAge = 0f,
                    maxLength = 2.5f,
                    parentNodeId = 6,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
            { 9, new Node()
                {
                    id = 9,
                    isRoot = false,
                    isMain = false,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 0f, -70f),
                    physiologicalAge = 0f,
                    maxLength = 2f,
                    parentNodeId = 6,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
            { 10, new Node()
                {
                    id = 10,
                    isRoot = true,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    physiologicalAge = 0f,
                    maxLength = 0f,
                    parentNodeId = 0,
                    childNodeIds = new List<int>{ 11 },
                    nodeGameObject = nodePrefab
                }
            },
            { 11, new Node()
                {
                    id = 11,
                    isRoot = true,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    physiologicalAge = 0f,
                    maxLength = 1.5f,
                    parentNodeId = 10,
                    childNodeIds = new List<int>{ 12, 13 },
                    nodeGameObject = nodePrefab
                }
            },
            { 12, new Node()
                {
                    id = 12,
                    isRoot = false,
                    isMain = false,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 45f, -30f),
                    physiologicalAge = 0f,
                    maxLength = 2f,
                    parentNodeId = 11,
                    childNodeIds = new List<int>(),
                    nodeGameObject = nodePrefab
                }
            },
            { 13, new Node()
                {
                    id = 13,
                    isRoot = false,
                    isMain = true,
                    position = Vector3.zero,
                    rotation = Quaternion.Euler(0f, 90f, -10f),
                    physiologicalAge = 0f,
                    maxLength = 1f,
                    parentNodeId = 11,
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
