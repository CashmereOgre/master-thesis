using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    int id;
    Plant plant;
    BranchPrototype prototype;
    float maxAge;
    float currentAge;
    Node root;
    Branch parentBranch;
    List<Branch> childrenBranches;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
