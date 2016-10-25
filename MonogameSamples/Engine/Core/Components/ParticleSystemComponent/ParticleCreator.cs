using MonogameSamples.Engine.Core.Common.Collections.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Core.Components.ParticleSystemComponent
{
    public class ParticleCreator : IObjectCreator<Particle2D>
    {
        ParticleSystem2D system;
        public ParticleCreator(ParticleSystem2D system)
        {
            this.system = system;
        }

        public Particle2D Create()
        {
            return new Particle2D(system);
        }
    }
}
