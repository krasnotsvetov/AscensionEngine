using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Graphics.Shaders
{
    public interface IPipelineStateSetter
    {
        string ShaderName { get; set; }
        Effect Effect { get; set; }
        void Initialize(Effect effect);
        void Set(RenderSystem renderSystem, Material material);
    }
}
