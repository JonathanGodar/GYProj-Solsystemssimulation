using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulation
{
    public class Planet
    {
        // Planet properties
        public double Mass { get; set; }

        public VectorD3 Velocity { get; set; }

        public VectorD3 Position { get; set; }

        public Planet(VectorD3 pos, VectorD3 vel, double mass){
            this.Mass = mass;
            this.Velocity = vel;
            this.Position = pos;
        }

        public void Update(double dt, VectorD3 force){
            // Using the formula F = ma => a = F/m
            VectorD3 acceleration = force / Mass;

            // Using the formula v = v0 + a*t
            Velocity += acceleration;

            // Using the formula s = s0 + v*t + 1/2*a*t^2
            Position += Velocity * dt + acceleration * dt * dt / 2;
        }        

    } 
}
