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

        double radius;
        double angle;

        Planet p1 = new Planet(new VectorD3(0, 5, 0), new VectorD3(0.0031, 0, 0), 2_000_000);
        Planet p2 = new Planet(new VectorD3(0, -5, 0), new VectorD3(-0.0031, 0, 0), 2_000_000);

        ReducedPlanet p = new ReducedPlanet(p1, p2);

        for (int i = 0; i < ln.positionCount; i++)
        {
            angle = i * angleStep;
            radius = p.RadiusFromAngle(angle); // getRadius(angle);

            ln.SetPosition(i, new Vector3(
                (float)(Math.Cos(angle) * radius),
                (float)(Math.Sin(angle) * radius), 0
                 ));
        }

        //var a = new VectorD3(1, 2, 3);
        //var b = new VectorD3(4, 5, 6);
        //var a = new VectorD3(2, -1, 3);
        //var b = new VectorD3(5, 7, -4);
        //Debug.Log(VectorD3.Cross(a, b));
    }

    //double getRadius(double angle) {
    //    double elipson = 0.9;
    //    double r0 = 2;
    //    double beginningAngle = 0;
    //    return r0 / (1 + elipson * Math.Cos(angle - beginningAngle));
    //}




    // Update is called once per frame
    void Update()
    {

    }
}
