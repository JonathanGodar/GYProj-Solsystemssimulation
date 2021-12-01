using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{


    public class ReducedPlanet 
    {

        Planet p1;
        Planet p2;

        // planet 1 - planet 2


        public ReducedPlanet(Planet p1, Planet p2)
        {
            // Kan också vara p2 - p1?
            Mass = p1.Mass * p2.Mass / (p1.Mass + p2.Mass);
            Position = p1.Position - p2.Position;
            Velocity = p1.Velocity - p2.Velocity;

            this.p1 = p1;
            this.p2 = p2;

            Debug.Log(Velocity);
            Debug.Log(Position);
            Debug.Log(Mass);
            Debug.Log(AngularMomentum());

        }
        
        public double Mass { get; set; }

        public VectorD3 Velocity { get; set; }

        public VectorD3 Position { get; set; }

        

        /// <summary>
        /// Räknar ut Rörelsemängdsmoment runt origo 
        /// </summary>
        /// <returns></returns>
        public VectorD3 AngularMomentum() {
            return VectorD3.Cross(Position, Mass * Velocity);
        }


        public double RadiusFromAngle(double angle)
        {
            // TODO Kolla vad theta 0 är
            return R0() / (1 + Eccentricity() * Math.Cos(angle));
        }


        public double Eccentricity() {

            return Math.Sqrt(2 * TotalEnergy() * R0() / (PlanetarySimulation.G * p1.Mass * p2.Mass) + 1);

        }

        public double R0() {
            return AngularMomentum().LengthSquared() / (Mass * PlanetarySimulation.G * p1.Mass * p2.Mass);
        }

        public double TotalEnergy() {
            return KineticEnergy() - PotentialEnergy();
        }

        private double KineticEnergy()
        {
            return Mass * Velocity.LengthSquared() / 2; 
        }

        /// <summary>
        /// Räknar ut den potensiella energin från origo
        /// </summary>
        /// <returns></returns>
        private double PotentialEnergy()
        {
            return PlanetarySimulation.G * p1.Mass * p2.Mass / Position.LengthSquared();
        }
        

    }
}
