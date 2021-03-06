﻿using System.Collections.Generic;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Components;
using System.Collections.ObjectModel;
using System.Xml;
using System;
using System.Runtime.Serialization;
using Ascension.Engine.Graphics.CameraSystem;
using Microsoft.Xna.Framework;
using Ascension.Engine.Core.Systems.Content;
using System.Linq;
using System.Reflection;

namespace Ascension.Engine.Graphics.SceneSystem
{

    public partial class Scene : IContentObject
    {


        public bool IsInitialized { get; protected set; }
        public Vector3 Ambient = Color.White.ToVector3();

        public SceneUpdater sceneUpdater;
        public SceneRenderer sceneRenderer;

        public EventHandler<EventArgs> EnititiesChanged;
        public EventHandler<EventArgs> LightsChanged;

        public ReadOnlyCollection<Entity> Entities { get { return entities.AsReadOnly(); } }
        public ReadOnlyCollection<Light> Lights { get { return lights.AsReadOnly(); } }
        public ReadOnlyCollection<Camera> Cameras { get { return cameras.AsReadOnly(); } }

        public RenderSystem RenderSystem { get { return renderSystem; } }


        private RenderSystem renderSystem;


        public string Name { get; set; }


        // private List<Scene2D> scenes = new List<Scene2D>();
        private List<Entity> entities = new List<Entity>();
        internal List<Light> lights = new List<Light>();
        internal List<Camera> cameras = new List<Camera>();

        protected ContentContainer contentContainer;

        public Scene(string name, RenderSystem renderSystem)
        {
            this.Name = name;
            this.renderSystem = renderSystem;
            sceneRenderer = new SceneRenderer(this, renderSystem);
            sceneRenderer.Ambient = Ambient;
            sceneUpdater = new SceneUpdater(this);

            contentContainer = ContentContainer.Instance();
        }


        public void Initialize()
        {
            foreach (var e in entities)
            {
                e.Initialize();
            }
            IsInitialized = true;
        }

        public void ChangeRenderSystem(RenderSystem renderSystem)
        {
            this.renderSystem = renderSystem;
            sceneRenderer = new SceneRenderer(this, renderSystem);
            foreach (var e in entities)
            {
                e.RenderSystemChange();
            }
        }

        public void LoadContent()
        {
            sceneRenderer.LoadContent();
        }

        public static Scene Load(string name, RenderSystem renderSystem)
        {
            Scene scene = new Scene(name, renderSystem);
            Dictionary<int, List<int>> entityGraph = new Dictionary<int, List<int>>();
            Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
            List<int> loadOrder = new List<int>();
            bool isGraphRead = false;
            using (XmlReader reader = XmlReader.Create(name))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            var matSerializer = new DataContractSerializer(typeof(Material));
                            if (reader.Name.Equals("requiredContent"))
                            {

                            }
                            if (reader.Name.Equals("rootEntities"))
                            {
                                if (reader.HasAttributes)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        string[] loadOrderValue = reader.Value.Split(' ');
                                        foreach (var s in loadOrderValue)
                                        {
                                            loadOrder.Add(int.Parse(s));
                                        }
                                    }
                                }
                            }

                            Entity entity = null; ;
                            if (reader.Name.StartsWith("def-entities"))
                            {
                                NextElement(reader, 2);
                                while (isGraphRead && reader.Name.StartsWith("entity") && reader.Depth == 2)
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        entity = new Entity("");

                                        reader.MoveToNextAttribute();
                                        entity.ID = int.Parse(reader.Value);
                                        reader.MoveToNextAttribute();
                                        entity.Name = reader.Value;
                                    }


                                    NextElement(reader, 3);

