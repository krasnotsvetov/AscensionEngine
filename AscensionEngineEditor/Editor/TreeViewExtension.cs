using Ascension.Engine.Graphics.SceneSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AscensionEditor
{
    public class EntityTreeNode : TreeNode
    {
        public Entity Entity;

        public EntityTreeNode(Entity entity) : base()
        {
            Entity = entity;
            Text = "entity";

        }
        public override object Clone()
        {
            
            var t = base.Clone();
            ((EntityTreeNode)t).Entity = Entity;
            return t;
        }
    }

    class TreeViewExtension
    {
    }
}
