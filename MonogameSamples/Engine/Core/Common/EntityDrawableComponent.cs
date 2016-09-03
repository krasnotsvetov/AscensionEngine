using MonogameSamples.Engine.Graphics;

namespace MonogameSamples.Engine.Core.Common
{
    public class EntityDrawableComponent : DrawableComponent
    {

        public Entity Entity { get { return entity; } }


        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        private Entity entity;
        private Material material = null;

        public EntityDrawableComponent(Entity entity) : base()
        {
            this.entity = entity;
        }

         

    }
}
