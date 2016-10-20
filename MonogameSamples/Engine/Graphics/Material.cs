using Microsoft.Xna.Framework.Graphics; 
using System.Collections.Generic; 

namespace MonogameSamples.Engine.Graphics
{
    public class Material
    {
        public delegate void ShaderParamsSetter(Material material);
        public Effect effect;
        public List<Texture2D> textures = new List<Texture2D>();
        public ShaderParamsSetter shaderParamsSetter;

        public Material(IEnumerable<Texture2D> textureCollection, Effect effect = null, ShaderParamsSetter shaderParamsSetter = null)
        {
            textures.AddRange(textureCollection);
            this.effect = effect;
            this.shaderParamsSetter = shaderParamsSetter;
        }


        public void ApplyMaterial()
        {
            if (effect != null)
            {

                if (shaderParamsSetter != null)
                {
                    shaderParamsSetter(this);
                }
            }
        }

    }
}
