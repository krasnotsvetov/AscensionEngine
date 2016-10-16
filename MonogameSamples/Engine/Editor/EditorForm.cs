using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Common.Attributes;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace MonogameSamples.Engine.Editor
{
    public partial class EditorForm : Form
    {

        public GameEditor GameEditor;
        public EditorForm()
        {
            InitializeComponent();
        }

        public void InitializateGUI(GameEditor gameEditor)
        {
            this.GameEditor = gameEditor;
            RenderSystem rs = (RenderSystem)GameEditor.drawableComponents.Values.FirstOrDefault(t => t is RenderSystem);
            rs.GameComponents.Where(t => (t is Scene2DDrawer)).ToList().ForEach(w => SceneComboBox.Items.Add((w as Scene2DDrawer).Scene));
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.GetCustomAttribute(typeof(ComponentAttribute), true) != null)
                {
                    AvailableComponents.Items.Add(t);
                }
            }
        }
    
        private void SceneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntityView.Nodes.Clear();
            var scene = (SceneComboBox.SelectedItem) as Scene2D;
            EntityView.BeginUpdate();
            foreach (var ent in scene.Entities)
            {
                AddEntitiesToTreeViewRec(ent, EntityView.Nodes);
                //int x = 5;
            }
            EntityView.EndUpdate();
            
        }

        private void AddEntitiesToTreeViewRec(Entity ent, TreeNodeCollection collection)
        {
            int t = collection.Add(new EntityTreeNode(ent));
            foreach (var e in ent.Entities)
            {
                AddEntitiesToTreeViewRec(e, collection[t].Nodes);
            }
        }

        private void EntityView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            EntityTreeNode node = (EntityTreeNode)EntityView.SelectedNode;

            if (node != null)
            {
                ComponentBox.Items.Clear();
                var ent = node.Entity;
                GameEditor.SelectedEntity = ent;
                foreach (var dc in ent.DrawableComponents)
                {
                    ComponentBox.Items.Add(new ComponentCell(dc));
                }

                foreach (var uc in ent.UpdateableComponents)
                {
                    ComponentBox.Items.Add(new ComponentCell(uc));
                }
            }

        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x202)
            {
                int x = 5;
            }
            //Console.WriteLine(m);
            base.WndProc(ref m);
        }

        private void ComponentBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private class ComponentCell
        {
            public IGameComponent Component;
            public ComponentCell(IGameComponent component)
            {
                Component = component;
            }

            public override string ToString()
            { 
                return String.Format("{0}" + Component.ToString(), (Component is EntityDrawableComponent) ? "[Drawable]" : "[Updateable]");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IGameComponent component = (ComponentBox.SelectedItem as ComponentCell)?.Component;
            if (component != null)
            {
                using (MemoryStream memStm = new MemoryStream())
                {
                    var serializer = new DataContractSerializer(component.GetType());
                    serializer.WriteObject(memStm, component);

                    memStm.Seek(0, SeekOrigin.Begin);

                    using (var streamReader = new StreamReader(memStm))
                    {
                        string result = streamReader.ReadToEnd();
                        OutputBox.AppendText(result + "\r\n");
                        using (var t = GenerateStreamFromString(result))
                        {
                            var o = serializer.ReadObject(t);
                        }
                    }
                }
            }
        }


        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

    }


}
