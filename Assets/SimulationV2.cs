using System;
using System.Collections;
using System.Collections.Generic;
using Simulation;
using UnityEngine;
using System.Collections;

namespace v2
{

    // public class ExactPathV2 : MonoBehaviour 
    // {
    //     void Update(){
    //         Debug.Log("ExactPathV2");
    //     }
    // }

	public class SimulationV2 : MonoBehaviour
	{
        [SerializeField] float timeStep = 0.01f;

        // The gravitational constant in newtons gravitational law
		public const float G = 0.000000000066743f;  //6.674 * Math.Pow(10, -11);
        // public readonly float G = 0.0000000000667408f;

        [SerializeField] Planet p1, p2;


        [SerializeField] Planet test;


        GameObject distanceVisualizer, p1Visualizer, p2Visualizer;

        [SerializeField] float stepsPerFrame = 1;


        // Calculate the gravitational force between two objects
        public Vector3 CalculateGravitationalForce(Planet p1, Planet p2)
        {
            // Calculate the distance between the two objects
            Vector3 distance = p1.position - p2.position;

            // Calculate the magnitude of the distance
            float distanceMagnitude = distance.magnitude;

            // Calculate the gravitational force
            float gravitationalForce = (G * p1.mass * p2.mass) / (distanceMagnitude * distanceMagnitude);

            // Calculate the direction of the force
            Vector3 direction = distance.normalized;

            // Return the force
            return direction * gravitationalForce;
        }



		// Start is called before the first frame update
		void Start()
		{
			p1 = new Planet(new Vector3(-1, 0, 0), new Vector3(0, 0.005f, 0), 2_000_000);
			p2 = new Planet(new Vector3(1, 0, 0), new Vector3(0, -0.005f, 0), 2_000_000);

            // p1 = new Planet(new Vector3(1, 0, 0), Vector3.zero, 2_000_000);
            // p2 = new Planet(new Vector3(-1, 0, 0), Vector3.zero, 2_000_000);


            distanceVisualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            // Make the distance visualizer red
            distanceVisualizer.GetComponent<Renderer>().material.color = Color.red;

            // Instantiate the visualizers
            p1Visualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            p2Visualizer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		}

        void UpdatePlanet(ref Planet p, Vector3 force, float deltaTime){
            // Calculate the acceleration
            Vector3 acceleration = force / p.mass;


            // v*t + 1/2at^2
            p.position += p.velocity * deltaTime + 0.5f * acceleration * deltaTime * deltaTime;

            // v + at
            p.velocity += acceleration * deltaTime;
        }


		void StepTime(float deltaTime, Vector3 force)
        {
            // Use the euler method to find the planets new position
            UpdatePlanet(ref p1, -force, deltaTime);
            UpdatePlanet(ref p2, force, deltaTime);
        }

		// Update is called once per frame
		void Update()
		{
            for(int i = 0; i < stepsPerFrame; i++)
            {
                StepTime(timeStep * Time.deltaTime, CalculateGravitationalForce(p1, p2));
            }
            DrawVisualizers();
		}

		private void DrawVisualizers()
		{
            DrawDistance();
            DrawPlanetVisualizers();
		}

		private void DrawPlanetVisualizers()
		{
            p1Visualizer.transform.position = p1.position;
            p2Visualizer.transform.position = p2.position;
		}

		private void DrawDistance()
        {
            distanceVisualizer.transform.position = (Vector3)(p1.position - p2.position);
		}
	}

    public struct  Planet{
        // [System.Serializable]
        public Vector3 position;
        // [System.Serializable]
        public Vector3 velocity;

        // [System.Serializable]
        public float mass;

		public Planet(Vector3 position, Vector3 velocity, float mass) : this()
		{
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
		}
	}

}