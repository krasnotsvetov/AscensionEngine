using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Systems.Content;

namespace Ascension.Engine.Graphics.SceneSystem
{
    public class SceneRenderer : DrawableComponent
    {


        public delegate void OnDrawStartDelegate();

        public OnDrawStartDelegate OnDrawStart { get; set; }

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

        public Vector3 Ambient;

        private RenderSystem renderSystem;

        private Scene scene;
        private SpriteBatch spritebatch;
        private GraphicsDevice graphicsDevice;
        private RenderTarget2D diffuse;
        private RenderTarget2D depth;
        private RenderTarget2D normalMap;
        private RenderTarget2D lightMap;
        private RenderTarget2D volumeLightMask;

        private BasicEffect basicEffect;


        public SceneRenderer(Scene scene, RenderSystem renderSystem)
        {
            basicEffect = new BasicEffect(renderSystem.Device);
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


        public override void LoadContent()
        {
            clearEffect = ContentContainer.Instance().GetEffect("Engine\\Shaders\\ClearEffect");
            base.LoadContent();
        }

        public override void Draw(Matrix view, Matrix projection, GameTime gameTime)
        {

            graphicsDevice.SetRenderTargets(depth, diffuse, normalMap, lightMap);

            clearEffect.Parameters["Ambient"].SetValue(Ambient);
            spritebatch.Begin(effect: clearEffect);
            spritebatch.Draw(clearTexture, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            spritebatch.End();

            OnDrawStart?.Invoke();

            lastMaterial = null;
            currentEffect = null;
            foreach (var entity in scene.Entities)
            {

                DrawEntityRecursive(view, projection, entity, gameTime);
            }


            base.Draw(view, projection, gameTime);
        } 
        private void DrawEntityRecursive(Matrix view, Matrix projection, Entity entity, GameTime gameTime)
        {
            foreach (var e in entity.Entities)
            {
                DrawEntityRecursive(view, projection, e, gameTime);
            }

            DrawEntity(view, projection, entity, gameTime);
        }


        Effect currentEffect;

        private void DrawEntity(Matrix view, Matrix projection, Entity entity, GameTime gameTime)
        {

            foreach (var component in entity.DrawableComponents)
            {
                
                var edc = (component as EntityDrawableComponent);

                if (edc.Material == null)
                {
                    continue;
                }

                if (lastMaterial == null || !lastMaterial.MaterialName.Equals(edc.MaterialName))
                {
                    currentEffect = edc.Material.Effect;
                    lastMaterial = edc.Material;
                    if (currentEffect == null)
                    {
                        continue;
                    }
                    foreach (var p in currentEffect.Parameters)
                    {
                        if (p.ParameterType == EffectParameterType.Texture2D)
                        {
                            if (edc.Material.Textures.ContainsKey(p.Name))
                            {
                                p.SetValue(edc.Material.Textures[p.Name]);
                            } else
                            {
                                p.SetValue(default(Texture2D));
                            }
                        }
                    }
                }
               

                if (component.Visible)
                {
                    renderSystem.Device.BlendState = BlendState.AlphaBlend;
                    renderSystem.Device.DepthStencilState = DepthStencilState.Default;
                    renderSystem.Device.SamplerStates[0] = SamplerState.LinearClamp;
                    currentEffect.Parameters["World"].SetValue(entity.GlobalTransform.World);
                    currentEffect.Parameters["View"].SetValue(view);
                    currentEffect.Parameters["Projection"].SetValue(projection);
                    currentEffect.CurrentTechnique.Passes[0].Apply();
                    component.Draw(view, projection, gameTime);
                }
            }
        }

        public override void Dispose()
        {
            basicEffect.Dispose();
            base.Dispose();
        }
    }
}
