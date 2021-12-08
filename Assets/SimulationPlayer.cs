using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation;
using System;

public class SimulationPlayer : MonoBehaviour
{


    [SerializeField] GameObject planetPrefab;

    [SerializeField] GameObject sphere;


    [SerializeField] float timeStep = 0.01f;
    [SerializeField] int stepsPerFrame = 250;

    [SerializeField] int initialFrameSteps = 250 * 60 * 60 * 20;


    List<GraphicalPlanet> GPlanets = new List<GraphicalPlanet>();

    // Start is called before the first frame update
    void Start()
    {

        sim = new PlanetarySimulation();


        foreach (Planet p in sim.Planets)
        {
            GameObject go = Instantiate(planetPrefab);
            var gplanet = go.GetComponent<GraphicalPlanet>();
            gplanet.SetPlanet(p);
        }

        centerOfMass = Instantiate(sphere);

        for (int i = 0; i < initialFrameSteps; i++)
        {
            sim.Update(timeStep);
        }
    }


    GameObject centerOfMass;

    PlanetarySimulation sim;


    Action remove; 

    // Update is called once per frame
    void Update()
    {


        //if(remove != null)
        //{
        //    remove(); 
        //}


        for (int i = 0; i < stepsPerFrame; i++)
        {
            sim.Update(timeStep);
        }

        centerOfMass.GetComponent<Transform>().position = (Vector3)((sim.Planets[0].Position * sim.Planets[0].Mass + sim.Planets[1].Position * sim.Planets[1].Mass) / (sim.Planets[0].Mass + sim.Planets[1].Mass));
        GetComponent<Transform>().position = (Vector3)sim.CenterOfMass() - new Vector3(0, 0, 50);
    }
}
