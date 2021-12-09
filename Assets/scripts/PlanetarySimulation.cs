using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
	public class PlanetarySimulation
	{

		// K�lla v�rde f�r G
		public const double G = 0.000000000066743;  //6.674 * Math.Pow(10, -11);

		public Planet p1, p2;


		public PlanetarySimulation()
		{
			p1 = new Planet(new VectorD3(-1, 0, 0), new VectorD3(0, 0.005, 0),	 200_000);
			p2 = new Planet(new VectorD3(1, 0, 0), new VectorD3(0, -0.005, 0), 200_000);
		}

		public string DebugInfo()
		{
			return "Write debug func";
			// return planets[0].Position.X + " " + planets[1].Position.X;
		}

		public void Update(double timeStep)
		{
			// Calculate force from p1 to p2
			VectorD3 force = CalculateForce(p1, p2);

			// Update planets 
			p1.Update(timeStep, -1 * force);
			p2.Update(timeStep, force);
		}

		private VectorD3 CalculateForce(Planet p1, Planet p2)
		{
			// Using Newton's law of universal gravitation
			// F = G * m1 * m2 / r^2
			// F = G * m1 * m2 / (r1 - r2)^2

			return G * p1.Mass * p2.Mass / (p1.Position - p2.Position).LengthSquared() * (p1.Position - p2.Position).Normalized();
		}


		public static Double GravitationalForceBetween(Planet a, Planet b)
		{
			// K�lla Newtons gravitations lag. 
			return G * a.Mass * b.Mass / VectorD3.DistanceSquared(a.Position, b.Position);
		}


	}
}
