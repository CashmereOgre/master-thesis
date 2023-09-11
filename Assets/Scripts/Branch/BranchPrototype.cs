using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPrototype
{
    public float maturityAge { get; set; }
    public Node rootNode { get; set; }
    public Node centerNode { get; set; }
    public List<Node> terminalNodes { get; set; }
}


