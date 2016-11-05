using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension.Engine.Graphics.Filters
{
    public class Filter : IDisposable
    {

        public RenderSystem RenderSystem { get; }
        protected SpriteBatch spriteBatch;
        protected float width;
        protected float height;

        public Filter(RenderSystem renderSystem)
        {
            RenderSystem = renderSystem;
        }


        public virtual void Initialize()
        {
            this.width = RenderSystem.Device.Viewport.Width;
            this.height = RenderSystem.Device.Viewport.Height;
            spriteBatch = new SpriteBatch(RenderSystem.Device);
        }

        public virtual void LoadContent()
        {

        }


        public virtual void Dispose()
        {

        }

    }
}
