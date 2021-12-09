using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Simulation;

public class DrawPath : MonoBehaviour
{
	public static Planet p1, p2;

	public static ReducedPlanet p;

	public static List<ReducedPlanet> drawQueue = new List<ReducedPlanet>();

	public static void QueueDraw(PlanetarySimulation sim)
	{
		p1 = sim.p1;
		p2 = sim.p2;

		p = new ReducedPlanet(p1, p2);

		drawQueue.Add(p);
	}

	void Update()
	{
        
		if (drawQueue.Count == 0)
			return;
		p = drawQueue[0];
		drawQueue.RemoveAt(0);

		LineRenderer ln = GetComponent<LineRenderer>();

		double angleStep = 0.001;
		ln.positionCount = (int)(Math.PI * 2 / angleStep);

		float radius;
		double angle;

		for (int i = 0; i < ln.positionCount; i++)
		{
			angle = i * angleStep;
			// TODO remove
			radius = (float)p.RadiusFromAngle(angle);

			ln.SetPosition(i, new Vector3(
					(float)(Math.Cos(angle) * radius),
					(float)(Math.Sin(angle) * radius), 0
					 ));
		}
	}

}
