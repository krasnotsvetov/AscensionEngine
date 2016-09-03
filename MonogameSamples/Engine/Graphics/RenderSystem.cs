using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq; 
using MonogameSamples.Engine.Core.Common;

namespace MonogameSamples.Engine.Graphics
{
    public class RenderSystem : DrawableComponent
    {

        public GraphicsDevice Device { get { return device; } }
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


        private bool isDirty = false;

        public RenderSystem()
        {

        }



        public RenderSystem(IEnumerable<KeyValuePair<string, DrawableComponent>> gameComponents)
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



        public override void Initialize()
        {
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
        }
    }


    public class RenderSystemException : Exception
    {
        public RenderSystemException(string textMessage) : base(textMessage)
        {

        }
    }
}
