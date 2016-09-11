using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics;

namespace MonogameSamples.Engine.Core.Common
{
    public class EntityDrawableComponent : DrawableComponent
    {

        public IGameComponent ParentComponent { get { return parentComponent; } }




        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        private IGameComponent parentComponent;
        private Material material = null;



        public EntityDrawableComponent(IGameComponent parentComponent) : base()
        {
            this.parentComponent = parentComponent;
        }

         

    }
}
