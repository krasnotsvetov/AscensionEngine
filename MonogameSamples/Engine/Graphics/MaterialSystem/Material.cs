using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Graphics.MaterialSystem;
using MonogameSamples.Engine.Graphics.Shaders;
using System.Collections.Generic;

namespace MonogameSamples.Engine.Graphics
{
    public class Material
    {
        public ShaderReference ShaderReference;
        public List<Texture2D> textures = new List<Texture2D>();
        public IMaterialParameters Parameters;
        public IMaterialFlags Flags;

        public Material(IEnumerable<Texture2D> textureCollection, ShaderReference shaderReference)
        {
            this.ShaderReference = shaderReference;
            textures.AddRange(textureCollection);
        }
    }
}
