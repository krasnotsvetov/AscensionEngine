using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using System.Collections.ObjectModel;

namespace MonogameSamples.Engine.Graphics.SceneSystem
{


    public class Entity : IGameComponent
    {
         

       
        public ReadOnlyCollection<Entity> Entities { get { return entities.AsReadOnly(); } }
        public Scene2D Scene { get { return scene; } }

        private Dictionary<string, IGameComponent> components = new Dictionary<string, IGameComponent>();

        private List<EntityDrawableComponent> drawableComponents = new List<EntityDrawableComponent>();
        private List<EntityUpdateableComponent> updateableComponents = new List<EntityUpdateableComponent>();

        private List<Entity> entities = new List<Entity>();

        public Transform Transform { get { return transform; } }
        public Transform GlobalTransform
        {
            get
            {
                if (isGlobalTransformDirty)
                {
                    Transform temp = transform.ToGlobal();
                    ignoreChangeTransformEvent = true;
                    globalTransform.Position = temp.World.Translation;
                    globalTransform.Rotation = temp.Rotation;
                    globalTransform.Scale = temp.Scale;
                    ignoreChangeTransformEvent = false;
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


        public Guid Guid;
        public int ID = -1;

        private Entity parent;
        private bool isDrawDirty = false;
        private bool isUpdateDirty = false;

        private Scene2D scene;
        private Transform transform;
        private Transform parentTransform;
        private Transform globalTransform;
        private bool ignoreChangeTransformEvent = false;


        public Entity(Scene2D scene)
        {
            Guid = Guid.NewGuid();
            parent = null;
            this.scene = scene;
            transform = new Transform(this);
            globalTransform = new Transform(this);
            parentTransform = null;



            EventHandler<EventArgs> localTransformChanged = (s, e) => {
                if (!ignoreChangeTransformEvent)
                {
                    isGlobalTransformDirty = true;
                }
            };

            transform.PositionChanged += localTransformChanged;
            transform.ScaleChanged += localTransformChanged;
            transform.RotationChanged += localTransformChanged;

            globalTransform.PositionChanged += (s, e) =>
            {
                if (ignoreChangeTransformEvent)
                {
                    return;
                }
                ignoreChangeTransformEvent = true;
                Matrix pWorld = Parent == null ? Matrix.Identity : Parent.GlobalTransform.World;
                Vector3 lastPosition = Vector3.Transform((Vector3)s, Matrix.Invert(pWorld));
                Vector3 newPosition = Vector3.Transform(globalTransform.Position, Matrix.Invert(pWorld));
                Vector3 delta = newPosition - lastPosition;

                transform.Position += delta;
                ignoreChangeTransformEvent = false;
                foreach (Entity ent in entities)
                {
                    ent.isGlobalTransformDirty = true;
                }
            };

            globalTransform.RotationChanged += (s, e) =>
            {
                if (ignoreChangeTransformEvent)
                {
                    return;
                }
                ignoreChangeTransformEvent = true;
                Vector3 lastRotation = (Vector3)s;
                Vector3 newRotation = globalTransform.Rotation;
                Vector3 delta = newRotation - lastRotation;

                transform.Rotation += delta;
                ignoreChangeTransformEvent = false;
                foreach (Entity ent in entities)
                {
                    ent.isGlobalTransformDirty = true;
                }
            };

            globalTransform.ScaleChanged += (s, e) =>
            {
                if (ignoreChangeTransformEvent)
                {
                    return;
                }
                ignoreChangeTransformEvent = true;
                Vector3 lastScale = (Vector3)s;
                Vector3 newScale = globalTransform.Scale;
                Vector3 delta = new Vector3(newScale.X / lastScale.X, newScale.Y / lastScale.Y, newScale.Z / lastScale.Z);

                transform.Scale = new Vector3(transform.Scale.X * delta.X, transform.Scale.Y * delta.Y, transform.Scale.Z * delta.Z);
                ignoreChangeTransformEvent = false;
                foreach (Entity ent in entities)
                {
                    ent.isGlobalTransformDirty = true;
                }
            };

            updateableComponents.Add(transform);
        }




        public void Initialize()
        {
            //throw new NotImplementedException();
        }

        public List<EntityDrawableComponent> DrawableComponents
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

        public List<EntityUpdateableComponent> UpdateableComponents
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

        public IGameComponent GetComponent(string name)
        {
            if (components.ContainsKey(name))
            {
                return components[name];
            }
            return null;
        }


        public void AddDrawableComponent(string name, EntityDrawableComponent component)
        {
            if (components.ContainsKey(name))
            {
                //TODO
                throw new Exception();
            }

            component.ParentComponent = this;
            component.Initialize();
            component.DrawOrderChanged += (s, e) => isDrawDirty = true;
            components.Add(name, component);
            drawableComponents.Add(component);
            isDrawDirty = true;
        }

        public void AddUpdateableComponent(string name, EntityUpdateableComponent component)
        {
            if (components.ContainsKey(name))
            {
                //TODO
                throw new Exception();
            }

            component.ParentComponent = this;
            component.Initialize();
            component.UpdateOrderChanged += (s, e) => isUpdateDirty = true;
            components.Add(name, component);
            updateableComponents.Add(component);
            isUpdateDirty = true;
        }

        public void AddEntity(Entity entity)
        {
            if (entity.ID != -1)
            {
                entity.scene.RemoveEntity(entity);
            }
            entity.ID = scene.GetNewEntityId();

            entity.parentTransform = transform;
            entity.scene = scene;
            entity.parent = this;

            entities.Add(entity);
        }


        public void RemoveEntity(Entity entity, bool addToScene = false)
        {


            if (!entities.Contains(entity)) {
                return;
            }
            Transform transform = GlobalTransform;

                
            entity.transform.Position = GlobalTransform.Position;
            entity.transform.Rotation = GlobalTransform.Rotation;
            entity.transform.Scale = GlobalTransform.Scale;

            entity.parentTransform = null;
            entity.parent = null;
            entities.Remove(entity);
            if (addToScene)
            {
              
                scene.AddEntity(entity);
            }

        }
    }
}
