using System;
using System.Collections;
using System.Collections.Generic;
using Simulation;
using UnityEngine;
using System.Text;
using System.IO;

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

		StreamWriter timeGraphWriter;
		StringBuilder timeGraph;

		// Calculate the gravitational force between two objects
		public VectorD3 CalculateGravitationalForce(Planet p1, Planet p2)
		{
			return (p1.position - p2.position).normalized * /* Force is: */ G * p1.mass * p2.mass / (p1.position - p2.position).sqrMagnitude;
		}


		#region For analytical method
		public double GetDistanceFromRadius(double radians)
		{
			return r0 / (1 - eccentricity * Math.Cos(radians));
		}

		double r0 => (p1Initial.position - p2Initial.position).magnitude;

		double eccentricity => Math.Sqrt(1 + 2 * initialEnergy * Math.Pow(initialAngularMomentum, 2) / (reducedMass * Math.Pow((G * p1Initial.mass * p2Initial.mass), 2)));

		double sqrInitialRelativeVelocity => (p1Initial.velocity - p2Initial.velocity).sqrMagnitude;
		double initialRelativeVelocity => Math.Sqrt(sqrInitialRelativeVelocity);

		double initialAngularMomentum => VectorD3.Cross(p1Initial.position - p2Initial.position, reducedMass * (p1Initial.velocity - p2Initial.velocity)).magnitude;

		double initialEnergy => reducedMass / 2 * sqrInitialRelativeVelocity - G * p1Initial.mass * p2Initial.mass / r0;


		double reducedMass => (p1Initial.mass * p2Initial.mass) / (p1Initial.mass + p2Initial.mass);

		double angleZero => Math.PI; // Math.Atan2(p1Initial.position.y - p2Initial.position.y, p1Initial.position.x - p2Initial.position.x);

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

		void InitiateGraphs(){
			timeGraph = new StringBuilder();
			timeGraph.Append("Tid, Total energi, Vridmoment");

			timeGraphWriter = new StreamWriter("Assets/graphs/timeGraph.csv", false);
		}

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
				"\nRelative velocity: " + initialRelativeVelocity +
				"\nAngular momentum: " + initialAngularMomentum +
				"\nEnergy: " + initialEnergy +
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



		float timePassed = 0;


		double calculateTotalEnergy(Planet p1, Planet p2){
			return calculateKeneticEnergy(p1, p2) + calculatePotentialEnergy(p1, p2);
		}


		double calculateAngularMomentum(Planet p1, Planet p2){
			return VectorD3.Cross(p1.position - p2.position, p1.velocity - p2.velocity).magnitude;
		}

		// Update is called once per frame
		void Update()
		{
			for (int i = 0; i < stepsPerFrame; i++)
			{
				StepTime(timeStep, CalculateGravitationalForce(p1, p2));
			}

			DrawVisualizers();

			// Log data
			timeGraph.AppendLine(timePassed + "," + calculateTotalEnergy(p1, p2) + ", " + calculateAngularMomentum(p1, p2));

			Debug.Log("Enweri OwO");
			Debug.Log(p1.velocity.sqrMagnitude * p1.mass / 2 + p2.velocity.sqrMagnitude * p2.mass / 2 - G * p1.mass * p2.mass / (p1.position - p2.position).magnitude);

			Debug.Log("OwO Awular Mowentum");
			Debug.Log(VectorD3.Cross(p1.position - p2.position, reducedMass * (p1.velocity - p2.velocity)).magnitude);
		}


		double calculatePotentialEnergy(Planet p1, Planet p2)
		{
			return -G * p1.mass * p2.mass / (p1.position - p2.position).magnitude;
		}

		double calculateKeneticEnergy(Planet p1, Planet p2)
		{
			return p1.mass * p1.velocity.LengthSquared() + p2.mass * p2.velocity.LengthSquared() / 2;
		}

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

		private void WriteCache(){
			timeGraphWriter.Write(timeGraph);
			timeGraph.Clear();
		}

		void OnDestory(){
			WriteCache();
			timeGraphWriter.Close();
		}
	}

	public struct Planet
	{
		[SerializeField]
		public VectorD3 position;

		[SerializeField]
		public VectorD3 velocity;

		[SerializeField]
		public double mass;

		public double keneticEnergy => velocity.sqrMagnitude * mass / 2;

		public Planet(VectorD3 position, VectorD3 velocity, double mass) : this()
		{
			this.position = position;
			this.velocity = velocity;
			this.mass = mass;
		}
	}
}