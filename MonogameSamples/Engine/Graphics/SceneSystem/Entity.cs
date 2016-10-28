using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using System.Collections.ObjectModel;

namespace MonogameSamples.Engine.Graphics.SceneSystem
{


    public class Entity
    {
         

       
        public ReadOnlyCollection<Entity> Entities { get { return entities.AsReadOnly(); } }
        public Scene Scene
        {
            get { return scene; }
            internal set
            {
                if (value == scene)
                {
                    return;
                }
                Scene lastScene = scene;
                scene = value;
                foreach (Entity entity in entities)
                {
                    entity.Scene = value;
                }

                SceneChanged(lastScene);
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

        private Scene scene;
        private Transform transform;
        private Transform parentTransform;
        private Transform globalTransform;
        private bool ignoreChangeTransformEvent = false;


        public Entity(Scene scene) : this()
        {
            scene.AddEntity(this);
        }

        public Entity()
        { 
            Guid = Guid.NewGuid();
            parent = null;
            transform = new Transform("LocalTransform");
            globalTransform = new Transform("GlobalTrasform");

            transform.ParentComponent = globalTransform.ParentComponent = this;

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

        } 
         
        /// <summary>
        /// Initialize calls if scene changed.
        /// </summary>
        public void Initialize()
        {

            //TODO
            if (scene == null)
            {
                throw new Exception("Entity should be attached to scene");
            }

            foreach (UpdateableComponent uc in updateableComponents)
            {
                uc.Initialize();
            }

            foreach (DrawableComponent dc in drawableComponents)
            {
                dc.Initialize();
            }

            foreach (Entity e in entities)
            {
                e.Initialize();
            }
        }

        public ReadOnlyCollection<EntityDrawableComponent> DrawableComponents
        {
            get
            {
                if (isDrawDirty)
                {
                    isDrawDirty = false;
                    drawableComponents.Sort();
                }
                return drawableComponents.AsReadOnly();
            }
        }

        public ReadOnlyCollection<EntityUpdateableComponent> UpdateableComponents
        {
            get
            {
                if (isUpdateDirty)
                {
                    isUpdateDirty = false;
                    updateableComponents.Sort();
                }
                return updateableComponents.AsReadOnly();
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


        public virtual void AddDrawableComponent(EntityDrawableComponent component)
        {
            if (component.ParentEntity != null)
            {
                throw new Exception("ParentComponent must be null");
            }

            if (components.ContainsKey(component.Name))
            {
                //TODO
                throw new Exception("Enity has component with same name");
            }

            component.ParentEntity = this;
            if (scene != null)
            {
                component.Initialize();
            }

            component.DrawOrderChanged += (s, e) => isDrawDirty = true;
            components.Add(component.Name, component);
            drawableComponents.Add(component);
            isDrawDirty = true;
        }

        public virtual void AddUpdateableComponent(EntityUpdateableComponent component)
        {
            if (component.ParentComponent != null)
            {
                throw new Exception("ParentComponent must be null");
            }

            if (components.ContainsKey(component.Name))
            {
                //TODO
                throw new Exception("Enity has component with same name");
            }

            component.ParentComponent = this;

            if (scene != null)
            {
                component.Initialize();
            }
            component.UpdateOrderChanged += (s, e) => isUpdateDirty = true;
            components.Add(component.Name, component);
            updateableComponents.Add(component);
            isUpdateDirty = true;
        }

        public virtual void AddEntity(Entity entity)
        {
            if (entity.parent != null)
            {
                entity.parent.RemoveEntity(entity, true);
            }

            //TODO, setting parameters
            entity.parent = this;
            entity.parentTransform = transform;
            entities.Add(entity);
            //

            Scene lastScene = entity.scene;

            if (lastScene != null)
            { 
                if (lastScene == scene)
                {
                    lastScene.RemoveEntity(entity, true);
                } else
                {
                    lastScene.RemoveEntity(entity);
                    
                }
            }

            if (lastScene != scene)
            {
                //set recursive scene to all child
                entity.Scene = scene;
                if (scene != null)
                {
                    scene.entityDFSGetID(entity);
                    entity.Initialize();
                }
            }
        }

        /// <summary>
        /// Remove entity from child entity and from scene if addToScene equals False
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="addToScene"></param>
        public virtual void RemoveEntity(Entity entity, bool addToScene = false)
        {

            //TODO make assert here.
            if (entity.scene != scene)
            {
                throw new Exception("");
            }

            if (!entities.Contains(entity)) {
                throw new Exception("entity is not attached to this entity");
            }

            entity.parentTransform = null;
            entity.parent = null;
            entities.Remove(entity);

            entity.Transform.SetTransform(entity.globalTransform.Position, entity.globalTransform.Rotation, entity.globalTransform.Scale);



            if (scene != null)
            {
                if (addToScene)
                {
                    scene.AddEntity(entity);
                }
                else
                {
                    scene.RemoveEntity(entity);
                }
            }
        }


        /// <summary>
        /// This method will be called if scene for Entity will change
        /// </summary>
        /// <param name="lastScene">lastScene is reference to previous Scene</param>
        protected virtual void SceneChanged(Scene lastScene)
        {
            foreach (var uc in UpdateableComponents)
            {
                uc.SceneChanged(lastScene);
            }

            foreach (var dc in DrawableComponents)
            {
                dc.SceneChanged(lastScene);
            }
        }


        /// <summary>
        /// This method will be called if RenderSystem for scene will change
        /// </summary>
        internal void RenderSystemChange()
        {
            foreach (var e in entities)
            {
                e.RenderSystemChange();
            }

            foreach (var dc in drawableComponents)
            {
                dc.RenderSystemChange();
            }
        }


        public override string ToString()
        {
            return "TODO";
        }
    }
}
