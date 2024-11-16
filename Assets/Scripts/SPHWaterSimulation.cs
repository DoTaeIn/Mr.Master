using System.Collections.Generic;
using UnityEngine;

public class SPHWaterSimulation : MonoBehaviour
{
    public int numParticles = 100;                // Number of particles
    public float particleMass = 1.0f;             // Mass of each particle
    public float smoothingRadius = 0.5f;          // Smoothing radius
    public float stiffness = 200.0f;              // Stiffness of the fluid
    public float restDensity = 1000.0f;           // Rest density of water
    public float viscosity = 0.1f;                // Viscosity of water
    public Vector2 gravity = new Vector2(0, -9.81f);  // Gravity force

    private List<Particle> particles;             // List to store particles

    void Start()
    {
        particles = new List<Particle>();
        for (int i = 0; i < numParticles; i++)
        {
            // Initialize particles at random positions within a certain area
            Vector2 position = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-1.0f, 5.0f));
            particles.Add(new Particle(position, particleMass));
        }
    }

    void Update()
    {
        // Compute density and pressure for each particle
        foreach (var particle in particles)
        {
            ComputeDensityAndPressure(particle);
        }

        // Compute forces for each particle
        foreach (var particle in particles)
        {
            ComputeForces(particle);
        }

        // Integrate the equations of motion
        foreach (var particle in particles)
        {
            Integrate(particle);
        }

        // Display particles
        foreach (var particle in particles)
        {
            // Draw each particle as a small circle for visualization
            Debug.DrawLine(particle.position, particle.position + Vector2.up * 0.05f, Color.blue, 0.05f);
        }
    }

    void ComputeDensityAndPressure(Particle particle)
    {
        particle.density = 0;
        foreach (var neighbor in particles)
        {
            float distance = Vector2.Distance(particle.position, neighbor.position);
            if (distance < smoothingRadius)
            {
                // Kernel function to compute density
                float q = distance / smoothingRadius;
                particle.density += particle.mass * (1 - q) * (1 - q) * (1 - q);  // Cubic Spline Kernel
            }
        }

        // Calculate pressure based on density
        particle.pressure = stiffness * (particle.density - restDensity);
    }

    void ComputeForces(Particle particle)
    {
        Vector2 pressureForce = Vector2.zero;
        Vector2 viscosityForce = Vector2.zero;

        foreach (var neighbor in particles)
        {
            if (neighbor == particle) continue;

            float distance = Vector2.Distance(particle.position, neighbor.position);
            if (distance < smoothingRadius)
            {
                // Pressure force calculation (based on SPH principles)
                float q = distance / smoothingRadius;
                Vector2 direction = (neighbor.position - particle.position).normalized;
                pressureForce -= direction * (particle.pressure + neighbor.pressure) / (2 * neighbor.density) * (1 - q);

                // Viscosity force calculation
                viscosityForce += viscosity * (neighbor.velocity - particle.velocity) * (1 - q);
            }
        }

        // Gravity force
        Vector2 gravityForce = gravity * particle.mass;

        // Total force on the particle
        particle.force = pressureForce + viscosityForce + gravityForce;
    }

    void Integrate(Particle particle)
    {
        // Euler integration for updating particle positions and velocities
        particle.velocity += particle.force / particle.density * Time.deltaTime;
        particle.position += particle.velocity * Time.deltaTime;

        // Basic boundary conditions to keep particles in view
        if (particle.position.y < -5) { particle.position.y = -5; particle.velocity.y *= -0.5f; }
        if (particle.position.x < -5 || particle.position.x > 5) { particle.velocity.x *= -0.5f; }
    }
    
    void OnDrawGizmos()
    {
        if (particles == null) return;

        Gizmos.color = Color.blue;
        foreach (var particle in particles)
        {
            Gizmos.DrawSphere(particle.position, 0.05f);  // Adjust size for visibility
        }
    }


    // Particle class to store properties of each particle
    private class Particle
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 force;
        public float density;
        public float pressure;
        public float mass;

        public Particle(Vector2 pos, float mass)
        {
            position = pos;
            this.mass = mass;
            velocity = Vector2.zero;
            force = Vector2.zero;
            density = 0;
            pressure = 0;
        }
    }
}
