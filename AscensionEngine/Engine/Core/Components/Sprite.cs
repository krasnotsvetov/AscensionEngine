using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using Ascension.Engine.Core.Common.Attributes;
using System;
using System.Runtime.Serialization;

namespace Ascension.Engine.Core.Components
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
            transform = ParentEntity.GlobalTransform;
            spriteBatch = ParentEntity.Scene.sceneRenderer.SpriteBatch;
        }

        public override void Draw(Matrix view, Matrix projection, GameTime gameTime)
        {
            
            Vector2 position = new Vector2(ParentEntity.GlobalTransform.Position.X, ParentEntity.GlobalTransform.Position.Y);
            if (Material.Textures[0] != null)
            {
                spriteBatch.Draw(Material.Textures[0], position, new Rectangle(0, 0, Material.Textures[0].Width, Material.Textures[0].Height), Color.White, transform.Rotation.Z, Vector2.Zero, new Vector2(transform.Scale.X, transform.Scale.Y), SpriteEffects.None, 0f);
            }
            base.Draw(view, projection, gameTime);
        }

        public override string ToString()
        {
            return "Sprite";
        }


        internal override void RenderSystemChange()
        {
            base.RenderSystemChange();

            spriteBatch = ParentEntity.Scene.sceneRenderer.SpriteBatch;
        }
    }
}
