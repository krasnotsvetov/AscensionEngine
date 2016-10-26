using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core.Common;

namespace MonogameSamples.Engine.Graphics.Shaders
{
    public class ShaderReference : BaseReference<string>
    {
        public ShaderReference()
        {

        }

        protected ShaderReference(string name) : base(name)
        {

        }


        public static ShaderReference FromIdentifier(string name)
        {
            return new ShaderReference(name);
        }
    }
}
