using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Graphics;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Common
{
    [DataContract]
    public class EntityUpdateableComponent : UpdateableComponent
    {


        public virtual IGameComponent ParentComponent { get; set; }

        public EntityUpdateableComponent(IGameComponent parentComponent) : base()
        {
            ParentComponent = parentComponent;
        }

    }
}
