using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpecies : MonoBehaviour
{
    public float pMax { get; set; } /// Max age
	public float gp { get; set; }  /// Growth Rate
    public float g1 { get; set; }   /// Tropism decrease by age
	public float g2 { get; set; }   /// Tropism strength overall
    public float beta { get; set; } /// Scaling Coefficient

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
