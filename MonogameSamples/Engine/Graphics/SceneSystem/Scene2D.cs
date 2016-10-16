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

        public void Save()
        {
            entityGraph();
        }

        private List<List<int>> entityGraph()
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
            XmlWriter xmlWritter = XmlWriter.Create("test.xml", settings);
            xmlWritter.WriteStartDocument();
            xmlWritter.WriteStartElement("SceneData");
            xmlWritter.WriteStartElement("invTopSort");
            string entitiesID = "";
            allEntities.ForEach(t => entitiesID += t.ID + " ");
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
                foreach (var uc in ent.UpdateableComponents)
                {
                    xmlWritter.WriteStartElement(String.Format("Updateable-{0}", uc.ToString()));
                    var serializer = new DataContractSerializer(uc.GetType());
                    serializer.WriteObject(xmlWritter, uc);
                    xmlWritter.WriteEndElement();
                }

                foreach (var dc in ent.DrawableComponents)
                {
                    xmlWritter.WriteStartElement(String.Format("Drawable-{0}", dc.ToString()));
                    var serializer = new DataContractSerializer(dc.GetType());
                    serializer.WriteObject(xmlWritter, dc);
                    xmlWritter.WriteEndElement();
                }
                xmlWritter.WriteEndElement();
            }
            xmlWritter.WriteEndElement();
            xmlWritter.WriteEndDocument();
            xmlWritter.Close();

            return graph;
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
            entity.ID = GetNewEntityId();
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            freeId.Add(entity.ID);
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
