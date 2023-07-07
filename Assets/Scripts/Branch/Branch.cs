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

    public void drawBranch()
    {
        //Gizmos.DrawSphere(root.position, 0.125f);
        //Gizmos.DrawLine(root.position, branchBase.branchSegmentEnd.position);

        //if(!childBranches.Any())
        //{
        //    //Gizmos.DrawSphere(branchBase.branchSegmentEnd.position, 0.125f);
        //    //return;
        //}

        //foreach (Branch branch in childBranches)
        //{
        //    branch.drawBranch();
        //}
    }

    public void GrowBranch(float ageStep)
    {
         Debug.Log("Growing");
        if(prototype != null)
        {
            float newAge = terminalNode.age + ageStep;

            // TODO to improve when adding branches shredding and blooming
            //bool isBecomingMature = newAge >= prototype.maturityAge ? rootNode.age < prototype.maturityAge : false;
            //bool decay = rootNode.age >= prototype.maturityAge;

            // branch current age is not changed

            if (newAge < prototype.maturityAge)
            {
                terminalNode.nodeGameObject = terminalNode.growNode(ageStep);
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

    private Branch AttachBranch(int rootNodeId, int terminalNodeId, Node branchPrototypeTerminalNode)
    {
        Node terminalNode = branchPrototypeTerminalNode;
        terminalNode.id = terminalNodeId;
        terminalNode.parentNodeId = rootNodeId;
        terminalNode.nodeGameObject = terminalNode.instantiateNode();
        NodesLookupTable.nodesDictionary.Add(terminalNodeId, terminalNode);

        Branch childBranch = new Branch()
        {
            prototype = BranchPrototypesInstances.basicBranchPrototype,
            maxAge = maxAge, //set other maxAge, now is max age of whole tree
            currentAge = 0.0f,
            rootNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(rootNodeId), 
            terminalNode = NodesLookupTable.nodesDictionary.GetValueOrDefault(terminalNodeId),
            childBranches = new List<Branch>()
        };

        return childBranch;
    }
}
