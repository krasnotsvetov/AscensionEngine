using System.Collections.Generic;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using System.Collections.ObjectModel;
using System.Xml;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Graphics.SceneSystem
{

    public class Scene2D
    {
        public UpdateableComponent scene2DUpdater;
        public DrawableComponent scene2DDrawer;
        //public List<Scene2D> Scenes { get { return scenes; } }
        public ReadOnlyCollection<Entity> Entities { get { return entities.AsReadOnly(); } }
        public ReadOnlyCollection<Light> Lights { get { return lights.AsReadOnly(); } }


       
        private Background background;

        // private List<Scene2D> scenes = new List<Scene2D>();
        private List<Entity> entities = new List<Entity>();
        private List<Light> lights = new List<Light>();

        public Scene2D()
        {
            scene2DDrawer = new Scene2DDrawer(this);
            scene2DUpdater = new Scene2DUpdater(this);
        }


        public static Scene2D Load()
        {
            Scene2D scene = new Scene2D();
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
                                    }

                                    if (t is UpdateableComponent)
                                    {
                                        entity.UpdateableComponents.Add((EntityUpdateableComponent)t);
                                    }

                                    if (t is DrawableComponent)
                                    {
                                        entity.DrawableComponents.Add((EntityDrawableComponent)t);
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
            foreach (int i in loadOrder)
            {
                var list = entityGraph[entities[i].ID];
                scene.AddEntity(entities[i]);
                createEntityGraph(entities[i], list, entityGraph, entities);
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
                        xmlWritter.WriteStartElement(String.Format("Updateable-{0}", uc.ToString()));
                        xmlWritter.WriteAttributeString((String.Format("Updateable-{0}", uc.ToString())), uc.GetType().FullName);
                        var serializer = new DataContractSerializer(uc.GetType());
                        serializer.WriteObject(xmlWritter, uc);
                        xmlWritter.WriteEndElement();
                    }

                    foreach (var dc in ent.DrawableComponents)
                    {
                        xmlWritter.WriteStartElement(String.Format("Drawable-{0}", dc.ToString()));
                        xmlWritter.WriteAttributeString((String.Format("Drawable-{0}", dc.ToString())), dc.GetType().FullName);
                        var serializer = new DataContractSerializer(dc.GetType());
                        serializer.WriteObject(xmlWritter, dc);
                        xmlWritter.WriteEndElement();
                    }
                    xmlWritter.WriteEndElement();
                }

                xmlWritter.WriteStartElement("scene-data");
                //TODO
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

        public int GetNewEntityId()
        {
            if (freeId.Count == 0)
            {
                return nextId++;
            }
            int temp = freeId.Min;
            freeId.Remove(temp);
            return temp;
        }

        public void AddEntity(Entity entity)
        {
            if (entity.Scene != null)
            {
                entity.Scene.RemoveEntity(entity);
            }
            entity.Scene = this;
            entity.ID = GetNewEntityId();
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            
            entity.Scene = null;
            if (entity.ID > -1)
            {
                freeId.Add(entity.ID);
            }
            entity.ID = -1;
            entities.Remove(entity);
        }

        public void AddLight(Light light)
        {
            lights.Add(light);
        }

        public void RemoveLight(Light light)
        {
            lights.Remove(light);
        }
    }

    
}
