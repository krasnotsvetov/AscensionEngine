using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics.Scene;

namespace MonogameSamples.Engine.Graphics
{
 

    public class Entity
    {

        public List<UpdateableComponent> UpdateableComponents
        {
            get
            {
                if (isUpdateDirty)
                {
                    isUpdateDirty = false;
                    updateableComponents.Sort();
                }
                return updateableComponents;
            }
        }
        public List<Entity> Entities { get { return enities; } }
        public Scene2D Scene { get { return scene; } }

        private Dictionary<string, IGameComponent> components = new Dictionary<string, IGameComponent>();

        private List<DrawableComponent> drawableComponents = new List<DrawableComponent>();
        private List<UpdateableComponent> updateableComponents = new List<UpdateableComponent>();

        private List<Entity> enities = new List<Entity>();

        public Transform Transform;

        private bool isDrawDirty = false;
        private bool isUpdateDirty = false;

        private Scene2D scene;

        public Entity(Scene2D scene)
        {
            this.scene = scene;
            Transform = new Transform(this);

        }

        public List<DrawableComponent> DrawableComponents
        {
            get
            {
                if (isDrawDirty)
                {
                    isDrawDirty = false;
                    drawableComponents.Sort();
                }
                return drawableComponents;
            }
        }



        public IGameComponent GetComponent(string name)
        {
            if (components.ContainsKey(name))
            {
                return components[name];
            }
            return null;
        }


        public void AddDrawableComponent(string name, DrawableComponent component)
        {
            if (components.ContainsKey(name))
            {
                //TODO
                throw new Exception();
            }
            component.Initialize();
            component.DrawOrderChanged += (s, e) => isDrawDirty = true;
            components.Add(name, component);
            drawableComponents.Add(component);
            isDrawDirty = true;
        }

        public void AddUpdateableComponent(string name, UpdateableComponent component)
        {
            if (components.ContainsKey(name))
            {
                //TODO
                throw new Exception();
            }
            component.Initialize();
            component.UpdateOrderChanged += (s, e) => isUpdateDirty = true;
            components.Add(name, component);
            updateableComponents.Add(component);
            isUpdateDirty = true;
        }


 

         
    }
}
