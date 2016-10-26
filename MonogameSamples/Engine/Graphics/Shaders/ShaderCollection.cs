using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Graphics.Shaders
{
    internal class ShaderCollection : StringReferenceCollection<ShaderReference, Pair<Effect, IPipelineStateSetter>>
    {

    }
}
