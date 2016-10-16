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
        public Scene2D Scene
        {
            get { return scene; }
            set
            {
                if (value == scene)
                {
                    return;
                }
                if (value == null)
                {
                    ID = -1;
                } else
                {
                    ID = value.GetNewEntityId();
                }
                Scene2D prev = scene;
                scene = value;

                //ignoreChangeScene == true, когда сцена временно ставится в null и затем в предыдущую сцену, 
                //поэтому ничего не меняем
                if (!ignoreChangeScene)
                {
                    foreach (Entity entity in entities)
                    {
                        entity.Scene = value;
                    }

                    if (prev != null)
                    {
                        foreach (DrawableComponent dc in drawableComponents)
                        {
                            dc.SceneChanged(prev);
                        }

                        foreach (UpdateableComponent uc in updateableComponents)
                        {
                            uc.SceneChanged(prev);
                        }
                    }
                }
            }
        }

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

        /// <summary>
        /// WARNINIG, set only for load scene.
        /// </summary>
        internal int ID = -1;

        private Entity parent;
        private bool isDrawDirty = false;
        private bool isUpdateDirty = false;

        private Scene2D scene;
        private Transform transform;
        private Transform parentTransform;
        private Transform globalTransform;
        private bool ignoreChangeTransformEvent = false;
        private bool ignoreChangeScene = false;


        public Entity(Scene2D scene) : this()
        {
            scene.AddEntity(this);
        }

        public Entity()
        { 
            Guid = Guid.NewGuid();
            parent = null;
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
         
        /// <summary>
        /// Initialize calls if scene changed.
        /// </summary>
        public void Initialize()
        {
            if (scene != null)
            {
                foreach (DrawableComponent dc in drawableComponents)
                {
                    dc.Initialize();
                }

                foreach (UpdateableComponent uc in updateableComponents)
                {
                    uc.Initialize();
                }

                foreach (Entity e in entities)
                {
                    e.Initialize();
                }
            }
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
            if (scene != null)
            {
                component.Initialize();
            }

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

            if (scene != null)
            {
                component.Initialize();
            }
            component.UpdateOrderChanged += (s, e) => isUpdateDirty = true;
            components.Add(name, component);
            updateableComponents.Add(component);
            isUpdateDirty = true;
        }

        public void AddEntity(Entity entity)
        {
            Scene2D lastEntityScene = entity.Scene;

            //Don't call sceneChange if scenes is same
            if (lastEntityScene == scene)
            {
                entity.ignoreChangeScene = true;
            }

            if (entity.scene != null)
            {
                entity.scene.RemoveEntity(entity);
            }

            entity.parentTransform = transform;
            entity.Scene = scene;
            entity.parent = this;

            //Call initialize only if scene changed.
            if (scene != null && lastEntityScene != scene)
            {
                entity.Initialize();
            }

            if (scene != null)
            {
                entity.ID = scene.GetNewEntityId();
            }
            entities.Add(entity);
            entity.ignoreChangeScene = false;
        }


        public void RemoveEntity(Entity entity, bool addToScene = false)
        {


            if (!entities.Contains(entity)) {
                return;
            }
            entity.parentTransform = null;
            entity.parent = null;
            entities.Remove(entity);

            entity.Transform.SetTransform(entity.globalTransform.Position, entity.globalTransform.Rotation, entity.globalTransform.Scale);
            
            if (addToScene)
            {
              
                scene.AddEntity(entity);
            }

        }
    }
}
