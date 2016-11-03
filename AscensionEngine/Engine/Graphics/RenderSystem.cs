﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Graphics.SceneSystem;
using Ascension.Engine.Graphics.Filters;
using Microsoft.Xna.Framework.Content;
using AscensionEngine.Engine.Graphics.CameraSystem;
using AscensionEngine.Engine.Core.Systems;

namespace Ascension.Engine.Graphics
{
    public class RenderSystem : IDrawableSystem
    {

        public GraphicsDevice Device { get { return device; } }
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public Scene ActiveScene
        {
            get { return activeScene; }
            set
            {
                activeScene = value;
                if (activeScene != null)
                {
                    if (activeScene.Cameras.Count > 0)
                    {
                        ActiveCamera = activeScene.Cameras[0];
                    }
                }
            }
        }
        private Scene activeScene;

        public Camera ActiveCamera { get; set; }


        //public Dictionary<string, Effect> Shaders = new Dictionary<string, Effect>();

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

        public bool Enabled { get; set; } = true;

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


        public virtual void Initialize()
        {
            lightFilter = new DefferedLightFilter(this);
            lightFilter.Initialize();
           
            foreach (var gameComponent in gameComponents.Values)
            {
                gameComponent.Initialize();
            }
        }


        public virtual void LoadContent(ContentManager contentManager)
        { 
            lightFilter.LoadContent(contentManager);

            foreach (var gameComponent in gameComponents.Values)
            {
                gameComponent.LoadContent(contentManager);
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

        public virtual void Draw(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }
            if (isDirty)
            {
                isDirty = false;
                _gameComponents.Sort();
            }

            if (ActiveCamera == null)
            {
                return;
            }
            foreach (var gameComponent in _gameComponents)
            {
                gameComponent.Draw(ActiveCamera.View, ActiveCamera.Projection, gameTime);
            }



            device.SetRenderTarget(null);
            SceneRenderer sceneDrawer = ActiveScene?.sceneRenderer;
            if (sceneDrawer == null)
            {
                return;
            }


            spriteBatch.Begin();
            spriteBatch.Draw(sceneDrawer.DiffuseTexture, Vector2.Zero, Color.White);
            spriteBatch.End();
            lightFilter.Render(sceneDrawer.DiffuseTexture, sceneDrawer.NormalMapTexture, sceneDrawer.LightMapTexture, sceneDrawer.Scene.Lights);

           
        } 


        public virtual void Dispose()
        {
            foreach (var gameComponent in _gameComponents)
            {
                gameComponent.Dispose();
            }
        }
    }


    public class RenderSystemException : Exception
    {
        public RenderSystemException(string textMessage) : base(textMessage)
        {

        }
    }
}
