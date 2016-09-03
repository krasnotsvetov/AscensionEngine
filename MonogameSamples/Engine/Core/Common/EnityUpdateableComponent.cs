using MonogameSamples.Engine.Graphics; 

namespace MonogameSamples.Engine.Core.Common
{
    public class EntityUpdateableComponent : UpdateableComponent
    {


        public Entity Entity { get { return entity; } }
        private Entity entity;
        public EntityUpdateableComponent(Entity entity) : base()
        {
            this.entity = entity;
        }

    }
}
