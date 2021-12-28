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

		[SerializeField] Planet p1Initial, p2Initial;

		Planet p1, p2;

		GameObject distanceVisualizer, p1Visualizer, p2Visualizer;

		[SerializeField] double stepsPerFrame = 1;

		StreamWriter graphWriter;
		StringBuilder graph;

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


		VectorD3 VectorFromAngleAndRadius(double angle, double radius){
			return new VectorD3(radius * Math.Cos(angle), radius * Math.Sin(angle));
		}

		void DrawAtAngle(double angle)
		{
			double radius = GetDistanceFromRadius(angle);
			Debug.Log(angle + " " + radius);

			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go.GetComponent<Transform>().localScale = new Vector3(.1f, .1f, .1f);
			go.transform.position = (Vector3)(VectorFromAngleAndRadius(angle, radius));
			go.GetComponent<Renderer>().material.color = Color.red;
		}
		private void DrawAnalyticalPath()
		{
			for(double angle = 0; angle < Math.PI * 2; angle += Math.PI / 10)
			{
				DrawAtAngle(angle);
			}
		}
		#endregion

		void InitiateGraphs(){
			graph = new StringBuilder();
			graph.AppendLine("\"Tid\", \"Total energi\", \"Vridmoment\", \"Avstånd från analytisk lösning\"");

			graphWriter = new StreamWriter("Assets/graphs/graph.csv", false);
		}

		GameObject coolThingVisualizer;

		// Start is called before the first frame update
		void Start()
		{
			p1 = p1Initial;
			p2 = p2Initial;


			// Instantiate the visualizers
			p1Visualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			p2Visualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);


			coolThingVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			coolThingVisualizer.GetComponent<Renderer>().material.color = Color.blue;

			distanceVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			// Make the distance visualizer red
			distanceVisualizer.GetComponent<Renderer>().material.color = Color.red;

			DrawAnalyticalPath();
			InitiateGraphs();
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
			timePassed += deltaTime;
		}



		double timePassed = 0;


		double calculateTotalEnergy(Planet p1, Planet p2){
			return calculateKineticEnergy(p1, p2) + calculatePotentialEnergy(p1, p2);
		}


		double calculateAngularMomentum(Planet p1, Planet p2){
			return VectorD3.Cross(p1.position - p2.position, p1.velocity - p2.velocity).magnitude;
		}

		// Update is called once per frame
		// void Update()
		// {
		// 	for (int i = 0; i < stepsPerFrame; i++)
		// 	{
		// 		StepTime(timeStep, CalculateGravitationalForce(p1, p2));
		// 	}

		// 	DrawVisualizers();

		// 	// Log data
		// 	var totalEnergy = calculateTotalEnergy(p1, p2);
		// 	var angularMomentum = calculateAngularMomentum(p1, p2);
		// 	var distanceFrom = calculateDistanceFromAnalytical(p1, p2);
		// 	Debug.Log(distanceFrom);
		// 	graph.AppendLine($"{timePassed}, {totalEnergy}, {angularMomentum}, {distanceFrom}");

		// 	if(Input.GetKeyDown(KeyCode.Space)) {
		// 		FinalizeGraphs();
		// 		Destroy(this);
		// 	}
		// }

		double calculateDistanceFromAnalytical(Planet p1, Planet p2){
			double angle = Math.Atan2(p1.position.y - p2.position.y, p1.position.x - p2.position.x);
			double currentDistance = (p1.position - p2.position).magnitude;

			return GetDistanceFromRadius(angle) - currentDistance;
		}


		void FinalizeGraphs(){
			WriteCache();
			graphWriter.Close();
		}

		double calculatePotentialEnergy(Planet p1, Planet p2)
		{
			return -G * p1.mass * p2.mass / (p1.position - p2.position).magnitude;
		}

		double calculateKineticEnergy(Planet p1, Planet p2)
		{
			return p1.mass * p1.velocity.LengthSquared() + p2.mass * p2.velocity.LengthSquared() / 2;
		}

		private void DrawVisualizers()
		{
			DrawCoolThing();
			DrawDistance();
			DrawPlanetVisualizers();
		}

		private void DrawCoolThing(){
			double currAngle = Math.Atan2(-p1.position.y + p2.position.y, -p1.position.x + p2.position.x);
			double distance = GetDistanceFromRadius(currAngle);
			coolThingVisualizer.transform.position = (Vector3)(VectorFromAngleAndRadius(currAngle, distance));
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
			Debug.Log(graph);
			graphWriter.Write(graph.ToString());
			graph.Clear();
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

		public double keneticEnergy => velocity.sqrMagnitude * mass / 2;

		public Planet(VectorD3 position, VectorD3 velocity, double mass) : this()
		{
			this.position = position;
			this.velocity = velocity;
			this.mass = mass;
		}
	}
}