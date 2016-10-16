using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameSamples.Engine.Core.Components.ParticleSystemComponent
{
    //Primitive pool

    public class ParticlePool2D
    {
        private ParticleSystem2D particleSystem;
        private List<Particle2D> particles;
        public ParticlePool2D(int size, ParticleSystem2D particleSystem)
        {
            particles = new List<Particle2D>(size);
            this.particleSystem = particleSystem;
        }


        public Particle2D GetParticle()
        {
            if (particles.Count > 0)
            {
                Particle2D p = particles[particles.Count - 1];
                particles.RemoveAt(particles.Count - 1);
                return p;
            } else
            {
                return new Particle2D(particleSystem);
            }
        }

        public void FreeParticle(Particle2D particle)
        {
            CleanParticle(particle);
            particles.Add(particle);
        }


        private void CleanParticle(Particle2D particle)
        {

        }

    }
}
