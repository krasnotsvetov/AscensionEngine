using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Graphics.Shaders
{
    public interface IPipelineStateSetter
    {

        void Initialize(Effect effect);
        void Set(RenderSystem renderSystem, Material material);
    }
}
