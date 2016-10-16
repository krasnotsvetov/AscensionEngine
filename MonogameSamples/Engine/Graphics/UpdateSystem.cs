using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonogameSamples.Engine.Graphics
{
    public class UpdateSystem : UpdateableComponent
    {

        public GraphicsDevice Device { get { return device; } }
        public List<UpdateableComponent> GameComponents
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
        private List<UpdateableComponent> _gameComponents = new List<UpdateableComponent>();

        private Dictionary<string, UpdateableComponent> gameComponents = new Dictionary<string, UpdateableComponent>();
        private GraphicsDevice device;

        private bool isDirty = false;

        public UpdateSystem()
        {

        }



        public UpdateSystem(IEnumerable<KeyValuePair<string, UpdateableComponent>> gameComponents)
        {
            gameComponents.ToList().ForEach(pair =>
            {
                if (this.gameComponents.ContainsKey(pair.Key))
                {
                    this.gameComponents.Add(pair.Key, pair.Value);
                    _gameComponents.Add(pair.Value);
                    pair.Value.UpdateOrderChanged += (s, e) => isDirty = true;
                }
                else
                {
                    throw new UpdateSystemException("Double definiton of renderSystem component");
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

        public void AddComponent(KeyValuePair<string, UpdateableComponent> updateableComponent)
        {
            if (gameComponents.ContainsKey(updateableComponent.Key))
            {
                throw new UpdateSystemException("Double definiton of renderSystem component");
            }
            updateableComponent.Value.Initialize();
            gameComponents.Add(updateableComponent.Key, updateableComponent.Value);
            _gameComponents.Add(updateableComponent.Value);
            updateableComponent.Value.UpdateOrderChanged += (s, e) => isDirty = true;
            isDirty = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (isDirty)
            {
                isDirty = false;
                _gameComponents.Sort();
            }

            foreach (var gameComponent in _gameComponents)
            {
                gameComponent.Update(gameTime);
            }
        }
    }


    public class UpdateSystemException : Exception
    {
        public UpdateSystemException(string textMessage) : base(textMessage)
        {

        }
    }
}
