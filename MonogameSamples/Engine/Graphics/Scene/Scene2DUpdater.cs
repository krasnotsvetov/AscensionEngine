using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;

namespace MonogameSamples.Engine.Graphics.Scene
{
    public class Scene2DUpdater : UpdateableComponent
    {
        private Scene2D scene;

        public Scene2DUpdater(Scene2D scene)
        {
            this.scene = scene;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // DrawSceneRecursive(scene, gameTime);
            foreach (var entity in scene.Entities)
            {
                UpdateEntityRecursive(entity, gameTime);
            }
            base.Update(gameTime);
        }

        

        private void UpdateEntityRecursive(Entity entity, GameTime gameTime)
        {
            foreach (var e in entity.Entities)
            {
                UpdateEntityRecursive(e, gameTime);
            }

            UpdateEntity(entity, gameTime);
        }

        private void UpdateEntity(Entity entity, GameTime gameTime)
        {
            foreach (var component in entity.UpdateableComponents)
            {
                if (component.Enabled)
                {
                    component.Update(gameTime);
                }
            }
        }
    }
}
