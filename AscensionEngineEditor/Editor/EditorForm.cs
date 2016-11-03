using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ascension.Engine.Content;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Common.Attributes;
using Ascension.Engine.Core.Common.EventArguments;
using Ascension.Engine.Core.Components;
using AscensionEditor.DialogForms;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using Ascension.Engine.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AscensionEngine.Engine.Graphics.CameraSystem;

namespace AscensionEditor
{
    public partial class EditorForm : Form
    {

        public GameEditor GameEditor;
        public Scene activeScene;
        private ContextMenu ComponentContextMenu;
        private ContextMenu MaterialReferenceContextMenu;
        private object clipboardObject;


        public EditorForm()
        {
            InitializeComponent();
            ComponentContextMenu = new ContextMenu(new[] { new MenuItem("Сopy", (s, e) => { clipboardObject = ComponentPropertyGrid.SelectedGridItem.Value; }),
                new MenuItem("Paste",
                (s, e) => 
                {
                    if (ComponentPropertyGrid.SelectedGridItem.PropertyDescriptor.PropertyType.Equals(clipboardObject.GetType())) {
                        ComponentPropertyGrid.SelectedGridItem.PropertyDescriptor.SetValue(ComponentPropertyGrid.SelectedObject,clipboardObject);
                        ComponentPropertyGrid.SelectedObject = (ComponentBox.SelectedItem as ComponentCell).Component;
                    }

                })});
            MaterialReferenceContextMenu = new ContextMenu(new[] { new MenuItem("Сopy", (s, e) => { clipboardObject = MaterialBox.SelectedItem; }),
                new MenuItem("Remove",
                (s, e) =>
                {
                    var r = MaterialBox.SelectedItem as MaterialReference;
                    r.IsValid = false;
                    activeScene.Materials.Remove(r);
                })});
        }

        public void InitializateGUI(GameEditor gameEditor)
        {
            this.GameEditor = gameEditor;
            RenderSystem rs = GameEditor.RenderSystem;
            rs.GameComponents.Where(t => (t is SceneRenderer)).ToList().ForEach(w => SceneComboBox.Items.Add((w as SceneRenderer).Scene));
            SceneComboBox.SelectedIndex = 0;
            SetFilters();
        }

