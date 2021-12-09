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


    // List<GraphicalPlanet> GPlanets = new List<GraphicalPlanet>();

    GraphicalPlanet gp1, gp2;


    
    GameObject relativeDistanceIndicator;



    // Start is called before the first frame update
    void Start()
    {

        sim = new PlanetarySimulation();

        GameObject temp = Instantiate(planetPrefab);
        gp1 = temp.GetComponent<GraphicalPlanet>();
        gp1.SetPlanet(sim.p1);

        temp = Instantiate(planetPrefab);
        gp2 = temp.GetComponent<GraphicalPlanet>();
        gp2.SetPlanet(sim.p2);

        DrawPath.QueueDraw(sim);

        relativeDistanceIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Set the distance indicator to be yellow
        relativeDistanceIndicator.GetComponent<Renderer>().material.color = Color.yellow;
    }


    GameObject centerOfMass;

    PlanetarySimulation sim;


    Action remove; 

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stepsPerFrame; i++)
        {
            sim.Update(timeStep * Time.deltaTime);
        }

        relativeDistanceIndicator.transform.position = (Vector3)(sim.p1.Position - sim.p2.Position); 
    }
}
