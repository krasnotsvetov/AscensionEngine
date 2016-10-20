using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Graphics.SceneSystem;
using MonogameSamples.Engine.Graphics.Filters;

namespace MonogameSamples.Engine.Graphics
{
    public class RenderSystem : DrawableComponent
    {

        public GraphicsDevice Device { get { return device; } }
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }


        public Dictionary<MaterialReference, Material> Materials { get; set; } = new Dictionary<MaterialReference, Material>();
        public Dictionary<string, Effect> Shaders = new Dictionary<string, Effect>();

        public  List<DrawableComponent> GameComponents
        {
            get
            {
                if (isDirty)
                {
                    _gameComponents.Sort();
                }
                return _gameComponents;
            }
        }
        private List<DrawableComponent> _gameComponents = new List<DrawableComponent>();

        private Dictionary<string, DrawableComponent> gameComponents = new Dictionary<string, DrawableComponent>();
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;


        private List<Filter> filters = new List<Filter>();

        private bool isDirty = false;

        public RenderSystem(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
            spriteBatch = new SpriteBatch(device);
        }



        public RenderSystem(GraphicsDevice graphicsDevice, IEnumerable<KeyValuePair<string, DrawableComponent>> gameComponents) : this(graphicsDevice)
        {
            gameComponents.ToList().ForEach(pair =>
            {
                if (this.gameComponents.ContainsKey(pair.Key))
                {
                    this.gameComponents.Add(pair.Key, pair.Value);
                    _gameComponents.Add(pair.Value);
                    pair.Value.DrawOrderChanged += (s, e) => isDirty = true;
                }
                else
                {
                    throw new RenderSystemException("Double definiton of renderSystem component");
                }
            });
            _gameComponents.Sort();
        }

        DefferedLightFilter lightFilter;


        public override void Initialize()
        {
            lightFilter = new DefferedLightFilter(this);
            lightFilter.Initialize();
           
            foreach (var gameComponent in gameComponents.Values)
            {
                gameComponent.Initialize();
            }
        }

        public void AddComponent(KeyValuePair<string, DrawableComponent> drawableComponent)
        {
            if (gameComponents.ContainsKey(drawableComponent.Key))
            {
                throw new RenderSystemException("Double definiton of renderSystem component");
            }
            drawableComponent.Value.Initialize();
            gameComponents.Add(drawableComponent.Key, drawableComponent.Value);
            _gameComponents.Add(drawableComponent.Value);
            drawableComponent.Value.DrawOrderChanged += (s, e) => isDirty = true;
            isDirty = true;
        }

        public override void Draw(GameTime gameTime)
        {
            if (isDirty)
            {
                isDirty = false;
                _gameComponents.Sort();
            }

            foreach (var gameComponent in _gameComponents)
            {
                gameComponent.Draw(gameTime);
            }



            device.SetRenderTarget(null);
            Scene2DDrawer sceneDrawer = (Scene2DDrawer)gameComponents.Values.FirstOrDefault(t => t is Scene2DDrawer);
            if (sceneDrawer == null)
            {
                return;
            }
            
            lightFilter.Render(sceneDrawer.DiffuseTexture, sceneDrawer.NormalMapTexture, sceneDrawer.LightMapTexture, sceneDrawer.Scene.Lights);

        }
    }


    public class RenderSystemException : Exception
    {
        public RenderSystemException(string textMessage) : base(textMessage)
        {

        }
    }
}
