using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Graphics;

namespace MonogameSamples.Engine.Core.Components
{
    public class Transform : EntityUpdateableComponent
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public Transform(Entity entity) : base(entity)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
