using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Node : MonoBehaviour
{
    Node parentNode;
    List<Node> childrenNodes;

    PlantSpecies PlantSpecies;

    Vector3 posision;
    Vector3 direction;

    float age;
    float maxLength;

    float thickness;
    float baseRadius;

    int rigIndex;

    bool root;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
