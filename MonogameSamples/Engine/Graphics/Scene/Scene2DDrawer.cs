using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core;
using MonogameSamples.Engine.Core.Common;

namespace MonogameSamples.Engine.Graphics.Scene
{
    public class Scene2DDrawer : DrawableComponent
    {

        public SpriteBatch SpriteBatch { get { return spritebatch; } }
        private Scene2D scene;
        private SpriteBatch spritebatch;
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D diffuse;
        private RenderTarget2D depth;
        private RenderTarget2D normalMap;
        private RenderTarget2D light;

        public Scene2DDrawer(Scene2D scene)
        {
            graphicsDevice = GameInfo.GraphicsDevice;
            spritebatch = new SpriteBatch(graphicsDevice);
            this.scene = scene;
        }

        public override void Initialize()
        {
            diffuse = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            depth = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            base.Initialize();
        }

        Material lastMaterial = null;

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.SetRenderTargets(diffuse, depth);
            graphicsDevice.Clear(Color.CornflowerBlue);
            lastMaterial = null;
            spritebatch.Begin();
            // DrawSceneRecursive(scene, gameTime);
            foreach (var entity in scene.Entities)
            {
                DrawEntityRecursive(entity, gameTime);
            }
            spritebatch.End();
            graphicsDevice.SetRenderTargets(null);
            spritebatch.Begin();
            spritebatch.Draw(diffuse, Vector2.Zero, Color.White);
            spritebatch.End();
            base.Draw(gameTime);
        }

        /*private void DrawSceneRecursive(Scene2D scene, GameTime gameTime)
        {
           
            foreach (var s in scene.Scenes)
            {
                DrawSceneRecursive(s, gameTime);
            }
            DrawScene(scene, gameTime);
        }

        private void DrawScene(Scene2D scene, GameTime gameTime)
        {

            foreach (var entity in scene.Entities)
            {
                DrawEntityRecursive(s, gameTime);
            }

        }*/

        private void DrawEntityRecursive(Entity entity, GameTime gameTime)
        {
            foreach (var e in entity.Entities)
            {
                DrawEntityRecursive(e, gameTime);
            }

            DrawEntity(entity, gameTime);
        }

        private void DrawEntity(Entity entity, GameTime gameTime)
        {

            foreach (var component in entity.DrawableComponents)
            {
                var edc = (component as EntityDrawableComponent);
                if (!ReferenceEquals(lastMaterial, edc.Material))
                {
                    spritebatch.End();
                    spritebatch.Begin(effect: edc.Material.effect);
                    edc.Material.ApplyMaterial();
                    lastMaterial = edc.Material;
                }
                if (component.Visible)
                {
                    component.Draw(gameTime);
                }
            }
        }
    }
}
