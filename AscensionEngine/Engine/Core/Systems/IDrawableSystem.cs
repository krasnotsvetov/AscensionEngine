using Ascension.Engine.Core.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscensionEngine.Engine.Core.Systems
{
    public interface IDrawableSystem : IDisposable
    {
        void Initialize();
        void LoadContent(ContentManager Content);
        void Draw(GameTime gameTime);
        List<DrawableComponent> GameComponents { get; }
        bool Enabled { get; set; }
    }
}
