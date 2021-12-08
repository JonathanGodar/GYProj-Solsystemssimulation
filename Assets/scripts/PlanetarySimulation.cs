using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class PlanetarySimulation
    {

        // Källa värde för G
        public const double G = 0.000000000066743;  //6.674 * Math.Pow(10, -11);


        public static Planet p1 = new Planet(new VectorD3(0, 10, 0), new VectorD3(0.0031, 0, 0), 2_000_000);
        public static Planet p2 = new Planet(new VectorD3(0, -10, 0), new VectorD3(-0.0031, 0, 0), 2_000_000);

        private List<Planet> planets = new List<Planet>();
        public PlanetarySimulation()
        {
            //Planet p1 = new Planet(new VectorD3(5, 0, 0), new VectorD3(0, 0.0031, 0), 2_000_000);
            //Planet p2 = new Planet(new VectorD3(-5, 0, 0), new VectorD3(0, -0.0031, 0), 2_000_000);

            //Planet earth = new Planet(new VectorD3(149597871000, 0, 0), new VectorD3(30_200), 5.972 * Math.Pow(10, 24));
            //Planet sun = new Planet(VectorD3.Zero, VectorD3.Zero, 1.989 * Math.Pow(10, 30));


            //planets.Add(earth);
            //planets.Add(sun);



            //Planet p1 = new Planet(new VectorD3(0, 5, 0), new VectorD3(0.0031, 0, 0), 2_000_000);
            //Planet p2 = new Planet(new VectorD3(0, -5, 0), new VectorD3(-0.0031, 0, 0), 2_000_000);


            planets.Add(p1);
            planets.Add(p2);
        }

        public string DebugInfo()
        {
            return planets[0].Position.X + " " + planets[1].Position.X;
        }


        public List<Planet> Planets
        {
            get => planets;
        }


        public void Update(double timeStep)
        {
            CalculateForces();
            StepPlanets(timeStep);
            //planets.ForEach((p) => Debug.Log(p));
        }


        public void StepPlanets(double timeStep)
        {
            planets.ForEach((p) => p.ApplyCurrentForce(timeStep));
        }


        public void CalculateForces()
        {
            ClearForces();

            double force;
            Planet a, b;
            VectorD3 bToA;
            for (int i = 0; i < planets.Count - 1; i++)
            {
                a = planets[i];

                for (int j = i + 1; j < planets.Count; j++)
                {
                    b = planets[j];
                    force = GravitationalForceBetween(a, b);

                    bToA = (a.Position - b.Position).Normalized();
                    a.CurrentForce -= bToA * force;
                    b.CurrentForce += bToA * force;
                }
            }

        }

        public VectorD3 CenterOfMass()
        {

            VectorD3 sum = new VectorD3();
            double totalMass = 0;
            foreach (var p in planets)
            {
                sum += p.Mass * p.Position;
                totalMass += p.Mass;
            }
            return sum / totalMass;
        }

        public void ClearForces()
        {
            planets.ForEach((p) => p.CurrentForce = VectorD3.Zero);
        }

        public static Double GravitationalForceBetween(Planet a, Planet b)
        {
            // Källa Newtons gravitations lag. 
            return G * a.Mass * b.Mass / VectorD3.DistanceSquared(a.Position, b.Position);
        }


    }
}
