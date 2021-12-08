using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Simulation;

public class DrawPath : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        LineRenderer ln = GetComponent<LineRenderer>();

        double angleStep = 0.001;
        ln.positionCount = (int)(Math.PI * 2 / angleStep);

        float radius;
        double angle;



        Planet p1 = PlanetarySimulation.p1;
        Planet p2 = PlanetarySimulation.p2;

        //Planet p1 = new Planet(new VectorD3(0, 5, 0), new VectorD3(0.0031, 0, 0), 2_000_000);
        //Planet p2 = new Planet(new VectorD3(0, -5, 0), new VectorD3(-0.0031, 0, 0), 2_000_000);

        ReducedPlanet p = new ReducedPlanet(p1, p2);

        for (int i = 0; i < ln.positionCount; i++)
        {
            angle = i * angleStep;
            // TODO remove
            radius = (float)p.RadiusFromAngle(angle);

            Console.WriteLine(radius);
            ln.SetPosition(i, new Vector3(
                (float)(Math.Cos(angle) * radius),
                (float)(Math.Sin(angle) * radius), 0
                 ));
        }

    }
}
