using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPrototypesInstances
{
    public BranchPrototype basicBranchPrototype = new BranchPrototype()
    {
        maturityAge = 0.5f,
        rootNode = NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(0),
        centerNodes = new List<Node>() { NodesLookupTable.nodesDictionaryForBranchPrototypes.GetValueOrDefault(1) },
    };
}