        private void SetFilters()
        {

            // this fails if called directly in OnLoad since the control didn't finish creating itself: 
            Application.AddMessageFilter(new PropertyGridMessageFilter(ComponentPropertyGrid.GetChildAtPoint(new System.Drawing.Point(40, 40)),
                new MouseEventHandler(ComponentPropertyGrid_MouseUp)));
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in a.GetTypes())
                {
                    if (t.GetCustomAttribute(typeof(ComponentAttribute), true) != null)
                    {
                        AvailableComponents.Items.Add(new TypeCell(t, t.GetCustomAttribute<ComponentAttribute>().Name));
                    }
                }
            }
            ComponentPropertyGrid.MouseUp += ComponentPropertyGrid_MouseUp;
            TexturesPanel.Controls.AddRange(MaterialShower.TextureShower(6).ToArray());
        }


        private void ComponentPropertyGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ComponentPropertyGrid.SelectedGridItem != null && ComponentPropertyGrid.SelectedGridItem.PropertyDescriptor != null)
            {// the user right clicked on a property to see the context menu: 
                try
                {
                    ComponentContextMenu.Show(this, PointToClient(MousePosition));

                }
                catch
                {
                }
            }
        }

        private Scene lastScene;
        private void SceneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastScene != null)
            {
                ///
                /// Remove all handlers
                ///
                lastScene.Materials.CollectionChanged -= MaterialsChanged;
                lastScene.Shaders.CollectionChanged -= ShadersChanged;

            }
            EntityView.Nodes.Clear();

            MaterialBox.Items.Clear();
            MaterialShaderBox.Items.Clear();

            activeScene = (SceneComboBox.SelectedItem) as Scene;

            if (activeScene == null) return;

            activeScene.Materials.CollectionChanged += MaterialsChanged;
            activeScene.Shaders.CollectionChanged += ShadersChanged;
            activeScene.EnititiesChanged += EntityChanged;
            EntityView.BeginUpdate();
            foreach (var ent in activeScene.Entities)
            {
                AddEntitiesToTreeViewRec(ent, EntityView.Nodes);
                //int x = 5;
            }

            foreach (var sr in activeScene.Shaders.References())
            {
                MaterialShaderBox.Items.Add(sr);
            }

            foreach (var mr in activeScene.Materials.References())
            {
                MaterialBox.Items.Add(mr);
            }

            EntityView.EndUpdate();
            GameEditor.RenderSystem.ActiveScene = activeScene;
            lastScene = activeScene;
        }


        private void MaterialsChanged(object sender, ResourceCollectionEventArgs<string> e)
        {
            switch (e.Operation)
            {
                case Operation.Added:
                    MaterialBox.Items.Add(e.Reference);
                    break;
                case Operation.Removed:
                    MaterialBox.Items.Remove(e.Reference);
                    break;
                case Operation.Replaced:
                    //Nothing to do, we will store only MaterialReference in our MaterialBox.
                    break;
            }
        }


        private void ShadersChanged(object sender, ResourceCollectionEventArgs<string> e)
        {
            switch (e.Operation)
            {
                case Operation.Added:
                    MaterialShaderBox.Items.Add(e.Reference);
                    break;
                case Operation.Removed:
                    MaterialShaderBox.Items.Remove(e.Reference);
                    break;
                case Operation.Replaced:
                    //Nothing to do, we will store only MaterialReference in our MaterialBox.
                    break;
            }
        }

        private void EntityChanged(object sender, EventArgs e)
        {
            Console.WriteLine("EntityChanged");
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
                EntityPropertyGrid.SelectedObject = ent;
                GameEditor.SelectedEntity = ent;
                ComponentBox.Items.Add(new ComponentCell(ent.Transform));

                foreach (var dc in ent.DrawableComponents)
                {
                    ComponentBox.Items.Add(new ComponentCell(dc));
                }

                foreach (var uc in ent.UpdateableComponents)
                {
                    if (uc is Transform) continue;
                    ComponentBox.Items.Add(new ComponentCell(uc));
                }
                ComponentPropertyGrid.SelectedObject = null;
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

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private void saveSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            activeScene.Save();
        }

        private void openSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scene scene = Scene.Load(GameEditor.RenderSystem);
            scene.sceneRenderer.LoadContent(GameEditor.Content);
            SceneComboBox.Items.Add(scene);
        }

        private void addEmptyEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new GetNameForm("Entity name");
            if (form.ShowDialog() == DialogResult.OK)
            {
                var ent = new Entity(form.Name, activeScene);
                EntityView.BeginUpdate();
                int t = EntityView.Nodes.Add(new EntityTreeNode(ent));
                EntityView.EndUpdate();
            }
        }

        private void EntityView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
            
        }

        private void EntityView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void EntityView_DragDrop(object sender, DragEventArgs e)
        {
            EntityTreeNode NewNode;

            if (e.Data.GetDataPresent("MonogameSamples.Engine.Editor.EntityTreeNode", false))
            {
                System.Drawing.Point pt = ((TreeView)sender).PointToClient(new System.Drawing.Point(e.X, e.Y));
                EntityTreeNode DestinationNode = (EntityTreeNode)((TreeView)sender).GetNodeAt(pt);
                NewNode = (EntityTreeNode)e.Data.GetData("MonogameSamples.Engine.Editor.EntityTreeNode");


                if (DestinationNode == null)
                {
                    if (NewNode.Entity.Parent != null)
                    {
                        NewNode.Entity.Parent.RemoveEntity(NewNode.Entity, true);
                        NewNode.Remove();
                        EntityView.Nodes.Add(NewNode);
                    }

                } else
                if (DestinationNode.TreeView == EntityView)
                {

                    Entity ent = DestinationNode.Entity;
                    do
                    {
                        if (NewNode.Entity == ent)
                        {
                            return;
                        }
                        ent = ent.Parent;
                    } while (ent != null);

                    EntityView.BeginUpdate();
                    NewNode.Remove();
                    DestinationNode.Entity.AddEntity(NewNode.Entity);
                    DestinationNode.Nodes.Add(NewNode);
                    DestinationNode.Expand();
                    EntityView.EndUpdate();

                }
            }
        }

        private List<string> textureNames;
        

        public void SetContent()
        {
            textureNames = ContentSystem.GetInstance().Textures.Keys.ToList();
            textureNames.Insert(0, "-");

            foreach (var c in TexturesPanel.Controls)
            {
                ComboBox comboBox = c as ComboBox;
                if (comboBox != null)
                {
                    foreach (var s in textureNames)
                    {
                        comboBox.Items.Add(s);
                    }
                    comboBox.SelectedIndexChanged += TextureComboBox_SelectedIndexChanged;
                }
            }
        }

        private void MaterialBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ignoreTextureComboBox = true;
            if (MaterialBox.SelectedItem != null)
            {
                Material m = activeScene.Materials[(MaterialBox.SelectedItem as MaterialReference)];
                MaterialShaderBox.SelectedItem = m.ShaderReference;

                foreach (var c in TexturesPanel.Controls)
                {
                    PictureBox pb = c as PictureBox;
                    if (pb != null)
                    {
                        Texture2D t = m.Textures[int.Parse(pb.Tag.ToString())];
                        if (t == null)
                        {
                            pb.BackgroundImage = null;
                        }
                        else
                        {
                            pb.BackgroundImage = ConvertToImage(t);
                        }
                    }
                }
                foreach (var c in TexturesPanel.Controls)
                {
                    ComboBox cb = c as ComboBox;
                    if (cb != null)
                    {
                        var r = m.TextureReferences[int.Parse(cb.Tag.ToString())];
                        cb.SelectedItem = r == null ? "-" : r.Name;
                    }
                }
                
            } else
            {
                MaterialShaderBox.SelectedItem = null;
            }
            ignoreTextureComboBox = false;
        }

        bool ignoreTextureComboBox = false;
        private void TextureComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreTextureComboBox)
            {
                return;
            }
            if (MaterialBox.SelectedItem != null)
            {
                int index = int.Parse((sender as ComboBox).Tag.ToString());
                var textureName = (sender as ComboBox).SelectedItem.ToString();
                activeScene.Materials[(MaterialBox.SelectedItem as MaterialReference)].TextureReferences[index] = textureName.Equals("-") ? null : Texture2DReference.FromIdentifier(textureName);
                var texture = activeScene.Materials[(MaterialBox.SelectedItem as MaterialReference)].Textures[index];
                if (texture != null)
                {
                    (TexturesPanel.Controls["textureBox" + index] as PictureBox).BackgroundImage = ConvertToImage(texture);
                } else
                {
                    (TexturesPanel.Controls["textureBox" + index] as PictureBox).BackgroundImage = null;

                }
            }
        }

        private void MaterialShaderBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MaterialBox.SelectedItem != null)
            {
                var m = activeScene.Materials[(MaterialBox.SelectedItem as MaterialReference)];
                m.ShaderReference = MaterialShaderBox.SelectedItem as ShaderReference;
            }
        }


        public static Image ConvertToImage(Texture2D texture)
        {
            Image img;
            using (MemoryStream MS = new MemoryStream())
            {
                texture.SaveAsPng(MS, texture.Width, texture.Height);
                MS.Seek(0, SeekOrigin.Begin);
                img = Bitmap.FromStream(MS);
            }
            return img;
        }


        /// <summary>
        /// This code was taken from : http://www.codeproject.com/Articles/12868/Context-menu-for-the-custom-properties-in-the-C-Pr
        /// Thanks author for good article.
        /// </summary>
        public class PropertyGridMessageFilter : IMessageFilter
        {
            /// <summary>
            /// The control to monitor
            /// </summary>
            public Control Control;

            public MouseEventHandler MouseUp;

            public PropertyGridMessageFilter(Control c, MouseEventHandler meh)
            {
                this.Control = c;
                MouseUp = meh;
            }

            #region IMessageFilter Members

            public bool PreFilterMessage(ref Message m)
            {
                if (!this.Control.IsDisposed && m.HWnd == this.Control.Handle && MouseUp != null)
                {
                    MouseButtons mb = MouseButtons.None;

                    switch (m.Msg)
                    {
                        case 0x0202:/*WM_LBUTTONUP, see winuser.h*/
                            mb = MouseButtons.Left;
                            break;
                        case 0x0205:/*WM_RBUTTONUP*/
                            mb = MouseButtons.Right;
                            break;
                    }

                    if (mb != MouseButtons.None)
                    {
                        MouseEventArgs e = new MouseEventArgs(mb, 1, m.LParam.ToInt32() & 0xFFff, m.LParam.ToInt32() >> 16, 0);

                        // you can visit these pages to understand where the above formulas came from 
                        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/mouseinput/mouseinputreference/mouseinputmessages/wm_lbuttonup.asp
                        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowmacros/get_x_lparam.asp

                        MouseUp(Control, e);
                    }
                }
                return false;
            }

            #endregion
        }

        private void ComponentBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ComponentPropertyGrid.SelectedObject = (ComponentBox.SelectedItem as ComponentCell).Component;
        }


        private void MaterialBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (MaterialBox.SelectedItem != null && e.Button == MouseButtons.Right)
            {
                MaterialReferenceContextMenu.Show(this, PointToClient(MousePosition));
            }
        }

        private void addMaterialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new GetNameForm("Material name");
            if (form.ShowDialog() == DialogResult.OK)
            {
                Material m = new Material(form.Name, null, null);
                if (activeScene != null)
                {
                    activeScene.Materials.Add(m.Reference, m);
                }
            }
        }

        private void addComponent_Click(object sender, EventArgs e)
        {
            if (GameEditor.SelectedEntity != null)
            {
                if (AvailableComponents.SelectedItem != null)
                {
                    
                    var c = (AvailableComponents.SelectedItem as TypeCell).ComponentType;
                    var n = (AvailableComponents.SelectedItem as TypeCell).Name;
                    int startIndex = 1;
                    string suffix = "";
                    while (GameEditor.SelectedEntity.ContainsComponentName(n + suffix))
                    {
                        suffix = startIndex.ToString();
                        startIndex++;
                    }
                    if (c.IsSubclassOf(typeof(EntityDrawableComponent)))
                    {
                        var t = Activator.CreateInstance(c, new[] { n + suffix, null });
                        GameEditor.SelectedEntity.AddDrawableComponent((EntityDrawableComponent)t);
                    } else
                    {
                        
                        var t = Activator.CreateInstance(c, new[] { n + suffix });
                        GameEditor.SelectedEntity.AddUpdateableComponent((EntityUpdateableComponent)t);
                    }
                }
            }
        }

