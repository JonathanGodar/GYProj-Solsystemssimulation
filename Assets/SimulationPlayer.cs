using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation;

public class SimulationPlayer : MonoBehaviour
{


    [SerializeField] GameObject planetPrefab;


    List<GraphicalPlanet> GPlanets = new List<GraphicalPlanet>();

    // Start is called before the first frame update
    void Start()
    {
        sim = new PlanetarySimulation();


        foreach(Planet p in sim.Planets)
        {
            GameObject go = Instantiate(planetPrefab);
            var gplanet = go.GetComponent<GraphicalPlanet>();
            gplanet.SetPlanet(p);
        }
    }

    PlanetarySimulation sim;

    // Update is called once per frame
    void Update()
    {
        sim.Update(1);
    }
}
