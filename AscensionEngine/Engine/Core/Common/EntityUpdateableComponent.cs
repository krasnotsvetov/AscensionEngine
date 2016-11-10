using Microsoft.Xna.Framework;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using System;
using System.Runtime.Serialization;

namespace Ascension.Engine.Core.Common
{
    [DataContract]
    public class EntityUpdateableComponent : UpdateableComponent
    {

        [DataMember]
        public string Name { get; set; }

        public virtual Entity Parent { get; set; }

        public EntityUpdateableComponent(String name) : base()
        {
            this.Name = name;
        }

        public override void Update(GameTime gameTime)
        {
            if (Parent != null)
            {
                //Update global transform
                var t = Parent.GlobalTransform;
            }
            base.Update(gameTime);
        }

    }
}
