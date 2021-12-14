using System;
using System.Collections;
using System.Collections.Generic;
using Simulation;
using UnityEngine;
using System.Collections;

namespace v3
{
	public class SimulationV3 : MonoBehaviour
	{
		[SerializeField] double timeStep = 0.01f;

		// The gravitational constant in newtons gravitational law
		public const double G = 0.000000000066743f;  //6.674 * Math.Pow(10, -11);
																								// public readonly double G = 0.0000000000667408f;

		[SerializeField] Planet p1Initial, p2Initial;

		Planet p1, p2;

		GameObject distanceVisualizer, p1Visualizer, p2Visualizer;

		[SerializeField] double stepsPerFrame = 1;


		// Calculate the gravitational force between two objects
		public VectorD3 CalculateGravitationalForce(Planet p1, Planet p2)
		{
			// // Calculate the distance between the two objects // VectorD3 distanceVec = p1.position - p2.position; // // Calculate the magnitude of the distance // double distanceMagnitude = distanceVec.magnitude; // // Calculate the gravitational force // double gravitationalForce = (G * p1.mass * p2.mass) / (distanceMagnitude * distanceMagnitude);

			// // Calculate the direction of the force
			// VectorD3 direction = distanceVec.normalized;

			// // Return the force
			// return direction * gravitationalForce;

			return (p1.position - p2.position).normalized * /* Force is: */ G * p1.mass * p2.mass / (p1.position - p2.position).sqrMagnitude;
		}


		#region For analytical method
		public double GetDistanceFromRadius(double radians)
		{
			return r0 / (1 - eccentricity * Math.Cos(radians));
		}

		double r0 => (p1Initial.position - p2Initial.position).magnitude;

		double eccentricity => Math.Sqrt(1 + 2 * energy * Math.Pow(angularMomentum, 2) / (reducedMass * Math.Pow((G * p1Initial.mass * p2Initial.mass), 2)));
		// double eccentricity => Math.Sqrt(1 + 2 * reducedMass * energy * Math.Pow(r0, 2) / Math.Pow(angularMomentum, 2));

		double sqlrRelativeVelocity => (p1Initial.velocity - p2Initial.velocity).sqrMagnitude;
		double relativeVelocity => Math.Sqrt(sqlrRelativeVelocity);

		double angularMomentum => VectorD3.Cross(p1Initial.position - p2Initial.position, reducedMass * (p1Initial.velocity - p2Initial.velocity)).magnitude;

		double energy => reducedMass / 2 * sqlrRelativeVelocity - G * p1Initial.mass * p2Initial.mass / r0;


		double reducedMass => (p1Initial.mass * p2Initial.mass) / (p1Initial.mass + p2Initial.mass);

		double angleZero =>  Math.PI;// Math.Atan2(p1Initial.position.y - p2Initial.position.y, p1Initial.position.x - p2Initial.position.x);

		void DrawAtAngle(double angle)
		{
			double radius = GetDistanceFromRadius(angle);

			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go.transform.position = (Vector3)(new VectorD3(radius * Math.Cos(angle + angleZero), radius * Math.Sin(angle + angleZero), 0));

			go.GetComponent<Renderer>().material.color = Color.red;
		}
		private void DrawAnalyticalPath()
		{


			for (double angle = angleZero; angle < Math.PI * 2 + angleZero; angle += Math.PI / 1000)
			{
				DrawAtAngle(angle);
			}
		}
		#endregion


		// Start is called before the first frame update
		void Start()
		{
			p1 = p1Initial;
			p2 = p2Initial;

			distanceVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);

			// Make the distance visualizer red
			distanceVisualizer.GetComponent<Renderer>().material.color = Color.red;

			// Instantiate the visualizers
			p1Visualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			p2Visualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);

			DrawAnalyticalPath();

			Debug.Log(
				"Eccentricity: " + eccentricity +
				"\nRelative velocity: " + relativeVelocity +
				"\nAngular momentum: " + angularMomentum +
				"\nEnergy: " + energy +
				"\nReduced mass: " + reducedMass +
				"\nR0: " + r0
			);
		}


		void UpdatePlanet(ref Planet p, VectorD3 force, double deltaTime)
		{
			// Calculate the acceleration: F = ma => a = F / m
			VectorD3 acceleration = force / p.mass;

			// v*t + 1/2at^2
			p.position += p.velocity * deltaTime + 0.5f * acceleration * deltaTime * deltaTime;

			// v + at
			p.velocity += acceleration * deltaTime;

		}


		void StepTime(double deltaTime, VectorD3 force)
		{
			// Use the euler method to find the planets new position
			UpdatePlanet(ref p1, -force, deltaTime);
			UpdatePlanet(ref p2, force, deltaTime);
		}

		// Update is called once per frame
		void Update()
		{
			for (int i = 0; i < stepsPerFrame; i++)
			{

				StepTime(timeStep, CalculateGravitationalForce(p1, p2));

			}
			DrawVisualizers();

	

			Debug.Log("Enweri OwO");
			Debug.Log(p1.velocity.sqrMagnitude * p1.mass / 2 + p2.velocity.sqrMagnitude * p2.mass / 2 - G * p1.mass * p2.mass / (p1.position - p2.position).magnitude);

			Debug.Log("OwO Awular Mowentum");
			Debug.Log()

		}

		// private double reducedMass

		private void DrawVisualizers()
		{
			DrawDistance();
			DrawPlanetVisualizers();
		}

		private void DrawPlanetVisualizers()
		{
			p1Visualizer.transform.position = (Vector3)p1.position;
			p2Visualizer.transform.position = (Vector3)p2.position;
		}

		private void DrawDistance()
		{
			distanceVisualizer.transform.position = (Vector3)(p1.position - p2.position);
		}
	}

	
	[Serializable]
	public struct Planet
	{
		[SerializeField]
		public VectorD3 position;
		
		[SerializeField]
		public VectorD3 velocity;

		[SerializeField]
		public double mass;

		public double keneticEnergy => velocity.sqrMagnitude  * mass / 2;

		public Planet(VectorD3 position, VectorD3 velocity, double mass) : this()
		{
			this.position = position;
			this.velocity = velocity;
			this.mass = mass;
		}
	}

}