                                    while (reader.Depth == 3)
                                    {
                                        reader.MoveToNextAttribute();
                                        Console.WriteLine(reader.AttributeCount);
                                        string typeName = reader.Value;

                                        DataContractSerializer serializer = null;
                                        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                                        {
                                            if (a.GetType(typeName) != null)
                                            {
                                                serializer = new DataContractSerializer(a.GetType(typeName));
                                            }
                                        }

                                        NextElement(reader, 3);
                                        var t = serializer.ReadObject(reader);

                                        if (t is Transform)
                                        {
                                            var temp = t as Transform;
                                            entity.Transform.SetTransform(temp.Position, temp.Rotation, temp.Scale);
                                        }
                                        else
                                        if (t is EntityUpdateableComponent)
                                        {
                                            var component = Activator.CreateInstance(t.GetType(), new[] { (t as EntityUpdateableComponent).Name });
                                            //TODO, fix COPY-PASTE.
                                            foreach (var field in t.GetType().GetRuntimeFields().Where(p => p.IsDefined(typeof(DataMemberAttribute), false)))
                                            {
                                                field.SetValue(component, field.GetValue(t));
                                            }
                                            foreach (var field in t.GetType().GetRuntimeProperties().Where(p => p.IsDefined(typeof(DataMemberAttribute), false)))
                                            {
                                                field.SetValue(component, field.GetValue(t));
                                            }
                                            entity.AddUpdateableComponent((EntityUpdateableComponent)component);
                                        }

                                        if (t is EntityDrawableComponent)
                                        {
                                            var component = Activator.CreateInstance(t.GetType(), new[] { (t as EntityDrawableComponent).Name, (t as EntityDrawableComponent).MaterialName });
                                            foreach (var field in t.GetType().GetRuntimeFields().Where(p => p.IsDefined(typeof(DataMemberAttribute), false)))
                                            {
                                                field.SetValue(component, field.GetValue(t));
                                            }
                                            foreach (var field in t.GetType().GetRuntimeProperties().Where(p => p.IsDefined(typeof(DataMemberAttribute), false)))
                                            {
                                                field.SetValue(component, field.GetValue(t));
                                            }
                                            entity.AddDrawableComponent((EntityDrawableComponent)component);
                                        }
                                        NextElement(reader, 3);
                                    }
                                    entities.Add(entity.ID, entity);
                                    NextElement(reader, 2);
                                }
                            }


                            if (reader.Name.Equals("entity-graph"))
                            {
                                if (reader.HasAttributes)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        string[] splitString = reader.Value.Split(' ');
                                        List<int> temp = new List<int>();
                                        for (int i = 1; i < splitString.Length; i++)
                                        {
                                            temp.Add(int.Parse(splitString[i]));
                                        }
                                        entityGraph.Add(int.Parse(splitString[0]), temp);
                                    }
                                }
                                isGraphRead = true;
                            }

                            if (reader.Name.Equals("SceneData"))
                            {
                                reader.MoveToNextAttribute();
                                scene.Name = reader.Value;
                            }
                            break;

                    }
                }

            }

            ///Create entity graph
            foreach (int i in loadOrder)
            {
                var list = entityGraph[entities[i].ID];
                createEntityGraph(entities[i], list, entityGraph, entities);
            }

            ///Add them all to scene
            ///
            foreach (int i in loadOrder)
            {
                scene.AddEntity(entities[i]);
            }
            return scene;

        }



        private static void createEntityGraph(Entity entity, List<int> list, Dictionary<int, List<int>> graph, Dictionary<int, Entity> entities)
        {
            foreach (int t in list)
            {
                var tempList = graph[entities[t].ID];
                entity.AddEntity(entities[t]);
                createEntityGraph(entities[t], tempList, graph, entities);
            }
        }


        /// <summary>
        /// will find the next Element with depth greater or equal than depthLevelAbove
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="depthLevel"></param>
        /// <returns></returns>
        public static bool NextElement(XmlReader reader, int depthLevelAbove)
        {
            do
            {
                if (!reader.Read())
                {
                    return false;
                }
                if (reader.Depth < depthLevelAbove)
                {
                    return false;
                }
            } while (reader.NodeType != XmlNodeType.Element);
            return true;
        }

        public void Save(string path)
        {
            List<List<int>> graph = new List<List<int>>();
            List<int> invTopSort = new List<int>();
            List<Entity> allEntities = new List<Entity>();
            foreach (Entity ent in entities)
            {
                dfsEntityGraph(ent, graph, allEntities);
            }
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            using (XmlWriter xmlWritter = XmlWriter.Create(path, settings))
            {
                xmlWritter.WriteStartDocument();
                xmlWritter.WriteStartElement("SceneData");
                xmlWritter.WriteAttributeString("SceneData", Name);
                xmlWritter.WriteStartElement("requiredContent");


                xmlWritter.WriteEndElement();
                xmlWritter.WriteStartElement("rootEntities");
                string entitiesID = "";
                entities.ForEach(t => entitiesID += t.ID + " ");
                entitiesID = entitiesID.Remove(entitiesID.Length - 1);
                xmlWritter.WriteAttributeString("entitiesID", entitiesID);
                xmlWritter.WriteEndElement();
                xmlWritter.WriteStartElement("entity-graph");
                for (int i = 0; i < graph.Count; i++)
                {
                    string temp = "";
                    graph[i].ForEach(t => temp += t.ToString() + " ");
                    temp = temp.Remove(temp.Length - 1);
                    xmlWritter.WriteAttributeString(String.Format("entity-{0}", graph[i][0]), temp);
                }
                xmlWritter.WriteEndElement();
                xmlWritter.WriteStartElement("def-entities");
                foreach (var ent in allEntities)
                {
                    xmlWritter.WriteStartElement(String.Format("entity-{0}", ent.ID));
                    xmlWritter.WriteAttributeString(String.Format("entity-{0}", ent.ID), ent.ID.ToString());
                    xmlWritter.WriteAttributeString("entity-name", ent.Name);

                    foreach (var uc in ent.UpdateableComponents)
                    {
                        var serializer = new DataContractSerializer(uc.GetType());
                        xmlWritter.WriteStartElement(String.Format("Updateable-{0}", uc.ToString()));
                        xmlWritter.WriteAttributeString((String.Format("Updateable-{0}", uc.ToString())), uc.GetType().FullName);
                        serializer.WriteObject(xmlWritter, uc);
                        xmlWritter.WriteEndElement();

                    }
                    foreach (var dc in ent.DrawableComponents)
                    {
                        var serializer = new DataContractSerializer(dc.GetType());
                        xmlWritter.WriteStartElement(String.Format("Drawable-{0}", dc.ToString()));
                        xmlWritter.WriteAttributeString((String.Format("Drawable-{0}", dc.ToString())), dc.GetType().FullName);
                        serializer.WriteObject(xmlWritter, dc);
                        xmlWritter.WriteEndElement();
                    }
                    xmlWritter.WriteEndElement();
                }
                xmlWritter.WriteEndElement();

                xmlWritter.WriteEndElement();
                xmlWritter.WriteEndDocument();
            }
        }

        private void dfsEntityGraph(Entity ent, List<List<int>> graph, List<Entity> allEntities)
        {

            List<int> temp = new List<int>();
            temp.Add(ent.ID);
            allEntities.Add(ent);
            foreach (Entity e in ent.Entities)
            {
                temp.Add(e.ID);
            }
            graph.Add(temp);

            foreach (Entity e in ent.Entities)
            {
                dfsEntityGraph(e, graph, allEntities);
            }
        }


        private SortedSet<int> freeId = new SortedSet<int>();
        private int nextId = 0;


        /// <summary>
        /// Return new unique ID for entity
        /// </summary>
        /// <returns></returns>
        internal int GetNewEntityId()
        {
            if (freeId.Count == 0)
            {
                return nextId++;
            }
            int temp = freeId.Min;
            freeId.Remove(temp);
            return temp;
        }


        /// <summary>
        /// Add entity to scene
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(Entity entity)
        {
            if (entity.Parent != null)
            {
                throw new Exception("Entity has parent");
            }

            if (entities.Contains(entity))
            {
                throw new Exception("Entity have been already attached to this scene");
            }




            if (entity.Scene != this)
            {
                if (entity.Scene != null)
                {
                    entity.Scene.entityDFSFreeID(entity);
                }
                entity.Scene = this;
                entityDFSGetID(entity);
                if (IsInitialized)
                {
                    entity.Initialize();
                }
            }

            entities.Add(entity);

            EnititiesChanged?.Invoke(this, EventArgs.Empty);
        }



        /// <summary>
        /// Remove entity and all children from scene and free IDs. If isMovedToAnotherEntity equals true,
        /// entity.Scene, entity's children Scene, entity.ID, entity children ID won't modified
        /// </summary>
        /// <param name="entity"></param>
        internal void RemoveEntity(Entity entity, bool isMovedToAnotherEntity)
        {

            if (entity.Scene != this)
            {
                throw new Exception("Entity is attached to another scene");
            }

            if (!entities.Contains(entity))
            {
                throw new Exception("entity is not attached to this scene");
            }


            //TODO, make it assert
            if (entity.ID == -1)
            {
                //TODO 
                throw new Exception("entity ID must be greater or equal to 0");
            }

            if (!isMovedToAnotherEntity)
            {
                entityDFSFreeID(entity);
                entity.Scene = null;
            }
            entities.Remove(entity);

            EnititiesChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Remove entity and all child from scene.
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(Entity entity)
        {

            if (entity.Scene != this)
            {
                throw new Exception("Entity is attached to another scene");
            }
            if (entity.Scene == null)
            {
                throw new Exception("entity Scene can't be null");
            }
            if (!entities.Contains(entity))
            {
                throw new Exception("entity is not attached to this scene");
            }
            if (entity.ID == -1)
            {
                //TODO 
                throw new Exception("entity ID must be greater or equal to 0");
            }

            RemoveEntity(entity, true);
        }



        /// <summary>
        /// use dfs to give ID 
        /// </summary>
        /// <param name="entity"></param>
        internal void entityDFSGetID(Entity entity)
        {
            entity.ID = GetNewEntityId();
            foreach (var e in entity.Entities)
            {
                entityDFSGetID(e);
            }
        }

        /// <summary>
        /// use dfs to free ID 
        /// </summary>
        /// <param name="entity"></param>
        internal void entityDFSFreeID(Entity entity)
        {
            entity.ID = -1;
            freeId.Add(entity.ID);
            foreach (var e in entity.Entities)
            {
                entityDFSFreeID(e);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }


}
