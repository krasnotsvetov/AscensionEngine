using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;

namespace MonogameSamples.Engine.Core.Components
{
    public class Sprite : EntityDrawableComponent
    {

        private SpriteBatch spriteBatch;
        public Sprite(IGameComponent parentComponent) : base(parentComponent)
        {

        }

        public override void Initialize()
        {
            spriteBatch = ((ParentComponent as Entity).Scene.scene2DDrawer as Scene2DDrawer).SpriteBatch;
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            
            Vector2 position = new Vector2((ParentComponent as Entity).GlobalTransform.Position.X, (ParentComponent as Entity).GlobalTransform.Position.Y);
            spriteBatch.Draw(Material.textures[0], position, Color.White);
            base.Draw(gameTime);
        }
    }
}
