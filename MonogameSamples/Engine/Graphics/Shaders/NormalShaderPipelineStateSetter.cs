using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameSamples.Engine.Graphics.Shaders
{
    public class NormalShaderPipelineStateSetter : IPipelineStateSetter
    {

        public NormalShaderPipelineStateSetter()
        {
        }

        private Effect effect;
        public void Initialize(Effect effect)
        {
            this.effect = effect;
        }

        public void Set(RenderSystem renderSystem, Material material)
        {
            effect.Parameters["NormalMap"].SetValue(material.Textures[1]);
        }
    }
}
