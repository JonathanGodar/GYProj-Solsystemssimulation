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

        public VectorD3 CurrentForce { get; set; } = VectorD3.Zero;

        public void ApplyCurrentForce(double timeStep)
        {

            VectorD3 acceleration = CurrentForce / Mass;
            VectorD3 newVel = Velocity + acceleration * timeStep;

            // s = vt + (at^2)/2
            Position += Velocity * timeStep + acceleration * timeStep * timeStep / 2;
            Velocity = newVel;
        }

        public Planet(VectorD3 position, VectorD3 velocity, double mass)
        {
            Velocity = velocity;
            Position = position;
            Mass = mass;
        }
    }

}
