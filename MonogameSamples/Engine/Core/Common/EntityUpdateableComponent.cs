using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Graphics;

namespace MonogameSamples.Engine.Core.Common
{
    public class EntityUpdateableComponent : UpdateableComponent
    {


        public IGameComponent ParentComponent { get { return parentComponent; } }
        private IGameComponent parentComponent;
        public EntityUpdateableComponent(IGameComponent parentComponent) : base()
        {
            this.parentComponent = parentComponent;
        }

    }
}
