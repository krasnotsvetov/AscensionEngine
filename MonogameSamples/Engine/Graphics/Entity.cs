using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System.Collections.ObjectModel;

namespace MonogameSamples.Engine.Graphics
{


    public class Entity : IGameComponent
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
        public ReadOnlyCollection<Entity> Entities { get { return enities.AsReadOnly(); } }
        public Scene2D Scene { get { return scene; } }

        private Dictionary<string, IGameComponent> components = new Dictionary<string, IGameComponent>();

        private List<DrawableComponent> drawableComponents = new List<DrawableComponent>();
        private List<UpdateableComponent> updateableComponents = new List<UpdateableComponent>();

        private List<Entity> enities = new List<Entity>();

        public Transform Transform { get { return transform; } }
        public Transform GlobalTransform
        {
            get
            {
                if (isGlobalTransformDirty)
                {
                    Transform temp = transform.ToGlobal();
                    globalTransform.UpdateTransform(temp.Position, temp.Rotation, temp.Scale);
                    isGlobalTransformDirty = false;
                    foreach (var ent in Entities)
                    {
                        ent.isGlobalTransformDirty = true;
                    }
                }
                return globalTransform;
            }
        }
        public Transform ParentTrasform { get { return parentTransform; } }

        public Entity Parent { get { return parent; } }

        private bool isGlobalTransformDirty = true;


        

        private Entity parent;
        private bool isDrawDirty = false;
        private bool isUpdateDirty = false;

        private Scene2D scene;
        private Transform transform;
        private Transform parentTransform;
        private Transform globalTransform;




        public Entity(Scene2D scene)
        {
            parent = null;
            this.scene = scene;
            transform = new Transform(this);
            globalTransform = new Transform(this);
            parentTransform = null;
           
 


            transform.PositionChanged += (s, e) => {
                isGlobalTransformDirty = true;
                
            };

            transform.ScaleChanged += (s, e) => {
                isGlobalTransformDirty = true;
            };


            transform.RotationChanged += (s, e) => {
                isGlobalTransformDirty = true;
            };

        }




        public void Initialize()
        {
            //throw new NotImplementedException();
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

        public void AddEntity(Entity entity)
        { 
            scene.Entities.Remove(entity);

            entity.parentTransform = transform;
            entity.scene = scene;
            entity.parent = this;
            enities.Add(entity);
        }
    }
}
