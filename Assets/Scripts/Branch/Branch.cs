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

    public Branch()
    {
        prototype = new BranchPrototype();
        maxAge = maxAge;
        currentAge = 0.0f;
        childBranches = new List<Branch>();
    }

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
        if(prototype != null)
        {
            float newAge = rootNode.age + ageStep;

            // TODO to improve when adding branches shredding and blooming
            bool isBecomingMature = newAge >= prototype.maturityAge ? rootNode.age < prototype.maturityAge : false;
            bool decay = rootNode.age >= prototype.maturityAge;

            if (newAge < prototype.maturityAge)
            {
                terminalNode.growNode(ageStep);
                return;
            }

            if (!childBranches.Any()) // if is mature and doesn't have yet child branches
            {
                // TODO create root node for each branch module, assign its rotation based on calculations 
            }
            
        }
    }
}
