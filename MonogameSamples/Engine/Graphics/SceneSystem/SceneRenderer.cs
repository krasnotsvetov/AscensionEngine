using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components.ParticleSystemComponent;
using MonogameSamples.Engine.Graphics.Shaders;

namespace MonogameSamples.Engine.Graphics.SceneSystem
{
    public class SceneRenderer : DrawableComponent
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


        public Scene Scene
        {
            get { return scene; }
        }

        public SpriteBatch SpriteBatch { get { return spritebatch; } }
        public RenderSystem RenderSystem { get { return renderSystem; } }

        private RenderSystem renderSystem;

        private Scene scene;
        private SpriteBatch spritebatch;
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D diffuse;
        private RenderTarget2D depth;
        private RenderTarget2D normalMap;
        private RenderTarget2D lightMap;
        private RenderTarget2D volumeLightMask;




        public SceneRenderer(Scene scene, RenderSystem renderSystem)
        {
            this.renderSystem = renderSystem;
            graphicsDevice = renderSystem.Device;
            spritebatch = renderSystem.SpriteBatch;
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


            base.Initialize();
        }

        Material lastMaterial = null;


        public override void LoadContent(ContentManager contentManager)
        {
            clearEffect = contentManager.Load<Effect>("Shaders\\clearEffect");
            base.LoadContent(contentManager);
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
                    var setter = edc.Material.ShaderReference != null ? scene.Shaders[edc.Material.ShaderReference] : null;
                    //TODO
#warning COPYPASTE
                    spritebatch.Begin(effect: setter?.Effect);
                    setter?.Set(RenderSystem, edc.Material);
                    lastMaterial = edc.Material;
                }

                if (component is ParticleSystem2D)
                {
                    lastMaterial = null; // 
                    spritebatch.End();
                    Vector3 p = Vector3.Transform(Vector3.One, entity.GlobalTransform.World);
                    //TODO
#warning COPYPASTE
                    var setter = edc.Material.ShaderReference != null ? scene.Shaders[edc.Material.ShaderReference] : null;

                    spritebatch.Begin(blendState: BlendState.AlphaBlend, effect: setter?.Effect, transformMatrix: entity.GlobalTransform.World); // Mul Camera Matrix TODO , before implement camera class.
                    setter?.Set(RenderSystem, edc.Material);

                }


                if (component.Visible)
                {
                    component.Draw(gameTime);
                }
            }
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
