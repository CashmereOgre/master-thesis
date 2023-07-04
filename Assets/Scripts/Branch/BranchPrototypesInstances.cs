using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPrototypesInstances
{
    BranchPrototype basicBranchPrototype = new BranchPrototype()
    {
        maturityAge = 0.5f,
        rootNode = new Node()
        {
            isRoot = true,
            position = Vector3.zero,
            rotation = Quaternion.identity,
            age = 0f,
            maxLength = 1,
            plantVariables = new PlantSpecies()
            {
                maxAge = 950f,
                gp = 0.12f,
                tropismMature = 0.2f,
                g2 = 0.87f,

            }
        }
    };
}
