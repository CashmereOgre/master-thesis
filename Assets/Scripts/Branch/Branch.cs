using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Branch
{
    public BranchPrototype prototype { get; set; }
    public float maxAge { get; set; }
    public float currentAge { get; set; }
    public Node rootNode { get; set; }
    public Node terminalNode { get; set; }
    public List<Branch> childBranches { get; set; }

    public void GrowBranch(float ageStep)
    {
        if(prototype != null)
        {
            terminalNode.age += ageStep;

            // TODO to improve when adding branches shredding and blooming
            //bool isBecomingMature = newAge >= prototype.maturityAge ? rootNode.age < prototype.maturityAge : false;
            //bool decay = rootNode.age >= prototype.maturityAge;

            if (terminalNode.age < prototype.maturityAge)
            {
                terminalNode.nodeGameObject = terminalNode.growNode();
                return;
            }

            if(childBranches.Any())
            {
                foreach(Branch childBranch in childBranches)
                {
                    childBranch.GrowBranch(ageStep);
                }
                return;
            }

            foreach (Node prototypeTerminalNode in prototype.terminalNodes)
            {
                int lookupTableLastKey = NodesLookupTable.nodesDictionary.Last().Key;
                Branch childBranch = AttachBranch(terminalNode.id, lookupTableLastKey + 1, prototypeTerminalNode); 
                childBranches.Add(childBranch);
            }            
        }
    }

    private Branch AttachBranch(int rootNodeId, int newNodeId, Node branchPrototypeTerminalNode)
    {
        Node newBranchRootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(rootNodeId);

        Node newNode = new Node(branchPrototypeTerminalNode);
        newNode.id = newNodeId;
        newNode.parentNodeId = rootNodeId;  
        newNode.nodeGameObject = newNode.instantiateNode(newBranchRootNode.nodeGameObject.transform);
        NodesLookupTable.nodesDictionary.Add(newNodeId, newNode);

        Branch childBranch = new Branch()
        {
            prototype = BranchPrototypesInstances.basicBranchPrototype,
            maxAge = maxAge, //set other maxAge, now is max age of whole tree
            currentAge = 0.0f,
            rootNode = newBranchRootNode, 
            terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(newNodeId), 
            childBranches = new List<Branch>()
        };

        return childBranch;
    }
}
