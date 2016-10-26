using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Graphics.Shaders
{
    [DataContract]
    public class ParticleShaderPipelineStateSetter : IPipelineStateSetter
    {
        [DataMember]
        public string ShaderName { get; set; }

        public ParticleShaderPipelineStateSetter()
        {

        }

        public Effect Effect { get; set; }

        public void Initialize(Effect effect)
        {
            this.Effect = effect;
            this.ShaderName = effect.Name;
        }

        public void Set(RenderSystem renderSystem, Material material)
        {
            //effect.Parameters["ScreenWidth"].SetValue((float)renderSystem.Device.Viewport.Width);
            //effect.Parameters["ScreenHeight"].SetValue((float)renderSystem.Device.Viewport.Height);
            Effect.Parameters["NormalMap"].SetValue(material.Textures[1]);
        }
    }
}
