using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components.ParticleSystemComponent;

namespace MonogameSamples.Engine.Graphics.SceneSystem
{
    public class Scene2DDrawer : DrawableComponent
    {


        public RenderTarget2D DiffuseTexture
        {
            get { return diffuse; }
        }

        public RenderTarget2D DepthTexture
        {
            get { return depth; }
        }

        public RenderTarget2D NormalMapTexture
        {
            get { return normalMap; }
        }

        public RenderTarget2D LightMapTexture
        {
            get { return lightMap; }
        }


        public Scene2D Scene
        {
            get { return scene; }
        }

        public SpriteBatch SpriteBatch { get { return spritebatch; } }
        private Scene2D scene;
        private SpriteBatch spritebatch;
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D diffuse;
        private RenderTarget2D depth;
        private RenderTarget2D normalMap;
        private RenderTarget2D lightMap;
        private RenderTarget2D volumeLightMask;

        public Scene2DDrawer(Scene2D scene)
        {
            graphicsDevice = GameInfo.GraphicsDevice;
            spritebatch = new SpriteBatch(graphicsDevice);
            this.scene = scene;
        }

        private Texture2D clearTexture;
        private Effect clearEffect;

        public override void Initialize()
        {
            diffuse = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            depth = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, false, graphicsDevice.Adapter.CurrentDisplayMode.Format, DepthFormat.Depth24);
            normalMap = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            lightMap = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            clearTexture = new Texture2D(graphicsDevice, 1, 1);
            clearTexture.SetData<Color>(new Color[] { Color.White });

            clearEffect = GameInfo.Content.Load<Effect>("Shaders\\clearEffect");

            base.Initialize();
        }

        Material lastMaterial = null;

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Draw(GameTime gameTime)
        {

            graphicsDevice.SetRenderTargets(depth, diffuse, normalMap, lightMap);

            spritebatch.Begin(effect: clearEffect);
            spritebatch.Draw(clearTexture, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            spritebatch.End();
            
            lastMaterial = null;
             spritebatch.Begin();
             // DrawSceneRecursive(scene, gameTime);
             foreach (var entity in scene.Entities)
             {

                 DrawEntityRecursive(entity, gameTime);
             }
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

                if (edc.Material == null)
                {
                    continue;
                }

                if (!ReferenceEquals(lastMaterial, edc.Material))
                {
                    spritebatch.End();
                    spritebatch.Begin(effect: edc.Material.effect);
                    edc.Material.ApplyMaterial();
                    lastMaterial = edc.Material;
                }

                if (component is ParticleSystem2D)
                {
                    lastMaterial = null; // 
                    spritebatch.End();
                    Vector3 p = Vector3.Transform(Vector3.One, entity.GlobalTransform.World);
                    spritebatch.Begin(blendState: BlendState.AlphaBlend, effect: edc.Material.effect, transformMatrix: entity.GlobalTransform.World); // Mul Camera Matrix TODO , before implement camera class.
                }


                if (component.Visible)
                {
                    component.Draw(gameTime);
                }
            }
        }
    }
}
