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
    public class NormalShaderPipelineStateSetter : IPipelineStateSetter
    {
        [DataMember]
        public string ShaderName { get; set; }

        public NormalShaderPipelineStateSetter()
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
            Effect.Parameters["NormalMap"].SetValue(material.Textures[1]);
        }
    }
}
