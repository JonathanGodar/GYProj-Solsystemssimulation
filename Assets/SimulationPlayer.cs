using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation;
using System;

public class SimulationPlayer : MonoBehaviour
{


    [SerializeField] GameObject planetPrefab;

    [SerializeField] GameObject sphere;


    [SerializeField] float timeStep = 0.1f;
    [SerializeField] int stepsPerFrame = 250;

    [SerializeField] int initialFrameSteps = 250 * 60 * 60 * 20;


    List<GraphicalPlanet> GPlanets = new List<GraphicalPlanet>();

    // Start is called before the first frame update
    void Start()
    {

        //sim = new PlanetarySimulation();


        //foreach (Planet p in sim.Planets)
        //{
        //    GameObject go = Instantiate(planetPrefab);
        //    var gplanet = go.GetComponent<GraphicalPlanet>();
        //    gplanet.SetPlanet(p);
        //}

        //centerOfMass = Instantiate(sphere);

        //for(int i = 0; i < initialFrameSteps; i++)
        //{
        //    sim.Update(timeStep);
        //}

        VectorD3 a = new VectorD3(-2, 6.5, 3);
        Vector3 ar = new Vector3(-2f, 6.5f, 3f);

        VectorD3 b = new VectorD3(32, -3.2222, 23);
        Vector3 br = new Vector3(-32, -3.2222f, 23f);



        //Debug.Log(new Vector3(2, 3, 4) * new Vector3(2, 3, 4));

        //Debug.Log(a * b);
        //Debug.Log(ar * br);

        //Debug.Log(ar * br);


        //Debug.Log()
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
