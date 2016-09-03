using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.Scene;

namespace MonogameSamples.Engine.Core.Components
{
    public class Sprite : EntityDrawableComponent
    {

        private SpriteBatch spriteBatch;
        public Sprite(Entity entity) : base(entity)
        {

        }

        public override void Initialize()
        {
            spriteBatch = (this.Entity.Scene.scene2DDrawer as Scene2DDrawer).SpriteBatch;
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = new Vector2(Entity.Transform.position.X, Entity.Transform.position.Y);
            spriteBatch.Draw(Material.textures[0], position, Color.White);
            base.Draw(gameTime);
        }
    }
}
