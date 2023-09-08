using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BranchPrototypesInstances
{
    public static BranchPrototype branchPrototype1 = new BranchPrototype();
    public static BranchPrototype branchPrototype2 = new BranchPrototype();
    public static BranchPrototype branchPrototype3 = new BranchPrototype();

    public static void Setup(GameObject nodePrefab)
    {
        NodesLookupTable.Setup(nodePrefab);

        branchPrototype1 = new BranchPrototype()
        {
            maturityAge = 5f,
            rootNode = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(0),
            centerNodes = new List<Node>() { NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(1) },
            terminalNodes = new List<Node>()
            {
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(2),
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(3),
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(4)
            }
        };

        branchPrototype2 = new BranchPrototype()
        {
            maturityAge = 3f,
            rootNode = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(5),
            centerNodes = new List<Node>() { NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(6) },
            terminalNodes = new List<Node>()
            {
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(7),
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(8),
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(9)
            }
        };

        branchPrototype3 = new BranchPrototype()
        {
            maturityAge = 1.5f,
            rootNode = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(10),
            centerNodes = new List<Node>() { NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(11) },
            terminalNodes = new List<Node>()
            {
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(12),
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(13)
            }
        };
    }
}
