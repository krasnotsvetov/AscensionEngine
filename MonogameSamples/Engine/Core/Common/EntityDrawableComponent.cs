using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Common
{
    [DataContract]
    public class EntityDrawableComponent : DrawableComponent
    {
        public virtual IGameComponent ParentComponent { get; set; }


        public Material Material
        {
            get { return material; }
            set { material = value; }
        }


        private Material material = null;



        public EntityDrawableComponent(IGameComponent parentComponent) : base()
        {
            this.ParentComponent = parentComponent;
        }

         

    }
}
