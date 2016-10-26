using System.Collections.Generic;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using System.Collections.ObjectModel;
using System.Xml;
using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Graphics.Shaders;
using MonogameSamples.Engine.Core.Common.Collections;
using MonogameSamples.Engine.Graphics.MaterialSystem;
using MonogameSamples.Engine.Content;

namespace MonogameSamples.Engine.Graphics.SceneSystem
{

    public class Scene
    {
        public SceneUpdater sceneUpdater;
        public SceneRenderer sceneRenderer;



        //public List<Scene2D> Scenes { get { return scenes; } }
        public ReadOnlyCollection<Entity> Entities { get { return entities.AsReadOnly(); } }
        public ReadOnlyCollection<Light> Lights { get { return lights.AsReadOnly(); } }

        public ShaderCollection Shaders = new ShaderCollection();
        public MaterialCollection Materials = new MaterialCollection();

        // internal Texture2DCollection Textures = new Texture2DCollection();


        public ContentSystem Content {get; set;}
        public RenderSystem RenderSystem { get { return renderSystem; } }


        private RenderSystem renderSystem;       
        private Background background;


        public string Name;


        // private List<Scene2D> scenes = new List<Scene2D>();
        private List<Entity> entities = new List<Entity>();
        private List<Light> lights = new List<Light>();

        public Scene(string name, RenderSystem renderSystem)
        {
            this.Name = name;
            this.renderSystem = renderSystem;
            sceneRenderer = new SceneRenderer(this, renderSystem);
            sceneUpdater = new SceneUpdater(this);

            Content = ContentSystem.GetInstance();
            renderSystem.AddComponent(new KeyValuePair<string, DrawableComponent>("SceneDrawer" + Name, sceneRenderer));
        }



        public void ChangeRenderSystem(RenderSystem renderSystem)
        {
            this.renderSystem = renderSystem;
            renderSystem.AddComponent(new KeyValuePair<string, DrawableComponent>("SceneDrawer" + Name, sceneRenderer));
            foreach (var e in entities)
            {
                e.RenderSystemChange();
            }
        }



