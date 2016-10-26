using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;
using MonogameSamples.Engine.Core.Common.Attributes;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Components
{
    [DataContract]
    [Component("Sprite")]
    public class Sprite : EntityDrawableComponent
    {
       
        private SpriteBatch spriteBatch;

        private Transform transform;

        public Sprite(string name, MaterialReference reference) : base(name, reference)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            transform = (ParentComponent as Entity).GlobalTransform;
            spriteBatch = (ParentComponent as Entity).Scene.sceneRenderer.SpriteBatch;
        }

        public override void Draw(GameTime gameTime)
        {
            
            Vector2 position = new Vector2((ParentComponent as Entity).GlobalTransform.Position.X, (ParentComponent as Entity).GlobalTransform.Position.Y);
            spriteBatch.Draw(Material.Textures[0], position, new Rectangle(0, 0, Material.Textures[0].Width, Material.Textures[0].Height), Color.White, transform.Rotation.Z, Vector2.Zero, new Vector2(transform.Scale.X, transform.Scale.Y), SpriteEffects.None, 0f);
            base.Draw(gameTime);
        }

        public override string ToString()
        {
            return "Sprite";
        }


        internal override void RenderSystemChange()
        {
            base.RenderSystemChange();

            spriteBatch = (ParentComponent as Entity).Scene.sceneRenderer.SpriteBatch;
        }
    }
}
