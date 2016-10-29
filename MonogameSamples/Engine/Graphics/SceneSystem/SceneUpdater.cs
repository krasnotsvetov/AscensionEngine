using Microsoft.Xna.Framework;
using Ascension.Engine.Core.Common;

namespace Ascension.Engine.Graphics.SceneSystem
{
    public class SceneUpdater : UpdateableComponent
    {

        public Scene Scene
        {
            get { return scene; }
        }

        private Scene scene;

        public SceneUpdater(Scene scene)
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