        public static Scene Load(RenderSystem renderSystem)
        {
            Scene scene = new Scene("temp", renderSystem);
            ContentSystem cs = ContentSystem.GetInstance();
            Dictionary<int, List<int>> entityGraph = new Dictionary<int, List<int>>();
            Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
            List<int> loadOrder = new List<int>();
            bool isGraphRead = false;
            using (XmlReader reader = XmlReader.Create("test.xml"))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            var matSerializer = new DataContractSerializer(typeof(Material));
                            if (reader.Name.Equals("materials"))
                            {
                                if (!NextElement(reader, 3))
                                {
                                    break;
                                }
                                while (reader.Depth == 3)
                                {
                                    var t = (Material)matSerializer.ReadObject(reader);
                                    t.textures = new List<Texture2D>();
                                    foreach (var r in t.References)
                                    {
                                        if (r == null)
                                        {
                                            t.textures.Add(null);
                                        }
                                        else
                                        {
                                            t.textures.Add(cs.Textures[r.Name]);
                                        }
                                    }
                                    scene.Materials.Add(MaterialReference.FromIdentifier(t.MaterialName), t);
                                    NextElement(reader, 3);
                                }
                            }

                            if (reader.Name.Equals("shaders"))
                            {
                                if (!NextElement(reader, 3))
                                {
                                    break;
                                }
                                while (reader.Depth >= 3)
                                { 
                                    reader.MoveToNextAttribute();
                                    string type = reader.Value;
                                    NextElement(reader, 3);
                                    var shaderSerializer = new DataContractSerializer(Type.GetType(type));
                                    var t = (IPipelineStateSetter)shaderSerializer.ReadObject(reader);
                                    t.Effect = cs.Effect[t.ShaderName];
                                    scene.Shaders.Add(ShaderReference.FromIdentifier(t.ShaderName), t);
                                    NextElement(reader, 3);
                                }
                                 
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
                            while (isGraphRead && reader.Name.StartsWith("entity") && reader.Depth == 1)
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    entity = new Entity();
                                    
                                    reader.MoveToNextAttribute();
                                    int id = int.Parse(reader.Value);
                                    entity.ID = id;
                                }
                                if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    entities.Add(entity.ID, entity);
                                }
                                NextElement(reader, 2);
                                while (reader.Depth == 2)
                                {
                                    reader.MoveToNextAttribute();
                                    string typeName = reader.Value;
                                    var serializer = new DataContractSerializer(Type.GetType(typeName));

                                    NextElement(reader, 2);
                                    var t = serializer.ReadObject(reader);
                                    if (t is Transform)
                                    {
                                        var temp = t as Transform;
                                        entity.Transform.SetTransform(temp.Position, temp.Rotation, temp.Scale);
                                    } else
                                    if (t is UpdateableComponent)
                                    {
                                        entity.AddUpdateableComponent((EntityUpdateableComponent)t);
                                    }

                                    if (t is DrawableComponent)
                                    {
                                        entity.AddDrawableComponent((EntityDrawableComponent)t);
                                    }
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

                            if (reader.Name.Equals("scene-data"))
                            {
                                //TODO
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

        public void Save()
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
            using (XmlWriter xmlWritter = XmlWriter.Create("test.xml", settings))
            {
                xmlWritter.WriteStartDocument();
                xmlWritter.WriteStartElement("SceneData");
                xmlWritter.WriteStartElement("scene-data");
                xmlWritter.WriteStartElement("materials");
                var serializer = new DataContractSerializer(typeof(Material));
                foreach (var m in Materials)
                {
                    serializer.WriteObject(xmlWritter, m);
                }
                xmlWritter.WriteEndElement();

                xmlWritter.WriteStartElement("shaders");
                int num = 0;
                foreach (var ps in Shaders)
                { 
                    serializer = new DataContractSerializer(ps.GetType());
                    xmlWritter.WriteStartElement("PipelineState" + num);
                    xmlWritter.WriteAttributeString("PipelineState" + num, ps.GetType().FullName);
                    serializer.WriteObject(xmlWritter, ps);
                    xmlWritter.WriteEndElement();
                    num++;
                }
                xmlWritter.WriteEndElement();

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
                foreach (var ent in allEntities)
                {
                    xmlWritter.WriteStartElement(String.Format("entity-{0}", ent.ID));
                    xmlWritter.WriteAttributeString(String.Format("entity-{0}", ent.ID), ent.ID.ToString());

                    foreach (var uc in ent.UpdateableComponents)
                    {
                        serializer = new DataContractSerializer(uc.GetType());
                        xmlWritter.WriteStartElement(String.Format("Updateable-{0}", uc.ToString()));
                        xmlWritter.WriteAttributeString((String.Format("Updateable-{0}", uc.ToString())), uc.GetType().FullName);
                        serializer.WriteObject(xmlWritter, uc);
                        xmlWritter.WriteEndElement();
                        
                    }
                    foreach (var dc in ent.DrawableComponents)
                    {
                        serializer = new DataContractSerializer(dc.GetType());
                        xmlWritter.WriteStartElement(String.Format("Drawable-{0}", dc.ToString()));
                        xmlWritter.WriteAttributeString((String.Format("Drawable-{0}", dc.ToString())), dc.GetType().FullName);
                        serializer.WriteObject(xmlWritter, dc);
                        xmlWritter.WriteEndElement();
                    }
                    xmlWritter.WriteEndElement();
                }

               
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
                entity.Initialize();
            }

            entities.Add(entity);
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

        /// <summary>
        /// Add light to scene
        /// </summary>
        /// <param name="light"></param>
        public void AddLight(Light light)
        {
            lights.Add(light);
        }


        /// <summary>
        /// Remove light from scene
        /// </summary>
        /// <param name="light"></param>
        public void RemoveLight(Light light)
        {
            lights.Remove(light);
        }



        /// <summary>
        /// effect.Name value will be used for ShaderReference
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="setter"></param>
        public void AddShader(Effect effect, IPipelineStateSetter setter, string referenceName = "")
        {
           
            setter.Initialize(effect);
            if (referenceName != "")
            {
                setter.ShaderName = referenceName;
            }
            Shaders.Add(referenceName.Equals("") ? effect.Name : referenceName, setter);
        }

        /// <summary>
        /// material.Name value will be used for MaterialReference
        /// </summary>
        /// <param name="material"></param>
        public void AddMaterial(Material material, string referenceName = "")
        {
            Materials.Add(referenceName.Equals("") ? material.MaterialName : referenceName, material);
        }

    }

    
}
