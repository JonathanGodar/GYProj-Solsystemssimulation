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


            //Debug.Log(1 + 2 * Mass * Energy() * Position.LengthSquared() / AngularMomentum().LengthSquared());
            //Debug.Log(Eccentricity());
            //Debug.Log(Energy());
            //Debug.Log(AngularMomentum());

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
            //return 0;
            return R0() / (1 - Eccentricity() * Math.Cos(angle));
        }

        private double Eccentricity()
        {
            return Math.Sqrt(Math.Abs(1 + 2 * Mass * Energy() * Position.LengthSquared() / AngularMomentum().LengthSquared()));
            //return Math.Sqrt(Math.Abs(1 + 2 * Mass * Energy() * Math.Pow(R0(), 2) / AngularMomentum().LengthSquared()));
            //return Math.Sqrt(Math.Abs(1 + 2 * Energy() * AngularMomentum().LengthSquared() /(Mass * Math.Pow(PlanetarySimulation.G * p1.Mass * p2.Mass, 2))));
        }

        private double Energy()
        {
            return 1 / 2 * Mass * Velocity.LengthSquared() - PlanetarySimulation.G * p1.Mass * p2.Mass / Position.Length();
        }

        private double R0()
        {
            return Position.Length();
            //return AngularMomentum().LengthSquared() / (Mass * PlanetarySimulation.G * p1.Mass * p2.Mass);
            //return AngularMomentum() / (Mass * p1.Mass * p2.Mass * PlanetarySimulation.G);
        }


        //public double Eccentricity() {

        //    return Math.Sqrt(2 * TotalEnergy() * R0() / (PlanetarySimulation.G * p1.Mass * p2.Mass) + 1);

        //}

        //public double R0() {
        //    return AngularMomentum().LengthSquared() / (Mass * PlanetarySimulation.G * p1.Mass * p2.Mass);
        //}

        //public double TotalEnergy() {
        //    return KineticEnergy() - PotentialEnergy();
        //}

        //private double KineticEnergy()
        //{
        //    return Mass * Velocity.LengthSquared() / 2; 
        //}

        ///// <summary>
        ///// Räknar ut den potensiella energin från origo
        ///// </summary>
        ///// <returns></returns>
        //private double PotentialEnergy()
        //{
        //    return PlanetarySimulation.G * p1.Mass * p2.Mass / Position.LengthSquared();
        //}
    }
}
