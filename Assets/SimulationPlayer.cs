using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation;

public class SimulationPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        sim = new PlanetarySimulation();
    }

    PlanetarySimulation sim;

    // Update is called once per frame
    void Update()
    {
        sim.Update(3600);
        Debug.Log(sim.DebugInfo());

        
    }
}
