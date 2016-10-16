using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonogameSamples.Engine.Editor
{
    public class EntityTreeNode : TreeNode
    {
        public Entity Entity;

        public EntityTreeNode(Entity entity) : base()
        {
            Entity = entity;
            Text = "entity";

        }

    }

    class TreeViewExtension
    {
    }
}