#warning refactor to generics.
        public class TypeCell
        {
            public Type ComponentType;
            public string Name;
            public TypeCell(Type type, string name)
            {
                ComponentType = type;
                this.Name = name;
            }

            public override string ToString()
            {
                return String.Format("{0}" + Name, (ComponentType.IsSubclassOf(typeof(EntityDrawableComponent))) ? "[Drawable]" : "[Updateable]");
            }
        }

        private void ComponentBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void EntityView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                EntityView.SelectedNode = EntityView.GetNodeAt(e.Location);
        }


        private void addAssemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Assembly.LoadFile(ofd.FileName);
            }

            AvailableComponents.Items.Clear();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in a.GetTypes())
                {
                    if (t.GetCustomAttribute(typeof(ComponentAttribute), true) != null)
                    {
                        AvailableComponents.Items.Add(new TypeCell(t, t.GetCustomAttribute<ComponentAttribute>().Name));
                    }
                }
            }
        }

        private void ComponentBox_MouseUp(object sender, MouseEventArgs e)
        {

            var contextMenu = new ContextMenu();
               
            if (e.Button == MouseButtons.Right)
            {
                int index = ComponentBox.IndexFromPoint(e.Location);
                if (index < 0) return;
                var item = (ComponentBox.Items[index] as ComponentCell).Component;
                if (item is Camera)
                {
                    contextMenu.MenuItems.Add(new MenuItem("Set as active", (s, ea) => GameEditor.renderSystem.ActiveCamera = (Camera)item));
                }
                contextMenu.Show(ComponentBox, e.Location);
            }
        }
    }


#warning refactor to generics.
    public class ComponentCell
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


}
