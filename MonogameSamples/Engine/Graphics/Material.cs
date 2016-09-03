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

        public Material(Texture2D texture, Effect effect = null, ShaderParamsSetter shaderParamsSetter = null)
        {
            textures.Add(texture);
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
