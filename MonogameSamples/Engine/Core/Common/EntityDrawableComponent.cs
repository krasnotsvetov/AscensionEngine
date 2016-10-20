using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Common
{
    [DataContract]
    public class EntityDrawableComponent : DrawableComponent
    {
        public virtual IGameComponent ParentComponent { get; set; }

        [DataMember]
        public string Name;


        [DataMember]
        public MaterialReference MaterialReference { get; internal set; }



        public Material Material { get { return material; } }

        private Material material;



        public EntityDrawableComponent(string name, MaterialReference materialReference) : base()
        {
            this.MaterialReference = materialReference;
            this.Name = name;
        }


        public override void Initialize()
        {
            if (MaterialReference != null)
            {
                material = (ParentComponent as Entity).Scene.RenderSystem.Materials[MaterialReference];
            }
            base.Initialize();
        }

        internal virtual void RenderSystemChange()
        {
            if (MaterialReference != null)
            {
                material = (ParentComponent as Scene2D).RenderSystem.Materials[MaterialReference];
            }
        }
    }
}
