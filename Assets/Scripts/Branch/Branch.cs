using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch
{
    public BranchPrototype prototype { get; set; }
    public float maxAge { get; set; }
    public float currentAge { get; set; }
    public BranchSegment branchBase { get; set; }
    public List<Branch> childBranches { get; set; }

    public Branch()
    {
        prototype = new BranchPrototype();
        maxAge = maxAge;
        currentAge = 0.0f;
        branchBase = branchBase;
        childBranches = new List<Branch>();
    }

    public void drawBranch()
    {
        Gizmos.DrawSphere(branchBase.branchSegmentBase.position, 0.125f);
        Gizmos.DrawSphere()
    }
}
