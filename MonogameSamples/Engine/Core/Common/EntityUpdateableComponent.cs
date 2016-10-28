using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Common
{
    [DataContract]
    public class EntityUpdateableComponent : UpdateableComponent
    {
        [DataMember]
        public string Name { get; set; }

        public virtual Entity ParentComponent { get; set; }

        public EntityUpdateableComponent(String name) : base()
        {
            this.Name = name;
        }

    }
}
