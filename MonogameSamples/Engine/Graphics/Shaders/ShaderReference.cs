using Microsoft.Xna.Framework.Graphics;
using Ascension.Engine.Core.Common;

namespace Ascension.Engine.Graphics.Shaders
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
