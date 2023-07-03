using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Branch
{
    public BranchPrototype prototype { get; set; }
    public float maxAge { get; set; }
    public float currentAge { get; set; }
    public Node root { get; set; }
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
}
