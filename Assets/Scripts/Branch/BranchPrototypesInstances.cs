using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BranchPrototypesInstances
{
    public static BranchPrototype basicBranchPrototype = new BranchPrototype();

    public static void Setup(GameObject nodePrefab)
    {
        NodesLookupTable.Setup(nodePrefab);

        basicBranchPrototype = new BranchPrototype()
        {
            maturityAge = 2.5f,
            rootNode = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(0),
            centerNodes = new List<Node>() { NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(1) },
            terminalNodes = new List<Node>()
            {
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(2),
                NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(3)
            }
        };
    }
}
